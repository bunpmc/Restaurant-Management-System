using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class OrderDAO
    {
        SushiRestaurantContext _context = new SushiRestaurantContext();
        public List<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }
        public Order? GetOrderById(int id)
        {
            return _context.Orders.FirstOrDefault(o => o.OrderId == id);
        }
        public bool AddOrder(Order order)
        {
            if (order == null) return false;
            _context.Orders.Add(order);
            return _context.SaveChanges() > 0;
        }
        public bool UpdateOrder(Order order)
        {
            if (order == null) return false;
            var existingOrder = _context.Orders.FirstOrDefault(o => o.OrderId == order.OrderId);
            if (existingOrder == null) return false;
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.EmployeeId = order.EmployeeId;
            existingOrder.CustomerId = order.CustomerId;
            existingOrder.OrderDate = order.OrderDate;
            return _context.SaveChanges() > 0;
        }
        public bool DeleteOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null) return false;
            _context.Orders.Remove(order);
            return _context.SaveChanges() > 0;
        }
        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            return _context.Orders.Where(o => o.CustomerId == customerId).ToList();
        }
        public List<Order> GetOrdersByEmployeeId(int employeeId)
        {
            return _context.Orders.Where(o => o.EmployeeId == employeeId).ToList();
        }
        public List<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.Orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate).ToList();
        }
    }
}
