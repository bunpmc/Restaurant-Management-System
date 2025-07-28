using BusinessObjects;
using Repositories;
using System;
using System.Collections.Generic;

namespace Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuCategoryRepository _categoryRepository;
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuService()
        {
            _categoryRepository = new MenuCategoryRepository();
            _menuItemRepository = new MenuItemRepository();
        }

        // Menu Category operations
        public List<MenuCategory> GetAllMenuCategories()
        {
            try
            {
                return _categoryRepository.GetAllMenuCategories();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menu categories: {ex.Message}");
            }
        }

        public MenuCategory GetMenuCategoryById(int id)
        {
            try
            {
                return _categoryRepository.GetMenuCategoryById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menu category: {ex.Message}");
            }
        }

        public bool SaveMenuCategory(MenuCategory category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    throw new ArgumentException("Category name is required.");
                }

                return _categoryRepository.SaveMenuCategory(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving menu category: {ex.Message}");
            }
        }

        public bool UpdateMenuCategory(MenuCategory category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    throw new ArgumentException("Category name is required.");
                }

                return _categoryRepository.UpdateMenuCategory(category);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating menu category: {ex.Message}");
            }
        }

        public bool DeleteMenuCategory(int id)
        {
            try
            {
                // Check if category is in use
                if (_categoryRepository.IsCategoryInUse(id))
                {
                    throw new InvalidOperationException("Cannot delete category that contains menu items. Please remove all menu items first.");
                }

                return _categoryRepository.DeleteMenuCategory(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting menu category: {ex.Message}");
            }
        }

        public List<MenuCategory> SearchMenuCategories(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return GetAllMenuCategories();
                }

                return _categoryRepository.SearchMenuCategories(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching menu categories: {ex.Message}");
            }
        }

        public bool IsCategoryInUse(int categoryId)
        {
            try
            {
                return _categoryRepository.IsCategoryInUse(categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking category usage: {ex.Message}");
            }
        }

        // Menu Item operations
        public List<MenuItem> GetAllMenuItems()
        {
            try
            {
                return _menuItemRepository.GetAllMenuItems();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menu items: {ex.Message}");
            }
        }

        public List<MenuItem> GetMenuItemsByCategory(int categoryId)
        {
            try
            {
                return _menuItemRepository.GetMenuItemsByCategory(categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menu items by category: {ex.Message}");
            }
        }

        public MenuItem GetMenuItemById(int id)
        {
            try
            {
                return _menuItemRepository.GetMenuItemById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menu item: {ex.Message}");
            }
        }

        public bool SaveMenuItem(MenuItem menuItem)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(menuItem.Name))
                {
                    throw new ArgumentException("Menu item name is required.");
                }

                if (menuItem.Price <= 0)
                {
                    throw new ArgumentException("Menu item price must be greater than zero.");
                }

                return _menuItemRepository.SaveMenuItem(menuItem);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving menu item: {ex.Message}");
            }
        }

        public bool UpdateMenuItem(MenuItem menuItem)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(menuItem.Name))
                {
                    throw new ArgumentException("Menu item name is required.");
                }

                if (menuItem.Price <= 0)
                {
                    throw new ArgumentException("Menu item price must be greater than zero.");
                }

                return _menuItemRepository.UpdateMenuItem(menuItem);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating menu item: {ex.Message}");
            }
        }

        public bool DeleteMenuItem(int id)
        {
            try
            {
                return _menuItemRepository.DeleteMenuItem(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting menu item: {ex.Message}");
            }
        }

        public List<MenuItem> SearchMenuItems(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return GetAllMenuItems();
                }

                return _menuItemRepository.SearchMenuItems(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching menu items: {ex.Message}");
            }
        }

        public List<MenuItem> GetAvailableMenuItems()
        {
            try
            {
                return _menuItemRepository.GetAvailableMenuItems();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving available menu items: {ex.Message}");
            }
        }

        public bool UpdateMenuItemAvailability(int id, bool isAvailable)
        {
            try
            {
                return _menuItemRepository.UpdateMenuItemAvailability(id, isAvailable);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating menu item availability: {ex.Message}");
            }
        }

        public List<MenuItem> GetMenuItemsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            try
            {
                if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
                {
                    throw new ArgumentException("Invalid price range.");
                }

                return _menuItemRepository.GetMenuItemsByPriceRange(minPrice, maxPrice);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menu items by price range: {ex.Message}");
            }
        }

        // Legacy methods for backward compatibility
        public List<MenuItem> GetMenuItems()
        {
            return GetAvailableMenuItems();
        }

        public List<MenuCategory> GetCategories()
        {
            return GetAllMenuCategories();
        }

        public List<MenuItem> GetMenuItemsByCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                return GetAllMenuItems();
            }
            return GetMenuItemsByCategory(categoryId.Value);
        }
    }
}
