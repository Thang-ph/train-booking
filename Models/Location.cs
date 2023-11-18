using System;
using System.Collections.Generic;

namespace PRN211_Project_Group_4.Models
{
    public partial class Location
    {
        public Location()
        {
            RouteTrainEnds = new HashSet<RouteTrain>();
            RouteTrainStarts = new HashSet<RouteTrain>();
        }

        public int LocationId { get; set; }
        public string? Location1 { get; set; }

        public virtual ICollection<RouteTrain> RouteTrainEnds { get; set; }
        public virtual ICollection<RouteTrain> RouteTrainStarts { get; set; }
    }
}
