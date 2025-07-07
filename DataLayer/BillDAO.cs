using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BillDAO
    {
        SushiRestaurantContext _context = new SushiRestaurantContext();

        public List<Bill> GetAllBills()
        {
            return _context.Bills.ToList();
        }

        public Bill? GetBillById(int id)
        {
            return _context.Bills.FirstOrDefault(b => b.BillId == id);
        }

        public bool AddBill(Bill bill)
        {
            if (bill == null) return false;
            Bill? existingBill = _context.Bills.FirstOrDefault(b => b.BillId == bill.BillId);
            if (existingBill != null) return false;
            return _context.SaveChanges() > 0;
        }

        public bool UpdateBill(Bill bill)
        {
            if (bill == null) return false;
            Bill? existingBill = _context.Bills.FirstOrDefault(b => b.BillId == bill.BillId);
            if (existingBill == null) return false;
            existingBill.TableId = bill.TableId;
            existingBill.BillDate = bill.BillDate;
            existingBill.TotalAmount = bill.TotalAmount;
            existingBill.Status = bill.Status;
            existingBill.CustomerId = bill.CustomerId;
            return _context.SaveChanges() > 0;
        }

        public bool DeleteBill(int id)
        {
            Bill? existingBill = _context.Bills.FirstOrDefault(b => b.BillId == id);
            if (existingBill == null) return false;
            _context.Bills.Remove(existingBill);
            return _context.SaveChanges() > 0;
        }

        public List<Bill> GetBillsByCustomerId(int customerId)
        {
            return _context.Bills.Where(b => b.CustomerId == customerId).ToList();
        }

        public List<Bill> GetBillsByTableId(int tableId)
        {
            return _context.Bills.Where(b => b.TableId == tableId).ToList();
        }

    }
}
