using System;
using System.Collections.Generic;

namespace PRN211_Project_Group_4.Models
{
    public partial class Trip
    {
        public Trip()
        {
            Bookings = new HashSet<Booking>();
        }

        public int TripId { get; set; }
        public int? Slot { get; set; }
        public int? TrainId { get; set; }
        public decimal? Price { get; set; }
        public DateTime? Date { get; set; }
        public int? RouteId { get; set; }
        public bool? Status { get; set; }

        public virtual RouteTrain? Route { get; set; }
        public virtual Train? Train { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
