using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public interface IOrderDetailRepository
    {
        public List<OrderDetail> GetAllOrderDetails();
        public OrderDetail? GetOrderDetailById(int id);
        public bool AddOrderDetail(OrderDetail orderDetail);
        public bool UpdateOrderDetail(OrderDetail orderDetail);
        public bool DeleteOrderDetail(int id);
        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId);

    }
}
