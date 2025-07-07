using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly OrderDetailRepository _orderDetailRepository;

        public OrderDetailService()
        {
            _orderDetailRepository = new OrderDetailRepository();
        }
        public bool AddOrderDetail(OrderDetail orderDetail)
        {
            return _orderDetailRepository.AddOrderDetail(orderDetail);
        }

        public bool DeleteOrderDetail(int id)
        {
            return _orderDetailRepository.DeleteOrderDetail(id);
        }

        public List<OrderDetail> GetAllOrderDetails()
        {
            return _orderDetailRepository.GetAllOrderDetails();
        }

        public OrderDetail? GetOrderDetailById(int id)
        {
            return _orderDetailRepository.GetOrderDetailById(id);
        }

        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
        }

        public bool UpdateOrderDetail(OrderDetail orderDetail)
        {
            return _orderDetailRepository.UpdateOrderDetail(orderDetail);
        }
    }
}
