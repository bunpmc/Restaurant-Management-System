using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly MenuItemRepository _menuItemRepository;

        public MenuItemService()
        {
            _menuItemRepository = new MenuItemRepository();
        }
        public bool AddMenuItem(MenuItem menuItem)
        {
            return _menuItemRepository.AddMenuItem(menuItem);
        }

        public bool DeleteMenuItem(int id)
        {
            return _menuItemRepository.DeleteMenuItem(id);
        }

        public List<MenuItem> GetAllMenuItems()
        {
            return _menuItemRepository.GetAllMenuItems();
        }

        public List<MenuItem> GetAvailableMenuItems()
        {
            return _menuItemRepository.GetAvailableMenuItems();
        }

        public MenuItem? GetMenuItemById(int id)
        {
            return _menuItemRepository.GetMenuItemById(id);
        }

        public List<MenuItem> GetMenuItemsByCategoryId(int categoryId)
        {
            return _menuItemRepository.GetMenuItemsByCategoryId(categoryId);
        }

        public bool UpdateMenuItem(MenuItem menuItem)
        {
            return _menuItemRepository.UpdateMenuItem(menuItem);
        }
    }
}
