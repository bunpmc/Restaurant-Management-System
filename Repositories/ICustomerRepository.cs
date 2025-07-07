using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAllCustomers();
        public Customer? GetCustomerById(int id);
        public bool AddCustomer(Customer customer);
        public bool UpdateCustomer(Customer customer);
        public bool DeleteCustomer(int id);
        public List<Customer> SearchCustomers(string searchTerm);
    }
}
