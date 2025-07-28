using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using BusinessObjects;
using Services;

namespace SushiRestaurant.Orders
{
    public partial class OrderViewWindow : Window
    {
        private readonly IOrderService _orderService;
        private readonly IEmployeeService _employeeService;
        private readonly ITableService _tableService;
        private readonly ICustomerService _customerService;
        private readonly IMenuService _menuService;

        private DispatcherTimer _refreshTimer;
        private List<OrderDisplayViewModel> _allOrders;
        private OrderViewWindowViewModel _viewModel;

        public OrderViewWindow()
        {
            InitializeComponent();

            _orderService = new OrderService();
            _employeeService = new EmployeeService();
            _tableService = new TableService();
            _customerService = new CustomerService();
            _menuService = new MenuService();

            _viewModel = new OrderViewWindowViewModel();
            DataContext = _viewModel;

            InitializeRefreshTimer();
            LoadOrders();
        }

        private void InitializeRefreshTimer()
        {
            // Refresh orders every 30 seconds for real-time updates
            _refreshTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(30)
            };
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                // Get orders from today that are pending or in progress
                _allOrders = GetActiveOrdersFromDatabase();

                // Distribute orders across three columns
                DistributeOrdersToColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<OrderDisplayViewModel> GetActiveOrdersFromDatabase()
        {
            try
            {
                var orders = _orderService.GetAllOrders();
                var orderDisplayModels = new List<OrderDisplayViewModel>();

                // Filter for today's orders that are not completed or cancelled
                var activeOrders = orders.Where(o =>
                    o.OrderTime?.Date == DateTime.Today &&
                    (o.Status == "Pending" || o.Status == "In Progress" || o.Status == "Ready"))
                    .OrderBy(o => o.OrderTime)
                    .ToList();

                foreach (var order in activeOrders)
                {
                    var orderDisplay = new OrderDisplayViewModel
                    {
                        Id = order.Id,
                        TableNumber = order.Table?.TableNumber ?? 0,
                        TimeStamp = order.OrderTime ?? DateTime.Now,
                        Status = order.Status ?? "Pending",
                        Items = new ObservableCollection<OrderItemDisplayViewModel>()
                    };

                    // Load order items
                    if (order.OrderItems != null)
                    {
                        foreach (var item in order.OrderItems)
                        {
                            var itemDisplay = new OrderItemDisplayViewModel
                            {
                                Name = $"{item.Quantity}x {item.MenuItem?.Name ?? "Unknown Item"}",
                                IsActive = order.Status == "In Progress" || order.Status == "Ready",
                                Quantity = item.Quantity,
                                MenuItemName = item.MenuItem?.Name ?? "Unknown Item"
                            };

                            orderDisplay.Items.Add(itemDisplay);
                        }
                    }

                    orderDisplayModels.Add(orderDisplay);
                }

                return orderDisplayModels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders from database: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<OrderDisplayViewModel>();
            }
        }

        private void DistributeOrdersToColumns()
        {
            // Clear existing columns
            _viewModel.OrdersColumn1.Clear();
            _viewModel.OrdersColumn2.Clear();
            _viewModel.OrdersColumn3.Clear();

            if (_allOrders == null || !_allOrders.Any())
                return;

            // Distribute orders evenly across three columns
            for (int i = 0; i < _allOrders.Count; i++)
            {
                var order = _allOrders[i];

                switch (i % 3)
                {
                    case 0:
                        _viewModel.OrdersColumn1.Add(order);
                        break;
                    case 1:
                        _viewModel.OrdersColumn2.Add(order);
                        break;
                    case 2:
                        _viewModel.OrdersColumn3.Add(order);
                        break;
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            // Stop the refresh timer when window closes
            _refreshTimer?.Stop();
            base.OnClosed(e);
        }

        // Optional: Add methods for manual refresh or status updates
        public void RefreshOrders()
        {
            LoadOrders();
        }

        public void MarkOrderItemComplete(int orderId, string itemName)
        {
            try
            {
                // This could be used to mark individual items as complete
                // Implementation would depend on your business logic
                var order = _allOrders.FirstOrDefault(o => o.Id == orderId);
                if (order != null)
                {
                    var item = order.Items.FirstOrDefault(i => i.MenuItemName == itemName);
                    if (item != null)
                    {
                        item.IsActive = false;
                        // Update in database if needed
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating item status: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            try
            {
                var order = _orderService.GetOrderById(orderId);
                if (order != null)
                {
                    order.Status = newStatus;
                    bool success = _orderService.UpdateOrder(order);

                    if (success)
                    {
                        // Refresh the display
                        LoadOrders();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating order status: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // ViewModel for the window
    public class OrderViewWindowViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<OrderDisplayViewModel> OrdersColumn1 { get; set; }
        public ObservableCollection<OrderDisplayViewModel> OrdersColumn2 { get; set; }
        public ObservableCollection<OrderDisplayViewModel> OrdersColumn3 { get; set; }

        public OrderViewWindowViewModel()
        {
            OrdersColumn1 = new ObservableCollection<OrderDisplayViewModel>();
            OrdersColumn2 = new ObservableCollection<OrderDisplayViewModel>();
            OrdersColumn3 = new ObservableCollection<OrderDisplayViewModel>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // View Models for the kitchen display
    public class OrderDisplayViewModel : INotifyPropertyChanged
    {
        private string _displayText;
        private string _status;

        public int Id { get; set; }
        public int TableNumber { get; set; }
        public DateTime TimeStamp { get; set; }

        public string DisplayText
        {
            get => _displayText ?? $"Table {TableNumber}";
            set
            {
                _displayText = value;
                OnPropertyChanged(nameof(DisplayText));
            }
        }

        public string Status
        {
            get => _status ?? "Pending";
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public ObservableCollection<OrderItemDisplayViewModel> Items { get; set; } = new ObservableCollection<OrderItemDisplayViewModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class OrderItemDisplayViewModel : INotifyPropertyChanged
    {
        private string _name;
        private bool _isActive;

        public string Name
        {
            get => _name ?? "";
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        public int Quantity { get; set; }
        public string MenuItemName { get; set; } = "";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}