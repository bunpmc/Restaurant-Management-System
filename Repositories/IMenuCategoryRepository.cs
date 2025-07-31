using BusinessObjects;
using System.Collections.Generic;

namespace Repositories
{
    public interface IMenuCategoryRepository
    {
        List<MenuCategory> GetAllMenuCategories();
        MenuCategory GetMenuCategoryById(int id);
        bool SaveMenuCategory(MenuCategory category);
        bool UpdateMenuCategory(MenuCategory category);
        bool DeleteMenuCategory(int id);
        List<MenuCategory> SearchMenuCategories(string searchTerm);
        bool IsCategoryInUse(int categoryId);
    }
}
