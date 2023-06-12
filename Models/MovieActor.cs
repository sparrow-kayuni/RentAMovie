using System;
using System.Collections.Generic;

namespace RentAMovie_v3.Models;

public partial class MovieActor
{
    public long? MovieId { get; set; }

    public long? ActorId { get; set; }

    public virtual Actor? Actor { get; set; }

    public virtual Movie? Movie { get; set; }
}
