using System;
using System.Collections.Generic;

namespace RentAMovie_v3.Models;

public partial class Customer
{
    public long CustomerId { get; set; }

    public string FName { get; set; } = null!;

    public string? MName { get; set; }

    public string LName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public long PhoneNo { get; set; }

    public long? AddressId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<RentalTransaction> RentalTransactions { get; set; } = new List<RentalTransaction>();
}
