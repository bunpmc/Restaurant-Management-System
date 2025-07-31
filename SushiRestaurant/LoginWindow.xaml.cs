using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessObjects;
using Services;

namespace SushiRestaurant
{
    public partial class LoginWindow : Window
    {
        private readonly EmployeeService _employeeService;

        public LoginWindow()
        {
            InitializeComponent();
            _employeeService = new EmployeeService(); // Initialize the service
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                StatusMessage.Text = "Please enter both username and password.";
                return;
            }

            try
            {
                bool isAuthenticated = _employeeService.Login(username, password) != null;

                if (isAuthenticated)
                {
                    Employee employee = _employeeService.Login(username, password);
                    StatusMessage.Text = "Login successful!";
                    StatusMessage.Foreground = System.Windows.Media.Brushes.Green;
                    // Proceed to the main application window or next step
                    // Example: Open main window
                    if (employee.Role == "Admin")
                    {
                        var mainWindow = new MainWindow();
                        mainWindow.Show();

                    } else if (employee.Role == "Cashier")
                    {
                        Menu.MenuSelectionWindow menuSelectionWindow = new Menu.MenuSelectionWindow();
                        menuSelectionWindow.Show();
                    } else if (employee.Role == "Waiter")
                    {
                        Orders.OrderListWindow orderListWindow = new Orders.OrderListWindow();
                        orderListWindow.Show();
                    }


                    this.Close();
                }
                else
                {
                    StatusMessage.Text = "Invalid username or password.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage.Text = $"Error: {ex.Message}";
            }
        }
    }
}