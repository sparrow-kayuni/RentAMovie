using System;
using System.Collections.Generic;

namespace RentAMovie_v3.Models;

public partial class RentalTransaction
{
    public long RentalId { get; set; }

    public DateTime RentalDay { get; set; }

    public DateTime ReturnDate { get; set; }

    public long? CustomerId { get; set; }

    public long? SessionId { get; set; }

    public long? MovieId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Movie? Movie { get; set; }

    public virtual LoginSession? Session { get; set; }
}
