using OnlineStore2023.DataContext;
using OnlineStore2023.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore2023.Model
{
    public class CustomerModel
    {
        public CustomerModel()
        {
        }
        public CustomerModel(
            Guid userID,
            string userLogin,
            string lastName,
            string firstName,
            string middleName,
            string userType,
            string userPhoneNumber,
            string email,
            string adress,
            bool? isActive)
        {
            UserID = userID;
            UserLogin = userLogin;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            UserType = userType;
            UserPhoneNumber = userPhoneNumber;
            Email = email;
            CustomerAdressLine = adress;
            IsActive = (bool)isActive;
        }

        public Guid UserID { get; set; }

        public string UserLogin { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string UserType { get; set; }

        public string UserPhoneNumber { get; set; }

        public string Email { get; set; }

        public string CustomerAdressLine { get; set; }

        public bool IsActive { get; set; }
    }
}