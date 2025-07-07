using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Services
{
    public interface IMenuItemService
    {
        List<MenuItem> GetAllMenuItems();
        MenuItem? GetMenuItemById(int id);
        bool AddMenuItem(MenuItem menuItem);
        bool UpdateMenuItem(MenuItem menuItem);
        bool DeleteMenuItem(int id);
        List<MenuItem> GetMenuItemsByCategoryId(int categoryId);
        List<MenuItem> GetAvailableMenuItems();
    }
}
