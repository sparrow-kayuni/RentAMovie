using System;
using System.Collections.Generic;

namespace RentAMovie_v3.Models;

public partial class Role
{
    public long RoleId { get; set; }

    public string Title { get; set; } = null!;
}
