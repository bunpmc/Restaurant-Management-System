using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataAccessLayer
{
    public class EmployeeDAO
    {
        SakanaHouseContext context = new SakanaHouseContext();

        public List<Employee> GetAllEmployees()
        {
            return context.Employees.ToList();
        }

        public Employee? Login(string username, string password)
        {
            return context.Employees.FirstOrDefault(e => e.Username == username && e.PasswordHash == password);
        }

        public bool AddEmployee(Employee employee)
        {
            try
            {
                context.Employees.Add(employee);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding employee: {ex.Message}");
                return false;
            }
        }

        public bool UpdateEmployee(Employee employee)
        {
            try
            {
                var existingEmployee = context.Employees.Find(employee.Id);
                if (existingEmployee != null)
                {
                    existingEmployee.Username = employee.Username;
                    existingEmployee.PasswordHash = employee.PasswordHash;
                    existingEmployee.FullName = employee.FullName;
                    existingEmployee.Email = employee.Email;
                    existingEmployee.Role = employee.Role;
                    existingEmployee.IsActive = employee.IsActive;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating employee: {ex.Message}");
                return false;
            }
        }

        public bool DeactivateEmployee(int employeeId)
        {
            try
            {
                var employee = context.Employees.Find(employeeId);
                if (employee != null)
                {
                    employee.IsActive = false;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deactivating employee: {ex.Message}");
                return false;
            }
        }

        public bool ActivateEmployee(int employeeId)
        {
            try
            {
                var employee = context.Employees.Find(employeeId);
                if (employee != null)
                {
                    employee.IsActive = true;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error activating employee: {ex.Message}");
                return false;
            }
        }

        public bool ResetPassword(int employeeId, string newPasswordHash)
        {
            try
            {
                var employee = context.Employees.Find(employeeId);
                if (employee != null)
                {
                    employee.PasswordHash = newPasswordHash;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error resetting password: {ex.Message}");
                return false;
            }
        }
    }
}
