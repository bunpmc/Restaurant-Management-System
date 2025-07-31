using BusinessObjects;
using Services;
using System;
using System.Windows;

namespace SushiRestaurant.Menu
{
    public partial class CategoryDialog : Window
    {
        private readonly IMenuService _menuService;
        private MenuCategory _category;
        private bool _isEditMode;

        public MenuCategory Category { get; private set; }

        public CategoryDialog(MenuCategory category = null)
        {
            InitializeComponent();
            _menuService = new MenuService();
            _category = category;
            _isEditMode = category != null;

            InitializeDialog();
        }

        private void InitializeDialog()
        {
            if (_isEditMode)
            {
                txtTitle.Text = "Edit Category";
                LoadCategoryData();
            }
            else
            {
                txtTitle.Text = "Add New Category";
                txtStatus.Text = "Status: New Category";
                txtItemCount.Text = "Items in this category: 0";
                txtCreatedDate.Text = "Created: N/A";
            }
        }

        private void LoadCategoryData()
        {
            if (_category != null)
            {
                txtCategoryName.Text = _category.Name;
                txtDescription.Text = _category.Description ?? "";
                
                // Load additional info
                var itemCount = _category.MenuItems?.Count ?? 0;
                txtItemCount.Text = $"Items in this category: {itemCount}";
                txtStatus.Text = itemCount > 0 ? "Status: Active (Contains Items)" : "Status: Empty Category";
                txtCreatedDate.Text = "Created: N/A"; // Add creation date if available
            }
        }

        private bool ValidateInput()
        {
            borderValidation.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                ShowValidationError("Category name is required.");
                return false;
            }

            if (txtCategoryName.Text.Trim().Length < 2)
            {
                ShowValidationError("Category name must be at least 2 characters long.");
                return false;
            }

            if (txtCategoryName.Text.Trim().Length > 50)
            {
                ShowValidationError("Category name cannot exceed 50 characters.");
                return false;
            }

            if (!string.IsNullOrEmpty(txtDescription.Text) && txtDescription.Text.Length > 200)
            {
                ShowValidationError("Description cannot exceed 200 characters.");
                return false;
            }

            return true;
        }

        private void ShowValidationError(string message)
        {
            txtValidationMessage.Text = message;
            borderValidation.Visibility = Visibility.Visible;
        }

        private MenuCategory CreateCategoryFromForm()
        {
            var category = _isEditMode ? _category : new MenuCategory();
            
            category.Name = txtCategoryName.Text.Trim();
            category.Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? 
                null : txtDescription.Text.Trim();

            return category;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                var category = CreateCategoryFromForm();
                bool success;

                if (_isEditMode)
                {
                    success = _menuService.UpdateMenuCategory(category);
                }
                else
                {
                    success = _menuService.SaveMenuCategory(category);
                }

                if (success)
                {
                    Category = category;
                    DialogResult = true;
                    Close();
                }
                else
                {
                    ShowValidationError("Failed to save category. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ShowValidationError($"Error saving category: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
