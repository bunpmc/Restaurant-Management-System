using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public interface IOrderRepository
    {
        public List<Order> GetAllOrders();
        public Order? GetOrderById(int id);
        public bool AddOrder(Order order);
        public bool UpdateOrder(Order order);
        public bool DeleteOrder(int id);
        public List<Order> GetOrdersByCustomerId(int customerId);
        public List<Order> GetOrdersByEmployeeId(int employeeId);
        public List<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate);
    }
}
