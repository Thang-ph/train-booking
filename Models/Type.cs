using System;
using System.Collections.Generic;

namespace PRN211_Project_Group_4.Models
{
    public partial class Type
    {
        public Type()
        {
            Trains = new HashSet<Train>();
        }

        public int TypeId { get; set; }
        public string? Type1 { get; set; }

        public virtual ICollection<Train> Trains { get; set; }
    }
}
