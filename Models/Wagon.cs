using System;
using System.Collections.Generic;

namespace PRN211_Project_Group_4.Models
{
    public partial class Wagon
    {
        public int WagonId { get; set; }
        public string? Name { get; set; }
        public int? Row { get; set; }
        public int? Col { get; set; }
        public int? TrainId { get; set; }
        public int? ClassId { get; set; }

        public virtual TicketClass? Class { get; set; }
        public virtual Train? Train { get; set; }
    }
}
