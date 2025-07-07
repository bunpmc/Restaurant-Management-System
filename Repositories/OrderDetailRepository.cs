using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        OrderDetailDAO _orderDetailDAO = new OrderDetailDAO();
        public bool AddOrderDetail(OrderDetail orderDetail)
        {
            return _orderDetailDAO.AddOrderDetail(orderDetail);
        }

        public bool DeleteOrderDetail(int id)
        {
            return _orderDetailDAO.DeleteOrderDetail(id);
        }

        public List<OrderDetail> GetAllOrderDetails()
        {
            return _orderDetailDAO.GetAllOrderDetails();
        }

        public OrderDetail? GetOrderDetailById(int id)
        {
            return _orderDetailDAO.GetOrderDetailById(id);
        }

        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _orderDetailDAO.GetOrderDetailsByOrderId(orderId);
        }

        public bool UpdateOrderDetail(OrderDetail orderDetail)
        {
            return _orderDetailDAO.UpdateOrderDetail(orderDetail);
        }
    }
}
