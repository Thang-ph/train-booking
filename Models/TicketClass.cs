using System;
using System.Collections.Generic;

namespace PRN211_Project_Group_4.Models
{
    public partial class TicketClass
    {
        public TicketClass()
        {
            Wagons = new HashSet<Wagon>();
        }

        public int ClassId { get; set; }
        public string? Class { get; set; }

        public virtual ICollection<Wagon> Wagons { get; set; }
    }
}
