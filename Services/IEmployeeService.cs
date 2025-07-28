using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Services
{
    public interface IEmployeeService
    {
        public List<Employee> GetAllEmployees();
        public Employee? Login(string username, string password);

        public bool AddEmployee(Employee employee);
        public bool UpdateEmployee(Employee employee);
        public bool DeactivateEmployee(int employeeId);

        public bool ResetPassword(int employeeId, string newPasswordHash);

        public bool ActivateEmployee(int employeeId);
    }
}
