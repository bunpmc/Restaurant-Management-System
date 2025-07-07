using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class MenuItemDAO
    {
        SushiRestaurantContext _context = new SushiRestaurantContext();
        public List<MenuItem> GetAllMenuItems()
        {
            return _context.MenuItems.ToList();
        }
        public MenuItem? GetMenuItemById(int id)
        {
            return _context.MenuItems.FirstOrDefault(m => m.MenuItemId == id);
        }
        public bool AddMenuItem(MenuItem menuItem)
        {
            if (menuItem == null) return false;
            _context.MenuItems.Add(menuItem);
            return _context.SaveChanges() > 0;
        }
        public bool UpdateMenuItem(MenuItem menuItem)
        {
            if (menuItem == null) return false;
            var existingMenuItem = _context.MenuItems.FirstOrDefault(m => m.MenuItemId == menuItem.MenuItemId);
            if (existingMenuItem == null) return false;
            existingMenuItem.Name = menuItem.Name;
            existingMenuItem.Price = menuItem.Price;
            existingMenuItem.Description = menuItem.Description;
            existingMenuItem.CategoryId = menuItem.CategoryId;
            existingMenuItem.ImagePath = menuItem.ImagePath;
            existingMenuItem.IsAvailable = menuItem.IsAvailable;
            return _context.SaveChanges() > 0;
        }
        public bool DeleteMenuItem(int id)
        {
            var menuItem = _context.MenuItems.FirstOrDefault(m => m.MenuItemId == id);
            if (menuItem == null) return false;
            _context.MenuItems.Remove(menuItem);
            return _context.SaveChanges() > 0;
        }

        public List<MenuItem> GetMenuItemsByCategoryId(int categoryId)
        {
            return _context.MenuItems.Where(m => m.CategoryId == categoryId).ToList();
        }

        public List<MenuItem> GetAvailableMenuItems()
        {
            return _context.MenuItems.Where(m => m.IsAvailable).ToList();
        }
    }
}
