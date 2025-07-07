using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public interface IBillRepository
    {
        public List<Bill> GetAllBills();
        public Bill? GetBillById(int id);
        public bool AddBill(Bill bill);
        public bool UpdateBill(Bill bill);
        public bool DeleteBill(int id);
        public List<Bill> GetBillsByCustomerId(int customerId);
        public List<Bill> GetBillsByTableId(int tableId);
    }
}
