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

namespace SushiRestaurant
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        // Event handler for Stock List button
        private void btnStockList_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Stock List window or functionality
            StockList.StockList stockWindow = new StockList.StockList();
            stockWindow.Show();
            this.Close();
        }

        // Event handler for Menu button
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Menu Management window or functionality
            Menu.MenuManagementWindow menuSelectionWindow = new Menu.MenuManagementWindow();
            menuSelectionWindow.Show();
            this.Close();
        }

        // Event handler for Reports button
        private void btnReports_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Reports window
            Reports.ReportsWindow reportsWindow = new Reports.ReportsWindow();
            reportsWindow.Show();
            this.Close();
        }

        // Event handler for Employee Account button
        private void btnEmployeeAccount_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Employee Account management window
            EmployeeAccountWindow employeeWindow = new EmployeeAccountWindow();
            employeeWindow.Show();
            this.Close();
        }

        // Event handler for Order List button
        private void btnOrderList_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Order List window
            Orders.OrderListWindow orderWindow = new Orders.OrderListWindow();
            orderWindow.Show();
            this.Close();
        }





        // Event handler for Logout button
        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Implement logout functionality
            MessageBoxResult result = MessageBox.Show("Are you sure you want to logout?", "Logout Confirmation",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Navigate back to login window
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}