using BusinessObjects;
using DataAccessLayer;
using System.Collections.Generic;

namespace Repositories
{
    public class MenuCategoryRepository : IMenuCategoryRepository
    {
        private readonly IMenuCategoryDAO _menuCategoryDAO;

        public MenuCategoryRepository()
        {
            _menuCategoryDAO = new MenuCategoryDAO();
        }

        public List<MenuCategory> GetAllMenuCategories()
        {
            return _menuCategoryDAO.GetAllMenuCategories();
        }

        public MenuCategory GetMenuCategoryById(int id)
        {
            return _menuCategoryDAO.GetMenuCategoryById(id);
        }

        public bool SaveMenuCategory(MenuCategory category)
        {
            return _menuCategoryDAO.SaveMenuCategory(category);
        }

        public bool UpdateMenuCategory(MenuCategory category)
        {
            return _menuCategoryDAO.UpdateMenuCategory(category);
        }

        public bool DeleteMenuCategory(int id)
        {
            return _menuCategoryDAO.DeleteMenuCategory(id);
        }

        public List<MenuCategory> SearchMenuCategories(string searchTerm)
        {
            return _menuCategoryDAO.SearchMenuCategories(searchTerm);
        }

        public bool IsCategoryInUse(int categoryId)
        {
            return _menuCategoryDAO.IsCategoryInUse(categoryId);
        }
    }
}
