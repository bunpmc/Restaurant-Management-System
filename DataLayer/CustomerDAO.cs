using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CustomerDAO
    {
        SushiRestaurantContext _context = new SushiRestaurantContext();
        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }
        public Customer? GetCustomerById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.CustomerId == id);
        }
        public bool AddCustomer(Customer customer)
        {
            if (customer == null) return false;
            Customer? existingCustomer = _context.Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
            if (existingCustomer != null) return false;
            _context.Customers.Add(customer);
            return _context.SaveChanges() > 0;
        }
        public bool UpdateCustomer(Customer customer)
        {
            if (customer == null) return false;
            Customer? existingCustomer = _context.Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
            if (existingCustomer == null) return false;
            existingCustomer.Name = customer.Name;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.Note = customer.Note;
            return _context.SaveChanges() > 0;
        }
        public bool DeleteCustomer(int id)
        {
            Customer? customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);
            if (customer == null) return false;
            _context.Customers.Remove(customer);
            return _context.SaveChanges() > 0;
        }
        public List<Customer> SearchCustomers(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return GetAllCustomers();
            return _context.Customers
                .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            (c.Phone != null && c.Phone.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                            (c.Note != null && c.Note.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }
    }
}
