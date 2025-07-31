using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories
{
    public interface IOrderRepository
    {
        public bool SaveOrder(Order order);
        public List<Order> GetAllOrders();
        public List<Order> GetOrdersByDateRange(DateTime fromDate, DateTime toDate);
        public List<Order> GetOrdersByStatus(string status);
        public List<Order> GetOrdersByEmployee(int employeeId);
        public List<Order> GetOrdersByTable(int tableId);
        public Order GetOrderById(int id);
        public bool UpdateOrder(Order order);
        public bool DeleteOrder(int id);
    }
}
