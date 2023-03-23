using System;
using System.Collections.Generic;

namespace OnlineStore2023.DataContext.Models;

public partial class ProductOrder
{
    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public int SelectedQuantity { get; set; }

    public decimal ProductsInOrderTotalPrice { get; set; }

    public virtual Order Order { get; set; }

    public virtual Product Product { get; set; }
}
