using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAMovie_v3.Models;

public partial class Movie
{
    public long MovieId { get; set; }

    public string Title { get; set; } = null!;

    [Display(Name = "Year of Release")]
    public long YearOfRelease { get; set; }

    [Display(Name = "Unit Price")]
    public long UnitPrice { get; set; }

    public virtual ICollection<RentalTransaction> RentalTransactions { get; set; } = new List<RentalTransaction>();
}
