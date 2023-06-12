using System;
using System.Collections.Generic;

namespace RentAMovie_v3.Models;

public partial class Actor
{
    public long ActorId { get; set; }

    public string ActorFname { get; set; } = null!;

    public string ActorSname { get; set; } = null!;
}
