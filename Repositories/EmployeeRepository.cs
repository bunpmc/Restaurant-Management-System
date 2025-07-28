using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;

namespace Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        EmployeeDAO employeeDAO = new EmployeeDAO();

        public bool ActivateEmployee(int employeeId)
        {
            return employeeDAO.ActivateEmployee(employeeId);
        }

        public bool AddEmployee(Employee employee)
        {
            return employeeDAO.AddEmployee(employee);
        }

        public bool DeactivateEmployee(int employeeId)
        {
            return employeeDAO.DeactivateEmployee(employeeId);
        }

        public List<Employee> GetAllEmployees()
        {
            return employeeDAO.GetAllEmployees();
        }

        public Employee? Login(string username, string password)
        {
            return employeeDAO.Login(username, password);
        }

        public bool ResetPassword(int employeeId, string newPasswordHash)
        {
            return employeeDAO.ResetPassword(employeeId, newPasswordHash);
        }

        public bool UpdateEmployee(Employee employee)
        {
            return employeeDAO.UpdateEmployee(employee);
        }
    }
}
