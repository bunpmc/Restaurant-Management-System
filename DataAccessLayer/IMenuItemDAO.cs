using BusinessObjects;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public interface IMenuItemDAO
    {
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
    }
}
