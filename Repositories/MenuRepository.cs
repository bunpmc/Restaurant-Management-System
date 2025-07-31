using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class MenuRepository : IMenuRepository
    {
        MenuCategoryDAO categoryDAO = new MenuCategoryDAO();
        MenuItemDAO menuItemDAO = new MenuItemDAO();
        public List<MenuCategory> GetCategories()
        {
            return categoryDAO.GetCategories();
        }

        public List<MenuItem> GetMenuItems()
        {
            return menuItemDAO.GetMenuItems();
        }

        public List<MenuItem> GetMenuItemsByCategory(int? categoryId)
        {
            return menuItemDAO.GetMenuItemsByCategory(categoryId);
        }
    }
}
