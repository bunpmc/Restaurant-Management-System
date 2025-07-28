using BusinessObjects;
using System.Collections.Generic;

namespace Services
{
    public interface IMenuService
    {
        // Menu Category operations
        List<MenuCategory> GetAllMenuCategories();
        MenuCategory GetMenuCategoryById(int id);
        bool SaveMenuCategory(MenuCategory category);
        bool UpdateMenuCategory(MenuCategory category);
        bool DeleteMenuCategory(int id);
        List<MenuCategory> SearchMenuCategories(string searchTerm);
        bool IsCategoryInUse(int categoryId);

        // Menu Item operations
        List<MenuItem> GetAllMenuItems();
        List<MenuItem> GetMenuItemsByCategory(int categoryId);
        MenuItem GetMenuItemById(int id);
        bool SaveMenuItem(MenuItem menuItem);
        bool UpdateMenuItem(MenuItem menuItem);
        bool DeleteMenuItem(int id);
        List<MenuItem> SearchMenuItems(string searchTerm);
        List<MenuItem> GetAvailableMenuItems();
        bool UpdateMenuItemAvailability(int id, bool isAvailable);
        List<MenuItem> GetMenuItemsByPriceRange(decimal minPrice, decimal maxPrice);

        // Legacy methods for backward compatibility
        List<MenuItem> GetMenuItems();
        List<MenuCategory> GetCategories();
        List<MenuItem> GetMenuItemsByCategory(int? categoryId);
    }
}
