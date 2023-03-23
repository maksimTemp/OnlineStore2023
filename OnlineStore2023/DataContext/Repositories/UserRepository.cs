using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OnlineStore2023.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineStore2023.DataContext.Repositories
{
    class UserRepository : OnlineShop2022Context
    {
        public UserRepository() : base() { }

        public User GetSimpleUserById(Guid id)
        {
            return Users.FirstOrDefault(x => x.UserId == id);
        }
        public User GetUserWithUserDatumById(Guid id) 
        {
            return Users
                .Where(x => x.UserId == id)
                .Include(x => x.UsersDatum)
                .FirstOrDefault();
        }
        public User GetUserWithEmployeeById(Guid id)
        { 
            return Users.Where(x => x.UserId == id)
                        .Include(x => x.UsersDatum)
                        .Include(x => x.Employee)
                        .FirstOrDefault();
        }
        public void RemoveRangeByIds(List<Guid> ids)
        {
            List<User> usersForRemove = new();

            foreach(var id in ids)
                usersForRemove.Add(GetSimpleUserById(id));

            Users.RemoveRange(usersForRemove);
        }
    }
}
