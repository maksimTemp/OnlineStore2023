using System;
using System.Collections.Generic;

namespace OnlineStore2023.DataContext.Models;

public partial class Product
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal? ProductWeight { get; set; }

    public decimal? ProductVolume { get; set; }

    public string ProductColor { get; set; }

    public string ProductDescription { get; set; }

    public int? ProductQuantity { get; set; }

    public decimal ProductPrice { get; set; }

    public byte[] ProductPhoto { get; set; }

    public bool? IsArchive { get; set; }

    public virtual ICollection<ProductOrder> ProductOrders { get; } = new List<ProductOrder>();
}
