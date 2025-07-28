using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService()
        {
            _employeeRepository = new EmployeeRepository();
        }

        public bool ActivateEmployee(int employeeId)
        {
            return _employeeRepository.ActivateEmployee(employeeId);
        }

        public bool AddEmployee(Employee employee)
        {
            return _employeeRepository.AddEmployee(employee);
        }

        public bool DeactivateEmployee(int employeeId)
        {
            return _employeeRepository.DeactivateEmployee(employeeId);
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeRepository.GetAllEmployees();
        }

        public Employee? Login(string username, string password)
        {
            return _employeeRepository.Login(username, password);
        }

        public bool ResetPassword(int employeeId, string newPasswordHash)
        {
            return _employeeRepository.ResetPassword(employeeId, newPasswordHash);
        }

        public bool UpdateEmployee(Employee employee)
        {
           return _employeeRepository.UpdateEmployee(employee);
        }
    }
}
