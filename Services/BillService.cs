using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class BillService : IBillService
    {
        private readonly BillRepository _billRepository;
        public BillService()
        {
            _billRepository = new BillRepository();
        }
        public bool AddBill(Bill bill)
        {
            return _billRepository.AddBill(bill);
        }

        public bool DeleteBill(int id)
        {
            return _billRepository.DeleteBill(id);
        }

        public List<Bill> GetAllBills()
        {
            return _billRepository.GetAllBills();
        }

        public Bill? GetBillById(int id)
        {
            return _billRepository.GetBillById(id);
        }

        public List<Bill> GetBillsByCustomerId(int customerId)
        {
            return _billRepository.GetBillsByCustomerId(customerId);
        }

        public List<Bill> GetBillsByTableId(int tableId)
        {
            return _billRepository.GetBillsByTableId(tableId);
        }

        public bool UpdateBill(Bill bill)
        {
            return _billRepository.UpdateBill(bill);
        }
    }
}
