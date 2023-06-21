using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RentAMovie_v3.Models
{
    public class ItemList<T> : List<T>
    {
        public List<RentalTransaction> Transactions { get; set; } = new List<RentalTransaction>();
        public void SetItems(List<T> items)
        {
            this.AddRange(items);
        }
    }
}
