using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        EmployeeDAO _employeeDAO = new EmployeeDAO();
        public bool AddEmployee(Employee employee)
        {
            return _employeeDAO.AddEmployee(employee);
        }

        public Employee? Login(string username, string password)
        {
            return _employeeDAO.Login(username, password);
        }

        public bool DeleteEmployee(int id)
        {
            return _employeeDAO.DeleteEmployee(id);
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeDAO.GetAllEmployees();
        }

        public Employee? GetEmployeeById(int id)
        {
            return _employeeDAO.GetEmployeeById(id);
        }

        public bool UpdateEmployee(Employee employee)
        {
            return _employeeDAO.UpdateEmployee(employee);
        }
    }
}
