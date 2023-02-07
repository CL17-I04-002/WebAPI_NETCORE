using EmployeeAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAPI.Repository.IRepository
{
    public interface IEmployeeRepository
    {
        bool SaveEmployee(Employee employee);
        bool UpdateEmployee(Employee employee);
        bool DeleteEmployee(Employee employee);
        Employee GetEmployee(int id);
        bool ExistEmployee(int id);
        bool ExistEmployee(string name);
        IEnumerable<Employee> GetAllEmployee();
        bool Save();
    }
}
