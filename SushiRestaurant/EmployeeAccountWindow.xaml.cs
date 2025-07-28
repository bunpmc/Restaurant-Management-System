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
    /// <summary>
    /// Interaction logic for EmployeeAccountWindow.xaml
    /// </summary>
    public partial class EmployeeAccountWindow : Window
    {
        private readonly EmployeeService _employeeService;
        private DateTime _currentDateTime;
        private Employee _selectedEmployee;
        private bool _isEditing;
        private List<Employee> _allEmployees;
        private List<Employee> _filteredEmployees;

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

        public EmployeeAccountWindow()
        {
            InitializeComponent();
            _employeeService = new EmployeeService();
            DataContext = this;
            CurrentDateTime = DateTime.Now;
            LoadEmployees();
            ToggleFormState(false);

            // Add event handlers
            EmployeeAccountGrid.SelectionChanged += EmployeeAccountGrid_SelectionChanged;
            cmbRole.SelectionChanged += CmbRole_SelectionChanged;

            // Start timer for current time
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => CurrentDateTime = DateTime.Now;
            timer.Start();
        }

        private void LoadEmployees()
        {
            try
            {
                _allEmployees = _employeeService.GetAllEmployees();
                _filteredEmployees = new List<Employee>(_allEmployees);
                EmployeeAccountGrid.ItemsSource = _filteredEmployees;

                // Update status bar
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EmployeeAccountGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeAccountGrid.SelectedItem is Employee selectedEmployee)
            {
                _selectedEmployee = selectedEmployee;
                _isEditing = true;
                PopulateForm();
                ToggleFormState(true);

                // Update button states based on employee status
                btnActivate.IsEnabled = selectedEmployee.IsActive != true;
                btnDeactivate.IsEnabled = selectedEmployee.IsActive == true;

                // Reset Password button only for Cashier
                btnResetPassword.IsEnabled = selectedEmployee.Role == "Cashier";
            }
        }

        //private void btnAddEdit_Click(object sender, RoutedEventArgs e)
        //{
        //    _isEditing = EmployeeAccountGrid.SelectedItem != null;
        //    _selectedEmployee = _isEditing ? EmployeeAccountGrid.SelectedItem as Employee : new Employee { IsActive = true, CreatedAt = DateTime.Now };
        //    ToggleFormState(true);
        //    PopulateForm();
        //}

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    // Create new employee if not editing
                    if (!_isEditing)
                    {
                        _selectedEmployee = new Employee
                        {
                            CreatedAt = DateTime.Now,
                            IsActive = true
                        };
                    }

                    // Update employee properties
                    _selectedEmployee.Username = txtUsername.Text.Trim();

                    // Only set password for Cashier role, others get default
                    var selectedRole = (cmbRole.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
                    if (selectedRole == "Cashier")
                    {
                        var password = string.IsNullOrWhiteSpace(txtPasswordHash.Text) ? "123456" : txtPasswordHash.Text;
                        _selectedEmployee.PasswordHash = HashPassword(password);
                    }
                    else
                    {
                        _selectedEmployee.PasswordHash = HashPassword("123456"); // Default password for non-cashier
                    }

                    _selectedEmployee.FullName = txtFullName.Text.Trim();
                    _selectedEmployee.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
                    _selectedEmployee.Role = selectedRole;
                    _selectedEmployee.IsActive = chkIsActive.IsChecked ?? false;

                    bool success;
                    string action;

                    if (_isEditing)
                    {
                        success = _employeeService.UpdateEmployee(_selectedEmployee);
                        action = "updated";
                    }
                    else
                    {
                        success = _employeeService.AddEmployee(_selectedEmployee);
                        action = "added";
                    }

                    if (success)
                    {
                        MessageBox.Show($"Employee {action} successfully!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadEmployees();
                        ToggleFormState(false);
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show($"Failed to {action.Replace("ed", "")} employee. Please try again.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving employee: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ToggleFormState(false);
            ClearForm();
        }

        private void btnDeactivate_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeAccountGrid.SelectedItem is Employee selectedEmployee)
            {
                var result = MessageBox.Show($"Are you sure you want to deactivate the employee '{selectedEmployee.FullName}'?",
                    "Confirm Deactivation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _employeeService.DeactivateEmployee(selectedEmployee.Id);
                        LoadEmployees();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deactivating employee: {ex.Message}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an employee to deactivate.", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeAccountGrid.SelectedItem is Employee selectedEmployee)
            {
                if (selectedEmployee.Role != "Cashier")
                {
                    MessageBox.Show("Password reset is only available for Cashier accounts.", "Information",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show($"Are you sure you want to reset the password for Cashier '{selectedEmployee.FullName}' to default (123456)?",
                    "Confirm Password Reset", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var defaultPassword = HashPassword("123456");
                        _employeeService.ResetPassword(selectedEmployee.Id, defaultPassword);
                        MessageBox.Show("Cashier password has been reset to default: 123456",
                            "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error resetting password: {ex.Message}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a Cashier employee to reset password.", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ToggleFormState(bool isActive)
        {
            // Form controls
            txtUsername.IsEnabled = isActive;
            txtPasswordHash.IsEnabled = isActive;
            txtFullName.IsEnabled = isActive;
            txtEmail.IsEnabled = isActive;
            cmbRole.IsEnabled = isActive;
            chkIsActive.IsEnabled = isActive;

            // Form buttons
            btnSave.IsEnabled = isActive;
            btnCancel.IsEnabled = isActive;

            // Grid and action buttons
            EmployeeAccountGrid.IsEnabled = !isActive;
            btnAddNew.IsEnabled = !isActive;
            btnActivate.IsEnabled = !isActive;
            btnDeactivate.IsEnabled = !isActive;
            btnResetPassword.IsEnabled = !isActive;

            // Update form title based on mode
            if (isActive)
            {
                // Update form title or add visual indicator for edit mode
                if (_isEditing)
                {
                    // Editing existing employee
                    txtUsername.IsEnabled = false; // Don't allow username changes
                }
                else
                {
                    // Adding new employee
                    txtUsername.IsEnabled = true;
                }
            }
        }

        private void PopulateForm()
        {
            if (_selectedEmployee != null)
            {
                txtUsername.Text = _selectedEmployee.Username;
                txtPasswordHash.Text = ""; // Don't show existing password
                txtFullName.Text = _selectedEmployee.FullName;
                txtEmail.Text = _selectedEmployee.Email ?? string.Empty;

                // Set role in ComboBox
                foreach (ComboBoxItem item in cmbRole.Items)
                {
                    if (item.Content.ToString() == _selectedEmployee.Role)
                    {
                        cmbRole.SelectedItem = item;
                        break;
                    }
                }

                chkIsActive.IsChecked = _selectedEmployee.IsActive;

                // Update password field visibility based on role
                UpdatePasswordFieldVisibility();
            }
        }

        private void ClearForm()
        {
            txtUsername.Text = string.Empty;
            txtPasswordHash.Text = string.Empty;
            txtFullName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            cmbRole.SelectedIndex = -1;
            chkIsActive.IsChecked = true;
            _selectedEmployee = null;
            _isEditing = false;
            EmployeeAccountGrid.SelectedItem = null;
        }

        private bool ValidateInput()
        {
            var errors = new List<string>();

            // Required field validation
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
                errors.Add("Username is required");

            // Password only required for Cashier role
            var selectedRole = (cmbRole.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
            if (selectedRole == "Cashier" && string.IsNullOrWhiteSpace(txtPasswordHash.Text))
                errors.Add("Password is required for Cashier role");

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
                errors.Add("Full Name is required");

            if (cmbRole.SelectedItem == null)
                errors.Add("Role is required");

            // Username validation
            if (!string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                if (txtUsername.Text.Length < 3)
                    errors.Add("Username must be at least 3 characters long");

                if (txtUsername.Text.Contains(" "))
                    errors.Add("Username cannot contain spaces");
            }

            // Password validation (only for Cashier if provided)
            if (selectedRole == "Cashier" && !string.IsNullOrWhiteSpace(txtPasswordHash.Text))
            {
                if (txtPasswordHash.Text.Length < 6)
                    errors.Add("Password must be at least 6 characters long");
            }

            // Email validation
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                if (!IsValidEmail(txtEmail.Text))
                    errors.Add("Please enter a valid email address");
            }

            // Role validation is handled by ComboBox selection

            if (errors.Any())
            {
                MessageBox.Show($"Validation Errors:\n\n{string.Join("\n", errors)}", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string HashPassword(string password)
        {
            // Simple hash for demo - in production, use proper password hashing like BCrypt
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + "SakanaHouseSalt"));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void btnActivate_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeAccountGrid.SelectedItem is Employee selectedEmployee)
            {
                var result = MessageBox.Show($"Are you sure you want to reactivate the employee '{selectedEmployee.FullName}'?",
                    "Confirm Reactivate", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _employeeService.ActivateEmployee(selectedEmployee.Id);
                        LoadEmployees();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error reactivate employee: {ex.Message}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
            else
            {
                MessageBox.Show("Please select an employee to reactivate.", "Warning",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            _isEditing = false;
            _selectedEmployee = null;
            ClearForm();
            ToggleFormState(true);
            txtUsername.Focus();

            // Set default role and update password field visibility
            cmbRole.SelectedIndex = 2; // Default to Cashier
            UpdatePasswordFieldVisibility();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterEmployees();
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = string.Empty;
            FilterEmployees();
        }

        private void FilterEmployees()
        {
            if (_allEmployees == null) return;

            var searchText = txtSearch.Text?.ToLower() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredEmployees = new List<Employee>(_allEmployees);
            }
            else
            {
                _filteredEmployees = _allEmployees.Where(emp =>
                    emp.FullName.ToLower().Contains(searchText) ||
                    emp.Username.ToLower().Contains(searchText) ||
                    emp.Role.ToLower().Contains(searchText) ||
                    (emp.Email?.ToLower().Contains(searchText) ?? false)
                ).ToList();
            }

            EmployeeAccountGrid.ItemsSource = _filteredEmployees;
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            if (_allEmployees == null)
            {
                txtEmployeeCount.Text = "Total: 0 employees";
                txtStatus.Text = "Ready";
                return;
            }

            var totalCount = _allEmployees.Count;
            var activeCount = _allEmployees.Count(e => e.IsActive == true);
            var inactiveCount = totalCount - activeCount;
            var displayedCount = _filteredEmployees?.Count ?? 0;

            txtEmployeeCount.Text = $"Total: {totalCount} employees ({activeCount} active, {inactiveCount} inactive)";

            if (_filteredEmployees != null && _filteredEmployees.Count != totalCount)
            {
                txtStatus.Text = $"Showing {displayedCount} of {totalCount} employees";
            }
            else
            {
                txtStatus.Text = "Ready";
            }
        }

        private void CmbRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdatePasswordFieldVisibility();
        }

        private void UpdatePasswordFieldVisibility()
        {
            var selectedRole = (cmbRole.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
            var isCashier = selectedRole == "Cashier";

            // Show/hide password field based on role
            lblPassword.Visibility = isCashier ? Visibility.Visible : Visibility.Collapsed;
            txtPasswordHash.Visibility = isCashier ? Visibility.Visible : Visibility.Collapsed;

            // Update tooltip and placeholder
            if (isCashier)
            {
                txtPasswordHash.ToolTip = "Enter password for Cashier login (minimum 6 characters)";
            }
            else
            {
                txtPasswordHash.ToolTip = "Password not required for this role (default: 123456)";
                txtPasswordHash.Text = ""; // Clear password field for non-cashier roles
            }
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
