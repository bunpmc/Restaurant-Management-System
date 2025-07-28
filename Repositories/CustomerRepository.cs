using BusinessObjects;
using DataAccessLayer;
using System.Collections.Generic;

namespace Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ICustomerDAO _customerDAO;

        public CustomerRepository()
        {
            _customerDAO = new CustomerDAO();
        }

        public List<Customer> GetAllCustomers()
        {
            return _customerDAO.GetAllCustomers();
        }

        public Customer GetCustomerById(int id)
        {
            return _customerDAO.GetCustomerById(id);
        }

        public bool SaveCustomer(Customer customer)
        {
            return _customerDAO.SaveCustomer(customer);
        }

        public bool UpdateCustomer(Customer customer)
        {
            return _customerDAO.UpdateCustomer(customer);
        }

        public bool DeleteCustomer(int id)
        {
            return _customerDAO.DeleteCustomer(id);
        }

        public List<Customer> SearchCustomers(string searchTerm)
        {
            return _customerDAO.SearchCustomers(searchTerm);
        }
    }
}
