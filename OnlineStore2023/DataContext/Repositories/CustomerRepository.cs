using Microsoft.EntityFrameworkCore;
using OnlineStore2023.DataContext.Models;
using OnlineStore2023.Model;
using OnlineStore2023.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore2023.DataContext.Repositories
{
    class CustomerRepository : OnlineShop2022Context
    {
        public CustomerRepository() : base() { }
        //public List<CustomerModel> GetCustomerModelsFromDB()
        //{
        //    try
        //    {
        //        List<CustomerModel> customerModels = new();
        //        OnlineShop2022Context dataContext = new();

        //        var x = from cust in dataContext.Customers
        //                join usr in dataContext.Users
        //                on cust.UserId equals usr.UserId
        //                join usrData in dataContext.UsersData
        //                on usr.UserId equals usrData.UserId
        //                select new
        //                {
        //                    UserID = cust.UserId,
        //                    usr.UserLogin,
        //                    LastName = usrData.UserLastName,
        //                    FirstName = usrData.UserFirstName,
        //                    MiddleName = usrData.UserMiddleName,
        //                    usrData.UserPhoneNumber,
        //                    cust.CustomerEmailAdress,
        //                    usr.IsActive,
        //                    cust.CustomerAdressLine,
        //                    usr.UserType
        //                };
        //        foreach (var cust in x.ToList())
        //        {
        //            customerModels.Add(new CustomerModel(
        //                cust.UserID, cust.UserLogin,
        //                cust.LastName, cust.FirstName,
        //                cust.MiddleName, cust.UserType,
        //                cust.UserPhoneNumber, cust.CustomerEmailAdress,
        //                cust.CustomerAdressLine,
        //                cust.IsActive));
        //        }
        //        return customerModels;
        //    }
        //    catch (Exception ex)
        //    {
        //        string mess = "При загрузке данных произошла ошибка!\n";
        //        StandardMessages.Error(mess + ex.Message);
        //    }
        //    return null;
        //}
        public List<Customer> GetCustomerList()
        {
            return Customers.Include(x => x.User)
                .ThenInclude(x => x.UsersDatum)
                .Where(x => x.User.UsersDatum != null)
                .ToList();
        }
        public Customer GetCustomerById(Guid id)
        {
            return Customers.Include(x => x.User)
                .ThenInclude(x => x.UsersDatum)
                .FirstOrDefault(x => x.UserId == id);
        }
    }
}
