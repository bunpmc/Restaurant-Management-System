using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class MenuCategoryDAO : IMenuCategoryDAO
    {
        public List<MenuCategory> GetAllMenuCategories()
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.MenuCategories
                    .Include(c => c.MenuItems)
                    .OrderBy(c => c.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menu categories: {ex.Message}");
            }
        }

        public MenuCategory GetMenuCategoryById(int id)
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.MenuCategories
                    .Include(c => c.MenuItems)
                    .FirstOrDefault(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menu category: {ex.Message}");
            }
        }

        public bool SaveMenuCategory(MenuCategory category)
        {
            try
            {
                using var context = new SakanaHouseContext();
                context.MenuCategories.Add(category);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving menu category: {ex.Message}");
            }
        }

        public bool UpdateMenuCategory(MenuCategory category)
        {
            try
            {
                using var context = new SakanaHouseContext();
                context.MenuCategories.Update(category);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating menu category: {ex.Message}");
            }
        }

        public bool DeleteMenuCategory(int id)
        {
            try
            {
                using var context = new SakanaHouseContext();
                var category = context.MenuCategories.FirstOrDefault(c => c.Id == id);
                if (category != null)
                {
                    context.MenuCategories.Remove(category);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting menu category: {ex.Message}");
            }
        }

        public List<MenuCategory> SearchMenuCategories(string searchTerm)
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.MenuCategories
                    .Include(c => c.MenuItems)
                    .Where(c => c.Name.Contains(searchTerm) ||
                           (c.Description != null && c.Description.Contains(searchTerm)))
                    .OrderBy(c => c.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching menu categories: {ex.Message}");
            }
        }

        public bool IsCategoryInUse(int categoryId)
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.MenuItems.Any(m => m.CategoryId == categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking category usage: {ex.Message}");
            }
        }

        // Legacy method for backward compatibility
        public List<MenuCategory> GetCategories()
        {
            return GetAllMenuCategories();
        }
    }
}
