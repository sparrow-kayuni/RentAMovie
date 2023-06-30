using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAMovie_v3.Models;

public partial class Customer
{
    public long CustomerId { get; set; }

    [Display(Name = "First Name")]
    public string FName { get; set; } = null!;

    [Display(Name = "Middle Name")]
    public string? MName { get; set; }

    [Display(Name = "Last Name")]
    public string LName { get; set; } = null!;

    [Display(Name = "Email Address")]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone Number")]
    public long PhoneNo { get; set; }

    [Display(Name = "House Address")]
    public virtual Address? Address { get; set; }

    public virtual ICollection<RentalTransaction> RentalTransactions { get; set; } = new List<RentalTransaction>();
}
