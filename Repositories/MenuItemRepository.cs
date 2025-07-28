using BusinessObjects;
using DataAccessLayer;
using System.Collections.Generic;

namespace Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly IMenuItemDAO _menuItemDAO;

        public MenuItemRepository()
        {
            _menuItemDAO = new MenuItemDAO();
        }

        public List<MenuItem> GetAllMenuItems()
        {
            return _menuItemDAO.GetAllMenuItems();
        }

        public List<MenuItem> GetMenuItemsByCategory(int categoryId)
        {
            return _menuItemDAO.GetMenuItemsByCategory(categoryId);
        }

        public MenuItem GetMenuItemById(int id)
        {
            return _menuItemDAO.GetMenuItemById(id);
        }

        public bool SaveMenuItem(MenuItem menuItem)
        {
            return _menuItemDAO.SaveMenuItem(menuItem);
        }

        public bool UpdateMenuItem(MenuItem menuItem)
        {
            return _menuItemDAO.UpdateMenuItem(menuItem);
        }

        public bool DeleteMenuItem(int id)
        {
            return _menuItemDAO.DeleteMenuItem(id);
        }

        public List<MenuItem> SearchMenuItems(string searchTerm)
        {
            return _menuItemDAO.SearchMenuItems(searchTerm);
        }

        public List<MenuItem> GetAvailableMenuItems()
        {
            return _menuItemDAO.GetAvailableMenuItems();
        }

        public bool UpdateMenuItemAvailability(int id, bool isAvailable)
        {
            return _menuItemDAO.UpdateMenuItemAvailability(id, isAvailable);
        }

        public List<MenuItem> GetMenuItemsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _menuItemDAO.GetMenuItemsByPriceRange(minPrice, maxPrice);
        }
    }
}
