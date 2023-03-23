using System;
using System.Collections.Generic;

namespace OnlineStore2023.DataContext.Models;

public partial class Customer
{
    public Guid UserId { get; set; }
    public string CustomerEmailAdress { get; set; }
    public string CustomerAdressLine { get; set; }

    public virtual User User { get; set; }
}
