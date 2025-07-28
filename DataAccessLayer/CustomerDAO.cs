using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class CustomerDAO : ICustomerDAO
    {
        public List<Customer> GetAllCustomers()
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.Customers.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving customers: {ex.Message}");
            }
        }

        public Customer GetCustomerById(int id)
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.Customers.FirstOrDefault(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving customer: {ex.Message}");
            }
        }

        public bool SaveCustomer(Customer customer)
        {
            try
            {
                using var context = new SakanaHouseContext();
                context.Customers.Add(customer);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error saving customer: {ex.Message}");
            }
        }

        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                using var context = new SakanaHouseContext();
                context.Customers.Update(customer);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating customer: {ex.Message}");
            }
        }

        public bool DeleteCustomer(int id)
        {
            try
            {
                using var context = new SakanaHouseContext();
                var customer = context.Customers.FirstOrDefault(c => c.Id == id);
                if (customer != null)
                {
                    context.Customers.Remove(customer);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting customer: {ex.Message}");
            }
        }

        public List<Customer> SearchCustomers(string searchTerm)
        {
            try
            {
                using var context = new SakanaHouseContext();
                return context.Customers
                    .Where(c => c.FullName.Contains(searchTerm) ||
                               c.Phone.Contains(searchTerm) ||
                               c.Email.Contains(searchTerm))
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching customers: {ex.Message}");
            }
        }
    }
}
