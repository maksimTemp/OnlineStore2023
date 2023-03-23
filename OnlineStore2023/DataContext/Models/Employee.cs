using System;
using System.Collections.Generic;

namespace OnlineStore2023.DataContext.Models;

public partial class Employee
{
    public Guid UserId { get; set; }

    public int EmployeePassportSeries { get; set; }

    public int EmployeePassportNumber { get; set; }

    public string JobTitle { get; set; }

    public virtual User User { get; set; }
}
