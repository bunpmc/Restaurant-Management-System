using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAllCategories();
        Category? GetCategoryById(int id);
        bool AddCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(int id);
        List<Category> SearchCategories(string searchTerm);
        List<Category> GetCategoriesWithMenuItems();
    }
}
