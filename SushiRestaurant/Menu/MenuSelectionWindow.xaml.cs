using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BusinessObjects;
using Services;

namespace SushiRestaurant.Menu
{
    /// <summary>
    /// Interaction logic for MenuSelectionWindow.xaml
    /// </summary>
    public partial class MenuSelectionWindow : Window, INotifyPropertyChanged
    {
        private readonly MenuService _menuService;
        private readonly TableService _tableService;
        private readonly OrderService _orderService; // New service for order management
        private DateTime _currentDateTime;
        private ObservableCollection<OrderItemViewModel> _orderItems;

        public event PropertyChangedEventHandler PropertyChanged;

        public DateTime CurrentDateTime
        {
            get => _currentDateTime;
            set
            {
                _currentDateTime = value;
                OnPropertyChanged(nameof(CurrentDateTime));
            }
        }

        public ObservableCollection<OrderItemViewModel> OrderItems
        {
            get => _orderItems;
            set
            {
                _orderItems = value;
                OnPropertyChanged(nameof(OrderItems));
            }
        }

        public MenuSelectionWindow()
        {
            InitializeComponent();
            _menuService = new MenuService();
            _tableService = new TableService();
            _orderService = new OrderService(); // Initialize order service
            DataContext = this;
            OrderItems = new ObservableCollection<OrderItemViewModel>();
            OrderGrid.ItemsSource = OrderItems;
            CurrentDateTime = DateTime.Now;
            LoadCategories();
            LoadTables();
            InitializeEventHandlers();
        }

        private void LoadCategories()
        {
            try
            {
                CategoryCombo.ItemsSource = _menuService.GetCategories();
                CategoryCombo.DisplayMemberPath = "Name"; // Adjust based on your MenuCategory properties
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTables()
        {
            try
            {
                var tables = _tableService.GetTables();
                var tableViewModels = new ObservableCollection<TableViewModel>();
                foreach (var table in tables)
                {
                    var viewModel = new TableViewModel(table);
                    // Use the Status property to determine the table state
                    if (table.Status?.ToLower() == "occupied")
                    {
                        viewModel.StatusColor = "#EF5350"; // Red for occupied
                        viewModel.StatusForeground = "#D32F2F";
                        viewModel.StatusText = "Occupied";
                    }
                    else
                    {
                        viewModel.StatusColor = "#81C784"; // Green for available
                        viewModel.StatusForeground = "#388E3C";
                        viewModel.StatusText = "Available";
                    }
                    tableViewModels.Add(viewModel);
                }
                TableItemsControl.ItemsSource = tableViewModels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tables: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryCombo.SelectedItem is MenuCategory selectedCategory)
            {
                LoadMenuItems(selectedCategory.Id);
            }
        }

        private void LoadMenuItems(int? categoryId)
        {
            try
            {
                MenuItemsControl.ItemsSource = _menuService.GetMenuItemsByCategory(categoryId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading menu items: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeEventHandlers()
        {
            // Event handlers are set in XAML via MouseDoubleClick and Button Click
            OrderGrid.MouseRightButtonDown += OrderGrid_MouseRightButtonDown; // Add removal option
        }

        private void MenuItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Button button && button.DataContext is BusinessObjects.MenuItem menuItem)
            {
                try
                {
                    if (menuItem.IsAvailable == true)
                    {
                        AddItemToOrder(menuItem);
                    }
                    else
                    {
                        MessageBox.Show("This item is not available.", "Warning",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding item: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
            }
        }

        private void AddItemToOrder(BusinessObjects.MenuItem menuItem)
        {
            var existingItem = OrderItems.FirstOrDefault(item => item.ID == menuItem.Id);
            if (existingItem != null)
            {
                existingItem.Qty++;
                existingItem.Total = existingItem.Price * existingItem.Qty;
                existingItem.Tax = existingItem.Total * 0.142m;
            }
            else
            {
                var newItem = new OrderItemViewModel
                {
                    ID = menuItem.Id,
                    Items = menuItem.Name,
                    Price = menuItem.Price,
                    Qty = 1,
                    Total = menuItem.Price,
                    Tax = menuItem.Price * 0.142m,
                    Options = menuItem.Description ?? string.Empty
                };
                OrderItems.Add(newItem);
            }
            UpdateSummary();
        }

        private void OrderGrid_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem is OrderItemViewModel selectedItem)
            {
                var result = MessageBox.Show($"Remove '{selectedItem.Items}' from the order?",
                    "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    OrderItems.Remove(selectedItem);
                    UpdateSummary();
                }
            }
        }

        private void UpdateSummary()
        {
            decimal total = OrderItems.Sum(item => item.Total);
            decimal tax = total * 0.142m; // 14.2% tax rate
            decimal subtotal = total;
            decimal payable = total + tax;

            txtTotal.Text = $"Total: ${total:F2}";
            txtDiscount.Text = "Discount: $0.00";
            txtSubtotal.Text = $"Subtotal: ${subtotal:F2}";
            txtTax.Text = $"Tax: ${tax:F2}";
            txtTotalPayable.Text = $"Total Payable: ${payable:F2}";
            txtItemTypes.Text = $"Item Types: {OrderItems.Count}";
        }

        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {
            decimal totalPayable = decimal.Parse(txtTotalPayable.Text.Replace("Total Payable: $", ""));
            var result = MessageBox.Show($"Confirm payment of ${totalPayable:F2}? This will complete the order.",
                "Confirm Payment", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Create Order and OrderItem entities
                    var order = new Order
                    {
                        OrderTime = DateTime.Now,
                        TotalAmount = totalPayable,
                        Status = "Completed",
                        EmployeeId = 1, // Placeholder; replace with logged-in employee ID
                        TableId = null, // Placeholder; set based on Table tab selection
                        CustomerId = null // Placeholder; set if customer is selected
                    };

                    var orderItems = new List<OrderItem>();
                    foreach (var item in OrderItems)
                    {
                        orderItems.Add(new OrderItem
                        {
                            MenuItemId = item.ID,
                            Quantity = item.Qty,
                            UnitPrice = item.Price,
                            TotalPrice = item.Total
                        });
                    }
                    order.OrderItems = orderItems;

                    // Save to database (placeholder)
                    _orderService.SaveOrder(order);

                    // Clear the order
                    OrderItems.Clear();
                    UpdateSummary();
                    MessageBox.Show("Payment processed and order saved successfully.", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing payment: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void btnHomeMenu_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class OrderItemViewModel
    {
        public int ID { get; set; }
        public string Items { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public string Options { get; set; }
    }

    public class TableViewModel
    {
        private readonly BusinessObjects.Table _table;

        public TableViewModel(BusinessObjects.Table table)
        {
            _table = table;
        }

        public int Id => _table.Id;
        public int TableNumber => _table.TableNumber;
        public int? Capacity => _table.Capacity;
        public string Status => _table.Status;
        public string Note => _table.Note;

        public string StatusColor { get; set; }
        public string StatusForeground { get; set; }
        public string StatusText { get; set; }
    }
}