using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services
{
    public interface IOrderService
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

        // Report specific methods
        public decimal GetTotalSalesByDateRange(DateTime fromDate, DateTime toDate);
        public int GetOrderCountByDateRange(DateTime fromDate, DateTime toDate);
        public decimal GetAverageOrderValueByDateRange(DateTime fromDate, DateTime toDate);
        public List<Order> GetCompletedOrdersByDateRange(DateTime fromDate, DateTime toDate);
    }
}
