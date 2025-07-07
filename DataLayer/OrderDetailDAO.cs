using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class OrderDetailDAO
    {
        SushiRestaurantContext _context = new SushiRestaurantContext();
        public List<OrderDetail> GetAllOrderDetails()
        {
            return _context.OrderDetails.ToList();
        }
        public OrderDetail? GetOrderDetailById(int id)
        {
            return _context.OrderDetails.FirstOrDefault(od => od.OrderDetailId == id);
        }
        public bool AddOrderDetail(OrderDetail orderDetail)
        {
            if (orderDetail == null) return false;
            _context.OrderDetails.Add(orderDetail);
            return _context.SaveChanges() > 0;
        }
        public bool UpdateOrderDetail(OrderDetail orderDetail)
        {
            if (orderDetail == null) return false;
            var existingOrderDetail = _context.OrderDetails.FirstOrDefault(od => od.OrderDetailId == orderDetail.OrderDetailId);
            if (existingOrderDetail == null) return false;
            existingOrderDetail.OrderId = orderDetail.OrderId;
            existingOrderDetail.MenuItemId = orderDetail.MenuItemId;
            existingOrderDetail.Quantity = orderDetail.Quantity;
            existingOrderDetail.Price = orderDetail.Price;
            return _context.SaveChanges() > 0;
        }
        public bool DeleteOrderDetail(int id)
        {
            var orderDetail = _context.OrderDetails.FirstOrDefault(od => od.OrderDetailId == id);
            if (orderDetail == null) return false;
            _context.OrderDetails.Remove(orderDetail);
            return _context.SaveChanges() > 0;
        }
        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _context.OrderDetails.Where(od => od.OrderId == orderId).ToList();
        }
    }
}
