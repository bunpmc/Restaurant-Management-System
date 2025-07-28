using BusinessObjects;
using Services;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace SushiRestaurant.Menu
{
    public partial class MenuItemDialog : Window
    {
        private readonly IMenuService _menuService;
        private MenuItem _menuItem;
        private bool _isEditMode;

        public MenuItem MenuItem { get; private set; }

        public MenuItemDialog(MenuItem menuItem = null)
        {
            InitializeComponent();
            _menuService = new MenuService();
            _menuItem = menuItem;
            _isEditMode = menuItem != null;

            InitializeDialog();
        }

        private void InitializeDialog()
        {
            LoadCategories();

            if (_isEditMode)
            {
                txtTitle.Text = "Edit Menu Item";
                LoadMenuItemData();
            }
            else
            {
                txtTitle.Text = "Add New Menu Item";
                txtStatus.Text = "Status: New Item";
                txtOrderCount.Text = "Total orders: 0";
                txtCreatedDate.Text = "Created: N/A";
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _menuService.GetAllMenuCategories();
                cmbCategory.ItemsSource = categories;
                
                if (categories.Any())
                {
                    cmbCategory.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ShowValidationError($"Error loading categories: {ex.Message}");
            }
        }

        private void LoadMenuItemData()
        {
            if (_menuItem != null)
            {
                txtItemName.Text = _menuItem.Name;
                txtPrice.Text = _menuItem.Price.ToString("F2");
                txtDescription.Text = _menuItem.Description ?? "";
                chkIsAvailable.IsChecked = _menuItem.IsAvailable ?? true;

                // Set category
                if (_menuItem.CategoryId.HasValue)
                {
                    cmbCategory.SelectedValue = _menuItem.CategoryId.Value;
                }

                // Load additional info
                txtStatus.Text = _menuItem.IsAvailable == true ? "Status: Available" : "Status: Unavailable";
                txtOrderCount.Text = "Total orders: N/A"; // Add order count if available
                txtCreatedDate.Text = _menuItem.CreatedAt?.ToString("MM/dd/yyyy") ?? "Created: N/A";
            }
        }

        private bool ValidateInput()
        {
            borderValidation.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(txtItemName.Text))
            {
                ShowValidationError("Item name is required.");
                return false;
            }

            if (txtItemName.Text.Trim().Length < 2)
            {
                ShowValidationError("Item name must be at least 2 characters long.");
                return false;
            }

            if (txtItemName.Text.Trim().Length > 100)
            {
                ShowValidationError("Item name cannot exceed 100 characters.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                ShowValidationError("Price is required.");
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal price) &&
                !decimal.TryParse(txtPrice.Text, out price))
            {
                ShowValidationError("Please enter a valid price.");
                return false;
            }

            if (price <= 0)
            {
                ShowValidationError("Price must be greater than zero.");
                return false;
            }

            if (price > 999.99m)
            {
                ShowValidationError("Price cannot exceed $999.99.");
                return false;
            }

            if (cmbCategory.SelectedValue == null)
            {
                ShowValidationError("Please select a category.");
                return false;
            }

            if (!string.IsNullOrEmpty(txtDescription.Text) && txtDescription.Text.Length > 500)
            {
                ShowValidationError("Description cannot exceed 500 characters.");
                return false;
            }

            return true;
        }

        private void ShowValidationError(string message)
        {
            txtValidationMessage.Text = message;
            borderValidation.Visibility = Visibility.Visible;
        }

        private MenuItem CreateMenuItemFromForm()
        {
            var menuItem = _isEditMode ? _menuItem : new MenuItem();
            
            menuItem.Name = txtItemName.Text.Trim();
            
            // Parse price
            if (decimal.TryParse(txtPrice.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal price) ||
                decimal.TryParse(txtPrice.Text, out price))
            {
                menuItem.Price = price;
            }

            menuItem.Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? 
                null : txtDescription.Text.Trim();
            
            menuItem.CategoryId = (int)cmbCategory.SelectedValue;
            menuItem.IsAvailable = chkIsAvailable.IsChecked ?? true;

            return menuItem;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                var menuItem = CreateMenuItemFromForm();
                bool success;

                if (_isEditMode)
                {
                    success = _menuService.UpdateMenuItem(menuItem);
                }
                else
                {
                    success = _menuService.SaveMenuItem(menuItem);
                }

                if (success)
                {
                    MenuItem = menuItem;
                    DialogResult = true;
                    Close();
                }
                else
                {
                    ShowValidationError("Failed to save menu item. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ShowValidationError($"Error saving menu item: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
