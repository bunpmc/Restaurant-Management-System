using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        OrderDAO orderDAO = new OrderDAO();

        public bool SaveOrder(Order order)
        {
            return orderDAO.SaveOrder(order);
        }

        public List<Order> GetAllOrders()
        {
            return orderDAO.GetAllOrders();
        }

        public List<Order> GetOrdersByDateRange(DateTime fromDate, DateTime toDate)
        {
            return orderDAO.GetOrdersByDateRange(fromDate, toDate);
        }

        public List<Order> GetOrdersByStatus(string status)
        {
            return orderDAO.GetOrdersByStatus(status);
        }

        public List<Order> GetOrdersByEmployee(int employeeId)
        {
            return orderDAO.GetOrdersByEmployee(employeeId);
        }

        public List<Order> GetOrdersByTable(int tableId)
        {
            return orderDAO.GetOrdersByTable(tableId);
        }

        public Order GetOrderById(int id)
        {
            return orderDAO.GetOrderById(id);
        }

        public bool UpdateOrder(Order order)
        {
            return orderDAO.UpdateOrder(order);
        }

        public bool DeleteOrder(int id)
        {
            return orderDAO.DeleteOrder(id);
        }
    }
}
