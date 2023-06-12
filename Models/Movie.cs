using System;
using System.Collections.Generic;

namespace RentAMovie_v3.Models;

public partial class Movie
{
    public long MovieId { get; set; }

    public string Title { get; set; } = null!;

    public long YearOfRelease { get; set; }

    public long UnitPrice { get; set; }

    public virtual ICollection<RentalTransaction> RentalTransactions { get; set; } = new List<RentalTransaction>();
}
