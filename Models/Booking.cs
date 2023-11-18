using System;
using System.Collections.Generic;

namespace PRN211_Project_Group_4.Models
{
    public partial class Booking
    {
        public int BookId { get; set; }
        public int? TripId { get; set; }
        public string? SeatStatus { get; set; }
        public double? Amount { get; set; }
        public int? AccountId { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Trip? Trip { get; set; }
    }
}
