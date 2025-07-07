using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
        }
        public bool AddCustomer(Customer customer)
        {
            return _customerRepository.AddCustomer(customer);
        }

        public bool DeleteCustomer(int id)
        {
            return _customerRepository.DeleteCustomer(id);
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAllCustomers();
        }

        public Customer? GetCustomerById(int id)
        {
            return _customerRepository.GetCustomerById(id);
        }

        public List<Customer> SearchCustomers(string searchTerm)
        {
            return _customerRepository.SearchCustomers(searchTerm);
        }

        public bool UpdateCustomer(Customer customer)
        {
            return _customerRepository.UpdateCustomer(customer);
        }
    }
}
