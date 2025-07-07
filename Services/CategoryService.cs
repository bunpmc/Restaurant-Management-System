using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryService()
        {
            _categoryRepository = new CategoryRepository();
        }
        public bool AddCategory(Category category)
        {
            return _categoryRepository.AddCategory(category);
        }

        public bool DeleteCategory(int id)
        {
            return _categoryRepository.DeleteCategory(id);
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories();
        }

        public List<Category> GetCategoriesWithMenuItems()
        {
            return _categoryRepository.GetCategoriesWithMenuItems();
        }

        public Category? GetCategoryById(int id)
        {
            return _categoryRepository.GetCategoryById(id);
        }

        public List<Category> SearchCategories(string searchTerm)
        {
            return _categoryRepository.SearchCategories(searchTerm);
        }

        public bool UpdateCategory(Category category)
        {
            return _categoryRepository.UpdateCategory(category);
        }
    }
}
