using System;
using System.Collections.Generic;

namespace OnlineStore2023.DataContext.Models;

public partial class Order
{
    public Guid OrderId { get; set; }

    public Guid? EmployeeId { get; set; }

    public Guid? CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public string DeliveryAddress { get; set; }

    public string OrderStatus { get; set; }

    public decimal TotalPrice { get; set; }
    public bool? OrderPaid { get; set; }


    public virtual User Customer { get; set; }

    public virtual User Employee { get; set; }

    public virtual ICollection<ProductOrder> ProductOrders { get; } = new List<ProductOrder>();
}
