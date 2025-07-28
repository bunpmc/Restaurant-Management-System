using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Services;
using System.ComponentModel;

namespace SushiRestaurant.Orders
{
    public partial class OrderListWindow : Window
    {
        private readonly IOrderService _orderService;
        private readonly IEmployeeService _employeeService;
        private readonly ITableService _tableService;
        private readonly ICustomerService _customerService;
        private readonly IMenuService _menuService;

        private List<OrderViewModel> _allOrders;
        private List<OrderViewModel> _filteredOrders;
        private OrderViewModel _selectedOrder;

        public OrderListWindow()
        {
            InitializeComponent();
            _orderService = new OrderService();
            _employeeService = new EmployeeService();
            _tableService = new TableService();
            _customerService = new CustomerService();
            _menuService = new MenuService();

            InitializeFilters();
            LoadOrders();
        }

        private void InitializeFilters()
        {
            // Load employees for filter
            LoadEmployeeFilter();

            // Load tables for filter
            LoadTableFilter();

            // Set default date to today
            dpDateFilter.SelectedDate = DateTime.Today;
        }

        private void LoadEmployeeFilter()
        {
            try
            {
                // This would call the employee service to get all employees
                var employees = GetAllEmployees();

                cmbEmployeeFilter.Items.Clear();
                cmbEmployeeFilter.Items.Add(new ComboBoxItem { Content = "All Employees", Tag = null });

                foreach (var employee in employees)
                {
                    cmbEmployeeFilter.Items.Add(new ComboBoxItem
                    {
                        Content = employee.FullName,
                        Tag = employee.Id
                    });
                }

                cmbEmployeeFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTableFilter()
        {
            try
            {
                // This would call the table service to get all tables
                var tables = GetAllTables();

                cmbTableFilter.Items.Clear();
                cmbTableFilter.Items.Add(new ComboBoxItem { Content = "All Tables", Tag = null });

                foreach (var table in tables)
                {
                    cmbTableFilter.Items.Add(new ComboBoxItem
                    {
                        Content = $"Table {table.TableNumber}",
                        Tag = table.Id
                    });
                }

                cmbTableFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tables: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadOrders()
        {
            try
            {
                // Get all orders from database
                _allOrders = GetAllOrdersFromDatabase();
                _filteredOrders = new List<OrderViewModel>(_allOrders);

                ApplyFilters();
                UpdateOrderDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<OrderViewModel> GetAllOrdersFromDatabase()
        {
            try
            {
                var orders = _orderService.GetAllOrders();
                var orderViewModels = new List<OrderViewModel>();

                foreach (var order in orders)
                {
                    var orderViewModel = new OrderViewModel
                    {
                        Id = order.Id,
                        TableNumber = order.Table?.TableNumber ?? 0,
                        EmployeeName = order.Employee?.FullName ?? "Unknown",
                        CustomerName = order.Customer?.FullName ?? "Walk-in Customer",
                        OrderTime = order.OrderTime ?? DateTime.Now,
                        Status = order.Status ?? "Pending",
                        TotalAmount = order.TotalAmount ?? 0
                    };
                    orderViewModels.Add(orderViewModel);
                }

                return orderViewModels.OrderByDescending(o => o.OrderTime).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders from database: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<OrderViewModel>();
            }
        }

        private List<Employee> GetAllEmployees()
        {
            try
            {
                return _employeeService.GetAllEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<Employee>();
            }
        }

        private List<BusinessObjects.Table> GetAllTables()
        {
            try
            {
                return _tableService.GetTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tables: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<BusinessObjects.Table>();
            }
        }

        private void ApplyFilters()
        {
            if (_allOrders == null)
            {
                _filteredOrders = new List<OrderViewModel>();
                return;
            }

            _filteredOrders = new List<OrderViewModel>(_allOrders);

            // Apply status filter
            if (cmbStatusFilter.SelectedItem is ComboBoxItem statusItem && statusItem.Content.ToString() != "All")
            {
                var selectedStatus = statusItem.Content.ToString();
                _filteredOrders = _filteredOrders.Where(o => o.Status == selectedStatus).ToList();
            }

            // Apply employee filter
            if (cmbEmployeeFilter.SelectedItem is ComboBoxItem empItem && empItem.Tag != null)
            {
                var selectedEmployeeName = empItem.Content.ToString();
                _filteredOrders = _filteredOrders.Where(o => o.EmployeeName == selectedEmployeeName).ToList();
            }

            // Apply table filter
            if (cmbTableFilter.SelectedItem is ComboBoxItem tableItem && tableItem.Tag != null)
            {
                var selectedTableId = (int)tableItem.Tag;
                _filteredOrders = _filteredOrders.Where(o => o.TableNumber == selectedTableId).ToList();
            }

            // Apply date filter
            if (dpDateFilter.SelectedDate.HasValue)
            {
                var selectedDate = dpDateFilter.SelectedDate.Value.Date;
                _filteredOrders = _filteredOrders.Where(o => o.OrderTime.Date == selectedDate).ToList();
            }

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var searchTerm = txtSearch.Text.ToLower();
                _filteredOrders = _filteredOrders.Where(o =>
                    o.Id.ToString().Contains(searchTerm) ||
                    (o.CustomerName?.ToLower().Contains(searchTerm) ?? false) ||
                    (o.EmployeeName?.ToLower().Contains(searchTerm) ?? false)
                ).ToList();
            }

            // Sort by order time descending
            _filteredOrders = _filteredOrders.OrderByDescending(o => o.OrderTime).ToList();
        }

        private void UpdateOrderDisplay()
        {
            dgOrders.ItemsSource = _filteredOrders;
            UpdateStatusBar();

            // Clear order details
            ClearOrderDetails();
        }

        private void UpdateStatusBar()
        {
            if (_allOrders == null)
            {
                txtOrderCount.Text = "Total: 0 orders";
                txtStatus.Text = "Ready";
                return;
            }

            var totalCount = _allOrders.Count;
            var displayedCount = _filteredOrders?.Count ?? 0;
            var pendingCount = _allOrders.Count(o => o.Status == "Pending");
            var completedCount = _allOrders.Count(o => o.Status == "Completed");

            txtOrderCount.Text = $"Total: {totalCount} orders ({pendingCount} pending, {completedCount} completed)";

            if (_filteredOrders != null && _filteredOrders.Count != totalCount)
            {
                txtStatus.Text = $"Showing {displayedCount} of {totalCount} orders";
            }
            else
            {
                txtStatus.Text = "Ready";
            }
        }

        private void ClearOrderDetails()
        {
            txtOrderInfo.Text = "Select an order to view details";
            txtOrderDetails.Text = "";
            dgOrderItems.ItemsSource = null;
            cmbOrderStatus.SelectedIndex = 0;
            _selectedOrder = null;
        }

        private void LoadOrderDetails(OrderViewModel order)
        {
            _selectedOrder = order;

            txtOrderInfo.Text = $"Order #{order.Id} Details";
            txtOrderDetails.Text = $"Table: {order.TableNumber}\n" +
                                  $"Employee: {order.EmployeeName}\n" +
                                  $"Customer: {order.CustomerName}\n" +
                                  $"Order Time: {order.OrderTime:MM/dd/yyyy HH:mm}\n" +
                                  $"Status: {order.Status}\n" +
                                  $"Total: {order.TotalAmount:C}";

            // Set status combo box
            for (int i = 0; i < cmbOrderStatus.Items.Count; i++)
            {
                if (((ComboBoxItem)cmbOrderStatus.Items[i]).Content.ToString() == order.Status)
                {
                    cmbOrderStatus.SelectedIndex = i;
                    break;
                }
            }

            // Load order items
            LoadOrderItems(order.Id);
        }

        private void LoadOrderItems(int orderId)
        {
            try
            {
                var order = _orderService.GetOrderById(orderId);
                if (order != null && order.OrderItems != null)
                {
                    var orderItemViewModels = order.OrderItems.Select(oi => new OrderItemViewModel
                    {
                        MenuItemName = oi.MenuItem?.Name ?? "Unknown Item",
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        TotalPrice = oi.Quantity * oi.UnitPrice
                    }).ToList();

                    dgOrderItems.ItemsSource = orderItemViewModels;
                }
                else
                {
                    dgOrderItems.ItemsSource = new List<OrderItemViewModel>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order items: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                dgOrderItems.ItemsSource = new List<OrderItemViewModel>();
            }
        }



        // Event Handlers
        private void dgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgOrders.SelectedItem is OrderViewModel selectedOrder)
            {
                LoadOrderDetails(selectedOrder);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
            UpdateOrderDisplay();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
            UpdateOrderDisplay();
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
            ApplyFilters();
            UpdateOrderDisplay();
        }

        private void cmbStatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                ApplyFilters();
                UpdateOrderDisplay();
            }
        }

        private void cmbEmployeeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                ApplyFilters();
                UpdateOrderDisplay();
            }
        }

        private void cmbTableFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                ApplyFilters();
                UpdateOrderDisplay();
            }
        }

