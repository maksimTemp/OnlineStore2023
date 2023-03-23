using System;
using System.Collections.Generic;

namespace OnlineStore2023.DataContext.Models;

public partial class UsersDatum
{
    public Guid UserId { get; set; }

    public string UserFirstName { get; set; }

    public string UserMiddleName { get; set; }

    public string UserLastName { get; set; }

    public string UserPhoneNumber { get; set; }

    public virtual User User { get; set; }
}
