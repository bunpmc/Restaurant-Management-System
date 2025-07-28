using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Services;
using System.ComponentModel;

namespace SushiRestaurant.Reports
{
    public partial class ReportsWindow : Window
    {
        private readonly IOrderService _orderService;
        private readonly IMenuService _menuService;
        private readonly IEmployeeService _employeeService;
        private readonly ITableService _tableService;
        private readonly IReportService _reportService;

        public ReportsWindow()
        {
            InitializeComponent();
            _orderService = new OrderService();
            _menuService = new MenuService();
            _employeeService = new EmployeeService();
            _tableService = new TableService();
            _reportService = new ReportService();

            InitializeDateFilters();
            LoadReportsData();
        }

        private void InitializeDateFilters()
        {
            dpFromDate.SelectedDate = DateTime.Today.AddDays(-30);
            dpToDate.SelectedDate = DateTime.Today;

            cmbReportPeriod.SelectionChanged += CmbReportPeriod_SelectionChanged;
        }

        private void CmbReportPeriod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbReportPeriod.SelectedItem is ComboBoxItem selectedItem)
            {
                switch (selectedItem.Content.ToString())
                {
                    case "Today":
                        dpFromDate.SelectedDate = DateTime.Today;
                        dpToDate.SelectedDate = DateTime.Today;
                        break;
                    case "This Week":
                        var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                        dpFromDate.SelectedDate = startOfWeek;
                        dpToDate.SelectedDate = DateTime.Today;
                        break;
                    case "This Month":
                        dpFromDate.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        dpToDate.SelectedDate = DateTime.Today;
                        break;
                    case "Custom Range":
                        // User can manually select dates
                        break;
                }
            }
        }

        private void LoadReportsData()
        {
            try
            {
                LoadSalesOverview();
                LoadMenuPerformance();
                LoadEmployeePerformance();
                LoadTableUtilization();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reports data: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSalesOverview()
        {
            try
            {
                var fromDate = dpFromDate.SelectedDate ?? DateTime.Today.AddDays(-30);
                var toDate = dpToDate.SelectedDate ?? DateTime.Today;

                // Calculate key metrics using ReportService
                var totalSales = _reportService.GetTotalSalesByDateRange(fromDate, toDate);
                var totalOrders = _reportService.GetOrderCountByDateRange(fromDate, toDate);
                var averageOrder = _reportService.GetAverageOrderValueByDateRange(fromDate, toDate);
                var activeTables = GetActiveTablesCount();

                // Update UI
                txtTotalSales.Text = totalSales.ToString("C");
                txtTotalOrders.Text = totalOrders.ToString();
                txtAverageOrder.Text = averageOrder.ToString("C");
                txtActiveTables.Text = activeTables.ToString();

                // Load daily sales data
                var salesData = _reportService.GetDailySalesData(fromDate, toDate);
                var salesViewModels = salesData.Select(sd => new SalesDataViewModel
                {
                    Date = sd.Date,
                    OrderCount = sd.OrderCount,
                    TotalSales = sd.TotalSales,
                    AverageOrder = sd.AverageOrder,
                    TopSellingItem = sd.TopSellingItem
                }).ToList();

                dgSalesData.ItemsSource = salesViewModels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading sales overview: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMenuPerformance()
        {
            try
            {
                var fromDate = dpFromDate.SelectedDate ?? DateTime.Today.AddDays(-30);
                var toDate = dpToDate.SelectedDate ?? DateTime.Today;

                var menuPerformanceData = _reportService.GetMenuPerformanceByDateRange(fromDate, toDate);
                var menuPerformanceViewModels = menuPerformanceData.Select((mp, index) => new MenuPerformanceViewModel
                {
                    Rank = index + 1,
                    MenuItemName = mp.MenuItemName,
                    Category = mp.CategoryName,
                    QuantitySold = mp.QuantitySold,
                    Revenue = mp.Revenue,
                    AveragePrice = mp.AveragePrice
                }).ToList();

                dgMenuPerformance.ItemsSource = menuPerformanceViewModels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading menu performance: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadEmployeePerformance()
        {
            try
            {
                var fromDate = dpFromDate.SelectedDate ?? DateTime.Today.AddDays(-30);
                var toDate = dpToDate.SelectedDate ?? DateTime.Today;

                var employeePerformanceData = _reportService.GetEmployeePerformanceByDateRange(fromDate, toDate);
                var employeePerformanceViewModels = employeePerformanceData.Select(ep => new EmployeePerformanceViewModel
                {
                    EmployeeName = ep.EmployeeName,
                    Role = ep.Role,
                    OrdersHandled = ep.OrdersHandled,
                    TotalSales = ep.TotalSales,
                    AverageOrderValue = ep.AverageOrderValue,
                    LastActive = ep.LastActive
                }).ToList();

                dgEmployeePerformance.ItemsSource = employeePerformanceViewModels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employee performance: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTableUtilization()
        {
            try
            {
                var fromDate = dpFromDate.SelectedDate ?? DateTime.Today.AddDays(-30);
                var toDate = dpToDate.SelectedDate ?? DateTime.Today;

                var tableUtilizationData = _reportService.GetTableUtilizationByDateRange(fromDate, toDate);
                var tableUtilizationViewModels = tableUtilizationData.Select(tu => new TableUtilizationViewModel
                {
                    TableNumber = tu.TableNumber,
                    Capacity = tu.Capacity,
                    OrdersServed = tu.OrdersServed,
                    Revenue = tu.Revenue,
                    AverageOrderValue = tu.AverageOrderValue,
                    Status = tu.Status,
                    Note = tu.Note
                }).ToList();

                dgTableUtilization.ItemsSource = tableUtilizationViewModels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading table utilization: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Helper method for getting active tables count
        private int GetActiveTablesCount()
        {
            try
            {
                var tables = _tableService.GetTables();
                return tables.Count(t => t.Status != "Available");
            }
            catch
            {
                return 0;
            }
        }



        private void btnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            LoadReportsData();
            MessageBox.Show("Report generated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnExportPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fromDate = dpFromDate.SelectedDate ?? DateTime.Today.AddDays(-30);
                var toDate = dpToDate.SelectedDate ?? DateTime.Today;

                var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    DefaultExt = "pdf",
                    FileName = $"SalesReport_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}.pdf"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    // For now, save as text file since PDF library is not available
                    var reportText = _reportService.GenerateSalesReportText(fromDate, toDate);
                    System.IO.File.WriteAllText(saveDialog.FileName.Replace(".pdf", ".txt"), reportText);
                    MessageBox.Show($"Report exported to: {saveDialog.FileName.Replace(".pdf", ".txt")}",
                        "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting PDF: {ex.Message}", "Export Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExportExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fromDate = dpFromDate.SelectedDate ?? DateTime.Today.AddDays(-30);
                var toDate = dpToDate.SelectedDate ?? DateTime.Today;

                var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx|CSV files (*.csv)|*.csv",
                    DefaultExt = "csv",
                    FileName = $"SalesReport_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}.csv"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    ExportToCSV(fromDate, toDate, saveDialog.FileName);
                    MessageBox.Show($"Report exported to: {saveDialog.FileName}",
                        "Export Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting Excel: {ex.Message}", "Export Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fromDate = dpFromDate.SelectedDate ?? DateTime.Today.AddDays(-30);
                var toDate = dpToDate.SelectedDate ?? DateTime.Today;
                var reportText = _reportService.GenerateSalesReportText(fromDate, toDate);

                // Create a simple print dialog
                var printDialog = new System.Windows.Controls.PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    var flowDoc = new System.Windows.Documents.FlowDocument();
                    var paragraph = new System.Windows.Documents.Paragraph();
                    paragraph.Inlines.Add(new System.Windows.Documents.Run(reportText));
                    flowDoc.Blocks.Add(paragraph);

                    var paginator = ((System.Windows.Documents.IDocumentPaginatorSource)flowDoc).DocumentPaginator;
                    printDialog.PrintDocument(paginator, "Sales Report");

                    MessageBox.Show("Report sent to printer successfully!", "Print Successful",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing report: {ex.Message}", "Print Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToCSV(DateTime fromDate, DateTime toDate, string filePath)
        {
            try
            {
                var salesData = _reportService.GetDailySalesData(fromDate, toDate);
                var menuData = _reportService.GetMenuPerformanceByDateRange(fromDate, toDate);
                var employeeData = _reportService.GetEmployeePerformanceByDateRange(fromDate, toDate);

                using (var writer = new System.IO.StreamWriter(filePath))
                {
                    // Sales Summary
                    writer.WriteLine("SALES REPORT");
                    writer.WriteLine($"Period: {fromDate:MM/dd/yyyy} - {toDate:MM/dd/yyyy}");
                    writer.WriteLine($"Generated: {DateTime.Now:MM/dd/yyyy HH:mm}");
                    writer.WriteLine();

                    // Summary metrics
                    var totalSales = _reportService.GetTotalSalesByDateRange(fromDate, toDate);
                    var orderCount = _reportService.GetOrderCountByDateRange(fromDate, toDate);
                    var avgOrder = _reportService.GetAverageOrderValueByDateRange(fromDate, toDate);

                    writer.WriteLine("SUMMARY");
                    writer.WriteLine($"Total Sales,{totalSales:C}");
                    writer.WriteLine($"Total Orders,{orderCount}");
                    writer.WriteLine($"Average Order,{avgOrder:C}");
                    writer.WriteLine();

                    // Daily Sales Data
                    writer.WriteLine("DAILY SALES");
                    writer.WriteLine("Date,Orders,Total Sales,Average Order,Top Item");
                    foreach (var day in salesData)
                    {
                        writer.WriteLine($"{day.Date:MM/dd/yyyy},{day.OrderCount},{day.TotalSales:F2},{day.AverageOrder:F2},{day.TopSellingItem}");
                    }
                    writer.WriteLine();

                    // Menu Performance
                    writer.WriteLine("MENU PERFORMANCE");
                    writer.WriteLine("Rank,Menu Item,Category,Quantity Sold,Revenue,Average Price");
                    for (int i = 0; i < menuData.Count; i++)
                    {
                        var item = menuData[i];
                        writer.WriteLine($"{i + 1},{item.MenuItemName},{item.CategoryName},{item.QuantitySold},{item.Revenue:F2},{item.AveragePrice:F2}");
                    }
                    writer.WriteLine();

                    // Employee Performance
                    writer.WriteLine("EMPLOYEE PERFORMANCE");
                    writer.WriteLine("Employee,Role,Orders Handled,Total Sales,Average Order Value,Last Active");
                    foreach (var emp in employeeData)
                    {
                        writer.WriteLine($"{emp.EmployeeName},{emp.Role},{emp.OrdersHandled},{emp.TotalSales:F2},{emp.AverageOrderValue:F2},{emp.LastActive:MM/dd/yyyy}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to export CSV: {ex.Message}");
            }
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }

    // View Models for data binding
    public class SalesDataViewModel
    {
        public DateTime Date { get; set; }
        public int OrderCount { get; set; }
        public decimal TotalSales { get; set; }
        public decimal AverageOrder { get; set; }
        public string TopSellingItem { get; set; } = "";
    }

    public class MenuPerformanceViewModel
    {
        public int Rank { get; set; }
        public string MenuItemName { get; set; } = "";
        public string Category { get; set; } = "";
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public decimal AveragePrice { get; set; }
    }

    public class EmployeePerformanceViewModel
    {
        public string EmployeeName { get; set; } = "";
        public string Role { get; set; } = "";
        public int OrdersHandled { get; set; }
        public decimal TotalSales { get; set; }
        public decimal AverageOrderValue { get; set; }
        public DateTime LastActive { get; set; }
    }

    public class TableUtilizationViewModel
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public int OrdersServed { get; set; }
        public decimal Revenue { get; set; }
        public decimal AverageOrderValue { get; set; }
        public string Status { get; set; } = "";
        public string Note { get; set; } = "";
    }
}
