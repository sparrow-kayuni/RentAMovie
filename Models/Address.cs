using System;
using System.Collections.Generic;

namespace RentAMovie_v3.Models;

public partial class Address
{
    public long AddressId { get; set; }

    public string HouseAddress { get; set; } = null!;

    public long ZipCode { get; set; }

    public string City { get; set; }

    public long CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
}
