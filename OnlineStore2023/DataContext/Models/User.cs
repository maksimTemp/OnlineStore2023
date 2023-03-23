using System;
using System.Collections.Generic;

namespace OnlineStore2023.DataContext.Models;

public partial class User
{
    public User()
    {

    }
    public User(string userLogin,
        string userPasswordHash,
        string userPasswordSalt,
        string userType,
        bool? isActive,
        UsersDatum usersDatum,
        Customer customer)
    {
        UserLogin = userLogin;
        UserPasswordHash = userPasswordHash;
        UserPasswordSalt = userPasswordSalt;
        UserType = userType;
        IsActive = isActive;
        UsersDatum = usersDatum;
        Customer = customer;
    }
    public User(string userLogin,
        string userPasswordHash,
        string userPasswordSalt,
        string userType,
        bool? isActive,
        UsersDatum usersDatum,
        Employee employee)
    {
        UserLogin = userLogin;
        UserPasswordHash = userPasswordHash;
        UserPasswordSalt = userPasswordSalt;
        UserType = userType;
        IsActive = isActive;
        UsersDatum = usersDatum;
        Employee = employee;
    }

    public Guid UserId { get; set; }

    public string UserLogin { get; set; }

    public string UserPasswordHash { get; set; }

    public string UserPasswordSalt { get; set; }

    public string UserType { get; set; }

    public bool? IsActive { get; set; }

    public virtual Customer Customer { get; set; }

    public virtual Employee Employee { get; set; }

    public virtual ICollection<Order> OrderCustomers { get; } = new List<Order>();

    public virtual ICollection<Order> OrderEmployees { get; } = new List<Order>();

    public virtual UsersDatum UsersDatum { get; set; }
}
