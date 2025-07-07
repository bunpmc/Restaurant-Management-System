using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        CategoryDAO _categoryDAO = new CategoryDAO();

        public List<Category> GetAllCategories()
        {
            return _categoryDAO.GetAllCategories();
        }

        public Category? GetCategoryById(int id)
        {
            return _categoryDAO.GetCategoryById(id);
        }

        public bool AddCategory(Category category)
        {
            return _categoryDAO.AddCategory(category);
        }

        public bool UpdateCategory(Category category)
        {
            return _categoryDAO.UpdateCategory(category);
        }

        public bool DeleteCategory(int id)
        {
            return _categoryDAO.DeleteCategory(id);
        }

        public List<Category> SearchCategories(string searchTerm)
        {
            return _categoryDAO.SearchCategories(searchTerm);
        }

        public List<Category> GetCategoriesWithMenuItems()
        {
            return _categoryDAO.GetCategoriesWithMenuItems();
        }
    }
}
