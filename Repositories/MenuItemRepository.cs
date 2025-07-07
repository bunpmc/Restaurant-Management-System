using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        MenuItemDAO _menuItemDAO = new MenuItemDAO();
        public bool AddMenuItem(MenuItem menuItem)
        {
            return _menuItemDAO.AddMenuItem(menuItem);
        }

        public bool DeleteMenuItem(int id)
        {
            return _menuItemDAO.DeleteMenuItem(id);
        }

        public List<MenuItem> GetAllMenuItems()
        {
            return _menuItemDAO.GetAllMenuItems();
        }

        public List<MenuItem> GetAvailableMenuItems()
        {
            return _menuItemDAO.GetAvailableMenuItems();
        }

        public MenuItem? GetMenuItemById(int id)
        {
            return _menuItemDAO.GetMenuItemById(id);
        }

        public List<MenuItem> GetMenuItemsByCategoryId(int categoryId)
        {
            return _menuItemDAO.GetMenuItemsByCategoryId(categoryId);
        }

        public bool UpdateMenuItem(MenuItem menuItem)
        {
            return _menuItemDAO.UpdateMenuItem(menuItem);
        }
    }
}
