using System;
using System.Collections.Generic;
using BusinessObjects;
using DataAccessLayer;

namespace Services
{
    public interface IReportService
    {
        // Sales Reports
        decimal GetTotalSalesByDateRange(DateTime fromDate, DateTime toDate);
        int GetOrderCountByDateRange(DateTime fromDate, DateTime toDate);
        decimal GetAverageOrderValueByDateRange(DateTime fromDate, DateTime toDate);
        List<DailySalesData> GetDailySalesData(DateTime fromDate, DateTime toDate);
        
        // Menu Performance Reports
        List<MenuPerformanceData> GetMenuPerformanceByDateRange(DateTime fromDate, DateTime toDate);
        List<MenuPerformanceData> GetTopSellingItems(DateTime fromDate, DateTime toDate, int topCount = 10);
        
        // Employee Performance Reports
        List<EmployeePerformanceData> GetEmployeePerformanceByDateRange(DateTime fromDate, DateTime toDate);
        
        // Table Utilization Reports
        List<TableUtilizationData> GetTableUtilizationByDateRange(DateTime fromDate, DateTime toDate);
        
        // Export functionality
        bool ExportSalesReportToPdf(DateTime fromDate, DateTime toDate, string filePath);
        bool ExportSalesReportToExcel(DateTime fromDate, DateTime toDate, string filePath);
        string GenerateSalesReportText(DateTime fromDate, DateTime toDate);
    }

    // Data models for reports
    public class DailySalesData
    {
        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalSales { get; set; }
        public decimal AverageOrder { get; set; }
        public string TopSellingItem { get; set; } = "";
    }

    public class EmployeePerformanceData
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = "";
        public string Role { get; set; } = "";
        public int OrdersHandled { get; set; }
        public decimal TotalSales { get; set; }
        public decimal AverageOrderValue { get; set; }
        public DateTime LastActive { get; set; }
    }

    public class TableUtilizationData
    {
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public int OrdersServed { get; set; }
        public decimal Revenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public string Status { get; set; } = "";
        public string Note { get; set; } = "";
    }
}
