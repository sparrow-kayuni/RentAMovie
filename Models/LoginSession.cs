using System;
using System.Collections.Generic;

namespace RentAMovie_v3.Models;

public partial class LoginSession
{
    public long SessionId { get; set; }

    public long TimeStarted { get; set; }

    public long? TimeEnded { get; set; }

    public long StaffId { get; set; }

    public virtual ICollection<RentalTransaction> RentalTransactions { get; set; } = new List<RentalTransaction>();

    public virtual Staff Staff { get; set; } = null!;
}
