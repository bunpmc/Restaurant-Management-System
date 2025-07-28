using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class OrderItemDAO
    {
        SakanaHouseContext context = new SakanaHouseContext();

        public List<OrderItem> GetAllOrderItems()
        {
            try
            {
                return context.OrderItems
                    .Include(oi => oi.MenuItem)
                        .ThenInclude(mi => mi.Category)
                    .Include(oi => oi.Order)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<OrderItem>();
            }
        }

        public List<OrderItem> GetOrderItemsByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                return context.OrderItems
                    .Include(oi => oi.MenuItem)
                        .ThenInclude(mi => mi.Category)
                    .Include(oi => oi.Order)
                    .Where(oi => oi.Order.OrderTime >= fromDate && oi.Order.OrderTime <= toDate.AddDays(1))
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<OrderItem>();
            }
        }

        public List<OrderItem> GetOrderItemsByMenuItem(int menuItemId)
        {
            try
            {
                return context.OrderItems
                    .Include(oi => oi.MenuItem)
                        .ThenInclude(mi => mi.Category)
                    .Include(oi => oi.Order)
                    .Where(oi => oi.MenuItemId == menuItemId)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<OrderItem>();
            }
        }

        public List<OrderItem> GetOrderItemsByOrder(int orderId)
        {
            try
            {
                return context.OrderItems
                    .Include(oi => oi.MenuItem)
                        .ThenInclude(mi => mi.Category)
                    .Include(oi => oi.Order)
                    .Where(oi => oi.OrderId == orderId)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<OrderItem>();
            }
        }

        // Get menu performance data for reports
        public List<MenuPerformanceData> GetMenuPerformanceByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var result = context.OrderItems
                    .Include(oi => oi.MenuItem)
                        .ThenInclude(mi => mi.Category)
                    .Include(oi => oi.Order)
                    .Where(oi => oi.Order.OrderTime >= fromDate &&
                                oi.Order.OrderTime <= toDate.AddDays(1) &&
                                oi.Order.Status == "Completed")
                    .GroupBy(oi => new { oi.MenuItemId, MenuItemName = oi.MenuItem.Name, CategoryName = oi.MenuItem.Category.Name })
                    .Select(g => new MenuPerformanceData
                    {
                        MenuItemId = g.Key.MenuItemId ?? 0,
                        MenuItemName = g.Key.MenuItemName,
                        CategoryName = g.Key.CategoryName,
                        QuantitySold = g.Sum(oi => oi.Quantity),
                        Revenue = g.Sum(oi => oi.Quantity * oi.UnitPrice),
                        AveragePrice = g.Average(oi => oi.UnitPrice)
                    })
                    .OrderByDescending(mp => mp.QuantitySold)
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<MenuPerformanceData>();
            }
        }
    }

    // Helper class for menu performance data
    public class MenuPerformanceData
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public decimal AveragePrice { get; set; }
    }
}
