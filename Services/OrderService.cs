using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        public OrderService()
        {
            _orderRepository = new OrderRepository();
        }
        public bool AddOrder(Order order)
        {
            return _orderRepository.AddOrder(order);
        }

        public bool DeleteOrder(int id)
        {
            return _orderRepository.DeleteOrder(id);
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public Order? GetOrderById(int id)
        {
            return _orderRepository.GetOrderById(id);
        }

        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            return _orderRepository.GetOrdersByCustomerId(customerId);
        }

        public List<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            return _orderRepository.GetOrdersByDateRange(startDate, endDate);
        }

        public List<Order> GetOrdersByEmployeeId(int employeeId)
        {
            return _orderRepository.GetOrdersByEmployeeId(employeeId);
        }

        public bool UpdateOrder(Order order)
        {
            return _orderRepository.UpdateOrder(order);
        }
    }
}
