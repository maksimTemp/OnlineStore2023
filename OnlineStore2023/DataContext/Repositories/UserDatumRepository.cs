using Microsoft.EntityFrameworkCore;
using OnlineStore2023.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore2023.DataContext.Repositories
{
    internal class UserDatumRepository : OnlineShop2022Context
    {
        public UsersDatum GetUserDatumById(Guid id)
        {
            return UsersData.FirstOrDefault(x => x.UserId == id);
        }
    }
}
