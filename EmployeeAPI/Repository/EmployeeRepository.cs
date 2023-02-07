using EmployeeAPI.Context;
using EmployeeAPI.Model;
using EmployeeAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAPI.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public bool DeleteEmployee(Employee employee)
        {
            context.Employ.Remove(employee);
            return Save();
        }

        public bool ExistEmployee(int id)
        {
            return context.Employ.Any(x => x.Id.Equals(id));
        }

        public bool ExistEmployee(string name)
        {
            return context.Employ.Any(x => x.Name.Trim().ToLower().Equals(name.Trim().ToLower()));
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return context.Employ.ToList();
        }

        public Employee GetEmployee(int id)
        {
            return context.Employ.FirstOrDefault(x => x.Id.Equals(id));
        }

        public bool Save()
        {
            return context.SaveChanges() > 0 ? true : false;
        }

        public bool SaveEmployee(Employee employee)
        {
            context.Employ.Add(employee);
            return Save();
        }

        public bool UpdateEmployee(Employee employee)
        {
            context.Update(employee);
            return Save();
        }
    }
}
