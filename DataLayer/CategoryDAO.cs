using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CategoryDAO
    {
        SushiRestaurantContext _context = new SushiRestaurantContext();
        public List<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
        public Category? GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryId == id);
        }
        public bool AddCategory(Category category)
        {
            if (category == null) return false;
            Category? existingCategory = _context.Categories.FirstOrDefault(c => c.CategoryId == category.CategoryId);
            if (existingCategory != null) return false;
            _context.Categories.Add(category);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateCategory(Category category)
        {
            if (category == null) return false;
            Category? existingCategory = _context.Categories.FirstOrDefault(c => c.CategoryId == category.CategoryId);
            if (existingCategory == null) return false;
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.Description = category.Description;
            return _context.SaveChanges() > 0;
        }

        public bool DeleteCategory(int id)
        {
            Category? category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null) return false;
            _context.Categories.Remove(category);
            return _context.SaveChanges() > 0;
        }

        public List<Category> SearchCategories(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return GetAllCategories();
            return _context.Categories
                .Where(c => c.CategoryName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            (c.Description != null && c.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public List<Category> GetCategoriesWithMenuItems()
        {
            return _context.Categories
                .Where(c => c.MenuItems.Any())
                .ToList();
        }
    }
}
