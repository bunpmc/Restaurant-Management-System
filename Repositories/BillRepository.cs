using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public class BillRepository : IBillRepository
    {
        BillDAO bd = new BillDAO();

        public bool AddBill(Bill bill)
        {
            return bd.AddBill(bill);
        }

        public bool DeleteBill(int id)
        {
            return bd.DeleteBill(id);
        }

        public List<Bill> GetAllBills()
        {
            return bd.GetAllBills();
        }

        public Bill? GetBillById(int id)
        {
            return bd.GetBillById(id);
        }

        public List<Bill> GetBillsByCustomerId(int customerId)
        {
            return bd.GetBillsByCustomerId(customerId);
        }

        public List<Bill> GetBillsByTableId(int tableId)
        {
            return bd.GetBillsByTableId(tableId);
        }

        public bool UpdateBill(Bill bill)
        {
            return bd.UpdateBill(bill);
        }
    }
}