        private void dpDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                ApplyFilters();
                UpdateOrderDisplay();
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void btnUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrder != null && cmbOrderStatus.SelectedItem is ComboBoxItem statusItem)
            {
                var newStatus = statusItem.Content.ToString();

                try
                {
                    // Get the actual order from database
                    var order = _orderService.GetOrderById(_selectedOrder.Id);
                    if (order != null)
                    {
                        // Update the order status
                        order.Status = newStatus;
                        bool success = _orderService.UpdateOrder(order);

                        if (success)
                        {
                            _selectedOrder.Status = newStatus;
                            MessageBox.Show($"Order #{_selectedOrder.Id} status updated to {newStatus}", "Status Updated",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                            // Refresh the display
                            ApplyFilters();
                            UpdateOrderDisplay();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update order status. Please try again.", "Update Failed",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Order not found in database.", "Order Not Found",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating order status: {ex.Message}", "Database Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnViewBill_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrder != null)
            {
                try
                {
                    var order = _orderService.GetOrderById(_selectedOrder.Id);
                    if (order != null)
                    {
                        GenerateAndShowBill(order);
                    }
                    else
                    {
                        MessageBox.Show("Order not found in database.", "Order Not Found",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error generating bill: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void GenerateAndShowBill(Order order)
        {
            var billText = new StringBuilder();
            billText.AppendLine("=== SAKANA HOUSE RESTAURANT ===");
            billText.AppendLine("================================");
            billText.AppendLine();
            billText.AppendLine($"Order #: {order.Id}");
            billText.AppendLine($"Table: {order.Table?.TableNumber ?? 0}");
            billText.AppendLine($"Customer: {order.Customer?.FullName ?? "Walk-in Customer"}");
            billText.AppendLine($"Waiter: {order.Employee?.FullName ?? "Unknown"}");
            billText.AppendLine($"Date: {order.OrderTime?.ToString("yyyy-MM-dd HH:mm") ?? "Unknown"}");
            billText.AppendLine($"Status: {order.Status}");
            billText.AppendLine();
            billText.AppendLine("ITEMS ORDERED:");
            billText.AppendLine("--------------------------------");

            decimal subtotal = 0;
            if (order.OrderItems != null)
            {
                foreach (var item in order.OrderItems)
                {
                    var itemTotal = item.Quantity * item.UnitPrice;
                    billText.AppendLine($"{item.MenuItem?.Name ?? "Unknown Item"}");
                    billText.AppendLine($"  Qty: {item.Quantity} x ${item.UnitPrice:F2} = ${itemTotal:F2}");
                    subtotal += itemTotal;
                }
            }

            billText.AppendLine("--------------------------------");
            billText.AppendLine($"Subtotal: ${subtotal:F2}");
            billText.AppendLine($"Tax (10%): ${subtotal * 0.1m:F2}");
            billText.AppendLine($"TOTAL: ${subtotal * 1.1m:F2}");
            billText.AppendLine();
            billText.AppendLine("Thank you for dining with us!");
            billText.AppendLine("================================");

            // Show bill in a message box (in a real app, you might want to open a new window or print)
            MessageBox.Show(billText.ToString(), $"Bill for Order #{order.Id}",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnPrintOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedOrder != null)
            {
                MessageBox.Show($"Print order functionality for Order #{_selectedOrder.Id} would be implemented here.",
                    "Print Order", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // TODO: Open Order Management window for new order
                // var orderWindow = new OrderManagementWindow();
                // orderWindow.ShowDialog();

                MessageBox.Show("New Order functionality will be implemented when Order Management window is created.",
                    "New Order", MessageBoxButton.OK, MessageBoxImage.Information);

                // Refresh orders after closing order window
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening new order window: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExportOrders_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_filteredOrders == null || !_filteredOrders.Any())
                {
                    MessageBox.Show("No orders to export.", "Export Orders",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt",
                    DefaultExt = "csv",
                    FileName = $"Orders_Export_{DateTime.Now:yyyyMMdd_HHmmss}"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    ExportOrdersToFile(saveFileDialog.FileName);
                    MessageBox.Show($"Orders exported successfully to:\n{saveFileDialog.FileName}", "Export Complete",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting orders: {ex.Message}", "Export Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportOrdersToFile(string fileName)
        {
            var exportText = new StringBuilder();

            // Add header
            exportText.AppendLine("=== SAKANA HOUSE RESTAURANT ===");
            exportText.AppendLine("Orders Export Report");
            exportText.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            exportText.AppendLine($"Total Orders: {_filteredOrders.Count}");
            exportText.AppendLine();

            // Add CSV header
            if (fileName.EndsWith(".csv"))
            {
                exportText.AppendLine("Order ID,Table Number,Customer Name,Employee Name,Order Time,Status,Total Amount");

                foreach (var order in _filteredOrders)
                {
                    exportText.AppendLine($"{order.Id},{order.TableNumber},\"{order.CustomerName}\",\"{order.EmployeeName}\",{order.OrderTime:yyyy-MM-dd HH:mm:ss},{order.Status},{order.TotalAmount:F2}");
                }
            }
            else
            {
                // Text format
                exportText.AppendLine("ORDER DETAILS:");
                exportText.AppendLine("".PadRight(80, '='));

                foreach (var order in _filteredOrders)
                {
                    exportText.AppendLine($"Order ID: {order.Id}");
                    exportText.AppendLine($"Table: {order.TableNumber}");
                    exportText.AppendLine($"Customer: {order.CustomerName}");
                    exportText.AppendLine($"Employee: {order.EmployeeName}");
                    exportText.AppendLine($"Order Time: {order.OrderTime:yyyy-MM-dd HH:mm:ss}");
                    exportText.AppendLine($"Status: {order.Status}");
                    exportText.AppendLine($"Total Amount: ${order.TotalAmount:F2}");
                    exportText.AppendLine("".PadRight(40, '-'));
                }

                var totalAmount = _filteredOrders.Sum(o => o.TotalAmount);
                exportText.AppendLine();
                exportText.AppendLine($"SUMMARY:");
                exportText.AppendLine($"Total Orders: {_filteredOrders.Count}");
                exportText.AppendLine($"Total Revenue: ${totalAmount:F2}");
                exportText.AppendLine($"Average Order Value: ${(_filteredOrders.Count > 0 ? totalAmount / _filteredOrders.Count : 0):F2}");
            }

            System.IO.File.WriteAllText(fileName, exportText.ToString());
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }

    // View Models for data binding
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public string EmployeeName { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public DateTime OrderTime { get; set; }
        public string Status { get; set; } = "";
        public decimal TotalAmount { get; set; }
    }

    public class OrderItemViewModel
    {
        public string MenuItemName { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
