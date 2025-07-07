using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Services
{
    public interface IEmployeeService
    {
        public List<Employee> GetAllEmployees();
        public Employee? GetEmployeeById(int id);
        public Employee? Login(string username, string password);
        public bool AddEmployee(Employee employee);
        public bool UpdateEmployee(Employee employee);
        public bool DeleteEmployee(int id);
    }
}
