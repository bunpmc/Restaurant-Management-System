using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        CustomerDAO _customerDAO = new CustomerDAO();
        public bool AddCustomer(Customer customer)
        {
            return _customerDAO.AddCustomer(customer);
        }

        public bool DeleteCustomer(int id)
        {
            return _customerDAO.DeleteCustomer(id);
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerDAO.GetAllCustomers();
        }

        public Customer? GetCustomerById(int id)
        {
            return _customerDAO.GetCustomerById(id);
        }

        public List<Customer> SearchCustomers(string searchTerm)
        {
            return _customerDAO.SearchCustomers(searchTerm);
        }

        public bool UpdateCustomer(Customer customer)
        {
            return _customerDAO.UpdateCustomer(customer);
        }
    }
}
