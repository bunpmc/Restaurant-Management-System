using System;
using System.Collections.Generic;
using System.Linq;
using BusinessObjects;
using DataAccessLayer;

namespace Services
{
    public class ReportService : IReportService
    {
        private readonly OrderDAO _orderDAO;
        private readonly OrderItemDAO _orderItemDAO;
        private readonly EmployeeDAO _employeeDAO;
        private readonly TableDAO _tableDAO;

        public ReportService()
        {
            _orderDAO = new OrderDAO();
            _orderItemDAO = new OrderItemDAO();
            _employeeDAO = new EmployeeDAO();
            _tableDAO = new TableDAO();
        }

        // Sales Reports
        public decimal GetTotalSalesByDateRange(DateTime fromDate, DateTime toDate)
        {
            var orders = _orderDAO.GetOrdersByDateRange(fromDate, toDate)
                .Where(o => o.Status == "Completed");
            return orders.Sum(o => o.TotalAmount ?? 0);
        }

        public int GetOrderCountByDateRange(DateTime fromDate, DateTime toDate)
        {
            var orders = _orderDAO.GetOrdersByDateRange(fromDate, toDate)
                .Where(o => o.Status == "Completed");
            return orders.Count();
        }

        public decimal GetAverageOrderValueByDateRange(DateTime fromDate, DateTime toDate)
        {
            var orders = _orderDAO.GetOrdersByDateRange(fromDate, toDate)
                .Where(o => o.Status == "Completed").ToList();

            if (orders.Count == 0) return 0;
            return orders.Sum(o => o.TotalAmount ?? 0) / orders.Count;
        }

        public List<DailySalesData> GetDailySalesData(DateTime fromDate, DateTime toDate)
        {
            var orders = _orderDAO.GetOrdersByDateRange(fromDate, toDate)
                .Where(o => o.Status == "Completed").ToList();

            var salesData = new List<DailySalesData>();

            for (var date = fromDate.Date; date <= toDate.Date; date = date.AddDays(1))
            {
                var dayOrders = orders.Where(o => o.OrderTime?.Date == date).ToList();
                var totalSales = dayOrders.Sum(o => o.TotalAmount ?? 0);
                var orderCount = dayOrders.Count;
                var avgOrder = orderCount > 0 ? totalSales / orderCount : 0;

                // Get top selling item for the day
                var topItem = GetTopSellingItemForDate(date);

                salesData.Add(new DailySalesData
                {
                    Date = date,
                    OrderCount = orderCount,
                    TotalSales = totalSales,
                    AverageOrder = avgOrder,
                    TopSellingItem = topItem
                });
            }

            return salesData;
        }

        // Menu Performance Reports
        public List<MenuPerformanceData> GetMenuPerformanceByDateRange(DateTime fromDate, DateTime toDate)
        {
            return _orderItemDAO.GetMenuPerformanceByDateRange(fromDate, toDate);
        }

        public List<MenuPerformanceData> GetTopSellingItems(DateTime fromDate, DateTime toDate, int topCount = 10)
        {
            return GetMenuPerformanceByDateRange(fromDate, toDate)
                .OrderByDescending(mp => mp.QuantitySold)
                .Take(topCount)
                .ToList();
        }

        // Employee Performance Reports
        public List<EmployeePerformanceData> GetEmployeePerformanceByDateRange(DateTime fromDate, DateTime toDate)
        {
            var orders = _orderDAO.GetOrdersByDateRange(fromDate, toDate)
                .Where(o => o.Status == "Completed").ToList();

            var employees = _employeeDAO.GetAllEmployees();
            var performanceData = new List<EmployeePerformanceData>();

            foreach (var employee in employees)
            {
                var employeeOrders = orders.Where(o => o.EmployeeId == employee.Id).ToList();
                var totalSales = employeeOrders.Sum(o => o.TotalAmount ?? 0);
                var orderCount = employeeOrders.Count;
                var avgOrderValue = orderCount > 0 ? totalSales / orderCount : 0;
                var lastActive = employeeOrders.Any() ?
                    employeeOrders.Max(o => o.OrderTime ?? DateTime.MinValue) :
                    DateTime.MinValue;

                performanceData.Add(new EmployeePerformanceData
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.FullName,
                    Role = employee.Role,
                    OrdersHandled = orderCount,
                    TotalSales = totalSales,
                    AverageOrderValue = avgOrderValue,
                    LastActive = lastActive
                });
            }

