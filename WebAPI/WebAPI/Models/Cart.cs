using System;
using System.Collections.Generic;

namespace WebAPI.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public virtual User? User { get; set; }
}
