using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EmployeeDAO
    {
        SushiRestaurantContext _context = new SushiRestaurantContext();

        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public Employee? GetEmployeeById(int id)
        {
            return _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
        }

        public Employee? Login (string username, string password)
        {
            return _context.Employees.FirstOrDefault(e => e.UserName == username && e.Password == password);
        }

        public bool AddEmployee(Employee employee)
        {
            if (employee == null) return false;
            _context.Employees.Add(employee);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateEmployee(Employee employee)
        {
            if (employee == null) return false;
            var existingEmployee = _context.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);
            if (existingEmployee == null) return false;
            existingEmployee.Name = employee.Name;
            existingEmployee.UserName = employee.UserName;
            existingEmployee.Password = employee.Password;
            existingEmployee.HireDate = employee.HireDate;
            existingEmployee.BirthDate = employee.BirthDate;
            existingEmployee.Address = employee.Address;
            return _context.SaveChanges() > 0;
        }

        public bool DeleteEmployee(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            if (employee == null) return false;
            _context.Employees.Remove(employee);
            return _context.SaveChanges() > 0;
        }

    }
}
