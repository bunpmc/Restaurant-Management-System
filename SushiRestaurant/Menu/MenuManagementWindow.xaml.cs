using BusinessObjects;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MenuItem = BusinessObjects.MenuItem;

namespace SushiRestaurant.Menu
{
    public partial class MenuManagementWindow : Window
    {
        private readonly IMenuService _menuService;
        private List<MenuCategory> _allCategories;
        private List<MenuItem> _allMenuItems;
        private MenuCategory _selectedCategory;

        public MenuManagementWindow()
        {
            InitializeComponent();
            _menuService = new MenuService();

            InitializeEventHandlers();
            LoadData();
        }



        private void InitializeEventHandlers()
        {
            // Category buttons
            btnAddMenu.Click += btnAddCategory_Click;
            btnEditMenu.Click += btnEditCategory_Click;
            btnDeleteMenu.Click += btnDeleteCategory_Click;

            // Menu item buttons
            btnAddItem.Click += btnAddItem_Click;
            btnEditItem.Click += btnEditItem_Click;
            btnDeleteItem.Click += btnDeleteItem_Click;

            // Selection events
            MenuList.SelectionChanged += MenuList_SelectionChanged;

            // Add refresh functionality (if refresh button exists)
            // You can add a refresh button to the XAML if needed
        }

        private void LoadData()
        {
            try
            {
                LoadCategories();
                // LoadMenuItems() will be called automatically by MenuList_SelectionChanged
                // when a category is selected in LoadCategories()
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                _allCategories = _menuService.GetAllMenuCategories();
                MenuList.ItemsSource = _allCategories;

                if (_allCategories.Any())
                {
                    MenuList.SelectedIndex = 0;
                }
                else
                {
                    // If no categories, load all menu items
                    LoadMenuItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMenuItems()
        {
            try
            {
                if (_selectedCategory != null)
                {
                    _allMenuItems = _menuService.GetMenuItemsByCategory(_selectedCategory.Id);
                    System.Diagnostics.Debug.WriteLine($"Loading items for category: {_selectedCategory.Name}, Found: {_allMenuItems.Count} items");
                }
                else
                {
                    _allMenuItems = _menuService.GetAllMenuItems();
                    System.Diagnostics.Debug.WriteLine($"Loading all items, Found: {_allMenuItems.Count} items");
                }

                MenuItemGrid.ItemsSource = null; // Clear first
                MenuItemGrid.ItemsSource = _allMenuItems;

                // Debug: Print first few items
                foreach (var item in _allMenuItems.Take(3))
                {
                    System.Diagnostics.Debug.WriteLine($"Item: {item.Name}, Price: {item.Price}, Category: {item.Category?.Name}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading menu items: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                System.Diagnostics.Debug.WriteLine($"Error in LoadMenuItems: {ex}");
            }
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCategory = MenuList.SelectedItem as MenuCategory;
            LoadMenuItems();

            // Update UI to show selected category info
            if (_selectedCategory != null)
            {
                // You can add category info display here if needed
            }
        }

        // Category CRUD Operations
        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new CategoryDialog();
                if (dialog.ShowDialog() == true)
                {
                    MessageBox.Show("Category added successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCategories();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding category: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedCategory == null)
                {
                    MessageBox.Show("Please select a category to edit.", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var dialog = new CategoryDialog(_selectedCategory);
                if (dialog.ShowDialog() == true)
                {
                    MessageBox.Show("Category updated successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCategories();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing category: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_selectedCategory == null)
                {
                    MessageBox.Show("Please select a category to delete.", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Check if category has items
                if (_menuService.IsCategoryInUse(_selectedCategory.Id))
                {
                    MessageBox.Show("Cannot delete category that contains menu items. Please remove all menu items first.",
                        "Cannot Delete", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Are you sure you want to delete the category '{_selectedCategory.Name}'?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (_menuService.DeleteMenuCategory(_selectedCategory.Id))
                    {
                        MessageBox.Show("Category deleted successfully!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadCategories();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete category.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting category: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Menu Item CRUD Operations
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialog = new MenuItemDialog();
                if (dialog.ShowDialog() == true)
                {
                    MessageBox.Show("Menu item added successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadMenuItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding menu item: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = MenuItemGrid.SelectedItem as MenuItem;
                if (selectedItem == null)
                {
                    MessageBox.Show("Please select a menu item to edit.", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var dialog = new MenuItemDialog(selectedItem);
                if (dialog.ShowDialog() == true)
                {
                    MessageBox.Show("Menu item updated successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadMenuItems();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing menu item: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = MenuItemGrid.SelectedItem as MenuItem;
                if (selectedItem == null)
                {
                    MessageBox.Show("Please select a menu item to delete.", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show($"Are you sure you want to delete '{selectedItem.Name}'?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (_menuService.DeleteMenuItem(selectedItem.Id))
                    {
                        MessageBox.Show("Menu item deleted successfully!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadMenuItems();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete menu item.", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting menu item: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        // Method to force refresh all data
        public void RefreshAllData()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== FORCE REFRESH DATA ===");
                LoadData();
                MessageBox.Show("Data refreshed successfully!", "Refresh",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}", "Refresh Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
