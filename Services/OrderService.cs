using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class OrderService : IOrderService
    {
        IOrderRepository orderRepository = new OrderRepository();

        public bool SaveOrder(Order order)
        {
            return orderRepository.SaveOrder(order);
        }

        public List<Order> GetAllOrders()
        {
            return orderRepository.GetAllOrders();
        }

        public List<Order> GetOrdersByDateRange(DateTime fromDate, DateTime toDate)
        {
            return orderRepository.GetOrdersByDateRange(fromDate, toDate);
        }

        public List<Order> GetOrdersByStatus(string status)
        {
            return orderRepository.GetOrdersByStatus(status);
        }

        public List<Order> GetOrdersByEmployee(int employeeId)
        {
            return orderRepository.GetOrdersByEmployee(employeeId);
        }

        public List<Order> GetOrdersByTable(int tableId)
        {
            return orderRepository.GetOrdersByTable(tableId);
        }

        public Order GetOrderById(int id)
        {
            return orderRepository.GetOrderById(id);
        }

        public bool UpdateOrder(Order order)
        {
            return orderRepository.UpdateOrder(order);
        }

        public bool DeleteOrder(int id)
        {
            return orderRepository.DeleteOrder(id);
        }

        // Report specific methods
        public decimal GetTotalSalesByDateRange(DateTime fromDate, DateTime toDate)
        {
            var orders = GetCompletedOrdersByDateRange(fromDate, toDate);
            return orders.Sum(o => o.TotalAmount ?? 0);
        }

        public int GetOrderCountByDateRange(DateTime fromDate, DateTime toDate)
        {
            var orders = GetCompletedOrdersByDateRange(fromDate, toDate);
            return orders.Count;
        }

        public decimal GetAverageOrderValueByDateRange(DateTime fromDate, DateTime toDate)
        {
            var orders = GetCompletedOrdersByDateRange(fromDate, toDate);
            if (orders.Count == 0) return 0;
            return orders.Sum(o => o.TotalAmount ?? 0) / orders.Count;
        }

        public List<Order> GetCompletedOrdersByDateRange(DateTime fromDate, DateTime toDate)
        {
            return GetOrdersByDateRange(fromDate, toDate)
                .Where(o => o.Status == "Completed")
                .ToList();
        }
    }
}
