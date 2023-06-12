using System;
using System.Collections.Generic;

namespace RentAMovie_v3.Models;

public partial class Genre
{
    public long GenreId { get; set; }

    public string GenreName { get; set; } = null!;
}
