using System;
using System.Collections.Generic;

namespace PRN211_Project_Group_4.Models
{
    public partial class RouteTrain
    {
        public RouteTrain()
        {
            Trips = new HashSet<Trip>();
        }

        public int RouteId { get; set; }
        public int? StartId { get; set; }
        public int? EndId { get; set; }
        public int? RouteType { get; set; }

        public virtual Location? End { get; set; }
        public virtual Location? Start { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
    }
}
