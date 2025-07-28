using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories
{
    public interface IMenuRepository
    {
        public List<MenuItem> GetMenuItems();
        public List<MenuCategory> GetCategories();
        public List<MenuItem> GetMenuItemsByCategory(int? categoryId);
    }
}
