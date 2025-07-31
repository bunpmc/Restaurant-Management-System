using BusinessObjects;
using System.Collections.Generic;

namespace Services
{
    public interface ICustomerService
    {
        List<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        bool SaveCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(int id);
        List<Customer> SearchCustomers(string searchTerm);
    }
}
