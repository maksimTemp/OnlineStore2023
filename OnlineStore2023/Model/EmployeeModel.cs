using OnlineStore2023.DataContext;
using OnlineStore2023.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore2023.Model
{
    public class EmployeeModel
    {
        public EmployeeModel()
        {
        }
        public EmployeeModel(
            Guid userID,
            string userLogin,
            string lastName,
            string firstName,
            string middleName,
            string userType,
            string userPhoneNumber,
            string jobTitle,
            int employeePassportSeries,
            int employeePassportNumber,
            bool? isActive)
        {
            UserID = userID;
            UserLogin = userLogin;
            LastName = lastName;
            FirstName = firstName;
            MiddleName = middleName;
            UserType = userType;
            UserPhoneNumber = userPhoneNumber;
            JobTitle = jobTitle;
            EmployeePassportSeries= employeePassportSeries;
            EmployeePassportNumber = employeePassportNumber;
            IsActive = (bool)isActive;
        }

        public Guid UserID { get; set; }

        public string UserLogin { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string UserType { get; set; }

        public string UserPhoneNumber { get; set; }

        public string JobTitle { get; set; }

        public int EmployeePassportSeries { get; set; }

        public int EmployeePassportNumber { get; set; }

        public bool IsActive { get; set; }
        
    }
}