            return performanceData.OrderByDescending(ep => ep.TotalSales).ToList();
        }

        // Table Utilization Reports
        public List<TableUtilizationData> GetTableUtilizationByDateRange(DateTime fromDate, DateTime toDate)
        {
            var orders = _orderDAO.GetOrdersByDateRange(fromDate, toDate)
                .Where(o => o.Status == "Completed").ToList();

            var tables = _tableDAO.GetTables();
            var utilizationData = new List<TableUtilizationData>();

            foreach (var table in tables)
            {
                var tableOrders = orders.Where(o => o.TableId == table.Id).ToList();
                var revenue = tableOrders.Sum(o => o.TotalAmount ?? 0);
                var orderCount = tableOrders.Count;
                var avgOrderValue = orderCount > 0 ? revenue / orderCount : 0;

                utilizationData.Add(new TableUtilizationData
                {
                    TableId = table.Id,
                    TableNumber = table.TableNumber,
                    Capacity = table.Capacity ?? 4, // Default capacity
                    OrdersServed = orderCount,
                    Revenue = revenue,
                    AverageOrderValue = avgOrderValue,
                    Status = table.Status ?? "Available",
                    Note = table.Note ?? ""
                });
            }

            return utilizationData.OrderByDescending(tu => tu.Revenue).ToList();
        }

        // Helper methods
        private string GetTopSellingItemForDate(DateTime date)
        {
            try
            {
                var orderItems = _orderItemDAO.GetOrderItemsByDateRange(date, date)
                    .Where(oi => oi.Order.Status == "Completed");

                if (!orderItems.Any()) return "No sales";

                var topItem = orderItems
                    .GroupBy(oi => oi.MenuItem.Name)
                    .OrderByDescending(g => g.Sum(oi => oi.Quantity))
                    .FirstOrDefault();

                return topItem?.Key ?? "No sales";
            }
            catch
            {
                return "No sales";
            }
        }

        // Export functionality
        public bool ExportSalesReportToPdf(DateTime fromDate, DateTime toDate, string filePath)
        {
            // TODO: Implement PDF export using a library like iTextSharp
            return false;
        }

        public bool ExportSalesReportToExcel(DateTime fromDate, DateTime toDate, string filePath)
        {
            // TODO: Implement Excel export using a library like EPPlus
            return false;
        }

        public string GenerateSalesReportText(DateTime fromDate, DateTime toDate)
        {
            var report = $"SALES REPORT\n";
            report += $"Period: {fromDate:MM/dd/yyyy} - {toDate:MM/dd/yyyy}\n";
            report += $"Generated: {DateTime.Now:MM/dd/yyyy HH:mm}\n\n";

            // Summary
            var totalSales = GetTotalSalesByDateRange(fromDate, toDate);
            var orderCount = GetOrderCountByDateRange(fromDate, toDate);
            var avgOrder = GetAverageOrderValueByDateRange(fromDate, toDate);

            report += $"SUMMARY:\n";
            report += $"Total Sales: {totalSales:C}\n";
            report += $"Total Orders: {orderCount}\n";
            report += $"Average Order: {avgOrder:C}\n\n";

            // Daily breakdown
            var dailyData = GetDailySalesData(fromDate, toDate);
            report += $"DAILY BREAKDOWN:\n";
            foreach (var day in dailyData)
            {
                report += $"{day.Date:MM/dd/yyyy}: {day.OrderCount} orders, {day.TotalSales:C} sales, Top: {day.TopSellingItem}\n";
            }

            return report;
        }
    }
}
