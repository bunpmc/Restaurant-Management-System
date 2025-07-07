using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Repositories;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeRepository _employeeRepository;
        public EmployeeService()
        {
            _employeeRepository = new EmployeeRepository();
        }
        public bool AddEmployee(Employee employee)
        {
            return _employeeRepository.AddEmployee(employee);
        }

        public Employee? Login(string username, string password)
        {
            return _employeeRepository.Login(username, password);
        }
        public bool DeleteEmployee(int id)
        {
            return _employeeRepository.DeleteEmployee(id);
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeRepository.GetAllEmployees();
        }

        public Employee? GetEmployeeById(int id)
        {
            return _employeeRepository.GetEmployeeById(id);
        }

        public bool UpdateEmployee(Employee employee)
        {
            return _employeeRepository.UpdateEmployee(employee);
        }
    }
}
