using System;
using System.Collections.Generic;

namespace RentAMovie_v3.Models;

public partial class Staff
{
    public long StaffId { get; set; }

    public string StaffUserName { get; set; } = null!;

    public string StaffPassword { get; set; } = null!;

    public long? StaffRole { get; set; }

    public virtual LoginSession? LoginSession { get; set; }
}
