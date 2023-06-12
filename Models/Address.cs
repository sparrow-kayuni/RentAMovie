using System;
using System.Collections.Generic;

namespace RentAMovie_v3.Models;

public partial class Address
{
    public string HouseAddress { get; set; } = null!;

    public long AddressId { get; set; }

    public long ZipCode { get; set; }

    public virtual Customer? Customer { get; set; }
}
