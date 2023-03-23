using Microsoft.EntityFrameworkCore;
using OnlineStore2023.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace OnlineStore2023.DataContext.Repositories
{
    internal class EmployeeRepository : OnlineShop2022Context
    {
        public EmployeeRepository() : base() { }

        public Employee GetSimpleEmployeeById(Guid id)
        {
            return Employees
                .Include(x => x.User)
                .ThenInclude(x => x.UsersDatum)
                .FirstOrDefault(x => x.UserId == id);
        }

        public List<Employee> GetEmployeeList()
        {
            return Employees.Include(x => x.User)
                .ThenInclude(x => x.UsersDatum)
                .Where(x => x.User.UsersDatum != null)
                .ToList();
        }

        public List<Employee> GetEmployeeListById(Guid guid)
        {
            return Employees.Include(x => x.User)
                .ThenInclude(x => x.UsersDatum)
                .Where(x => x.UserId == guid)
                .ToList();
        }

        public List<Employee> GetEmployeeListByLogin(string login)
        {
            return Employees.Include(x => x.User)
                .ThenInclude(x => x.UsersDatum)
                .Where(x => x.User.UserLogin == login)
                .ToList();
        }

        public List<Employee> GetEmployeeListBySurname(string surname)
        {
            return Employees.Include(x => x.User)
                .ThenInclude(x => x.UsersDatum)
                .Where(x => x.User.UsersDatum.UserLastName == surname)
                .ToList();
        }

        public List<Employee> GetEmployeeListByJobTitle(string jobTitle)
        {
            return Employees.Include(x => x.User)
                .ThenInclude(x => x.UsersDatum)
                .Where(x => x.JobTitle == jobTitle)
                .ToList();
        }

        public List<Employee> GetEmployeeListByActive(bool isActive)
        {
            return Employees.Include(x => x.User)
                .ThenInclude(x => x.UsersDatum)
                .Where(x => x.User.IsActive == isActive)
                .ToList();
        }

        public bool DeleteRange(IEnumerable<Employee> employees)
        {
            try
            {
                Employees.RemoveRange(employees);
                SaveChanges();
                MessageBox.Show("Данные успешно удалены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }
            return true;
            
        }

    }
}
