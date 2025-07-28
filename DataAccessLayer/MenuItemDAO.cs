using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class MenuItemDAO : IMenuItemDAO
    {
        public List<MenuItem> GetAllMenuItems()
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.MenuItems
                    .Include(m => m.Category)
                    .OrderBy(m => m.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menu items: {ex.Message}");
            }
        }

        public List<MenuItem> GetMenuItemsByCategory(int categoryId)
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.MenuItems
                    .Include(m => m.Category)
                    .Where(m => m.CategoryId == categoryId)
                    .OrderBy(m => m.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menu items by category: {ex.Message}");
            }
        }

        public MenuItem GetMenuItemById(int id)
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.MenuItems
                    .Include(m => m.Category)
                    .FirstOrDefault(m => m.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menu item: {ex.Message}");
            }
        }

        public bool SaveMenuItem(MenuItem menuItem)
        {
            try
            {
                using var context = new SakanaHouseContext();
                menuItem.CreatedAt = DateTime.Now;
                menuItem.IsAvailable = true;
                context.MenuItems.Add(menuItem);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving menu item: {ex.Message}");
            }
        }

        public bool UpdateMenuItem(MenuItem menuItem)
        {
            try
            {
                using var context = new SakanaHouseContext();
                context.MenuItems.Update(menuItem);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating menu item: {ex.Message}");
            }
        }

        public bool DeleteMenuItem(int id)
        {
            try
            {
                using var context = new SakanaHouseContext();
                var menuItem = context.MenuItems.FirstOrDefault(m => m.Id == id);
                if (menuItem != null)
                {
                    context.MenuItems.Remove(menuItem);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting menu item: {ex.Message}");
            }
        }

        public List<MenuItem> SearchMenuItems(string searchTerm)
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.MenuItems
                    .Include(m => m.Category)
                    .Where(m => m.Name.Contains(searchTerm) ||
                               (m.Description != null && m.Description.Contains(searchTerm)))
                    .OrderBy(m => m.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching menu items: {ex.Message}");
            }
        }

        public List<MenuItem> GetAvailableMenuItems()
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.MenuItems
                    .Include(m => m.Category)
                    .Where(m => m.IsAvailable == true)
                    .OrderBy(m => m.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving available menu items: {ex.Message}");
            }
        }

        public bool UpdateMenuItemAvailability(int id, bool isAvailable)
        {
            try
            {
                using var context = new SakanaHouseContext();
                var menuItem = context.MenuItems.FirstOrDefault(m => m.Id == id);
                if (menuItem != null)
                {
                    menuItem.IsAvailable = isAvailable;
                    return context.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating menu item availability: {ex.Message}");
            }
        }

        public List<MenuItem> GetMenuItemsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.MenuItems
                    .Include(m => m.Category)
                    .Where(m => m.Price >= minPrice && m.Price <= maxPrice)
                    .OrderBy(m => m.Price)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving menu items by price range: {ex.Message}");
            }
        }

        // Legacy methods for backward compatibility
        public List<MenuItem> GetMenuItems()
        {
            return GetAvailableMenuItems();
        }

        public List<MenuItem> GetMenuItemsByCategory(int? categoryId)
        {
            if (categoryId == null)
            {
                return GetAllMenuItems();
            }
            return GetMenuItemsByCategory(categoryId.Value);
        }
    }
}
