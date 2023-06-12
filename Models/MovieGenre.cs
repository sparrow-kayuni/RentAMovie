using System;
using System.Collections.Generic;

namespace RentAMovie_v3.Models;

public partial class MovieGenre
{
    public long? MovieId { get; set; }

    public long? GenreId { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual Movie? Movie { get; set; }
}
