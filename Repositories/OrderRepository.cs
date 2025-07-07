using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        OrderDAO _orderDAO = new OrderDAO();
        public bool AddOrder(Order order)
        {
            return _orderDAO.AddOrder(order);
        }

        public bool DeleteOrder(int id)
        {
            return _orderDAO.DeleteOrder(id);
        }

        public List<Order> GetAllOrders()
        {
            return _orderDAO.GetAllOrders();
        }

        public Order? GetOrderById(int id)
        {
            return _orderDAO.GetOrderById(id);
        }

        public List<Order> GetOrdersByCustomerId(int customerId)
        {
            return _orderDAO.GetOrdersByCustomerId(customerId);
        }

        public List<Order> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            return _orderDAO.GetOrdersByDateRange(startDate, endDate);
        }

        public List<Order> GetOrdersByEmployeeId(int employeeId)
        {
            return _orderDAO.GetOrdersByEmployeeId(employeeId);
        }

        public bool UpdateOrder(Order order)
        {
            return _orderDAO.UpdateOrder(order);
        }
    }
}
