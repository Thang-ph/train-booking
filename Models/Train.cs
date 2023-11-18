namespace PRN211_Project_Group_4.Models
{
    public partial class Train
    {
        public Train()
        {
            Trips = new HashSet<Trip>();
            Wagons = new HashSet<Wagon>();
        }

        public int TrainId { get; set; }
        public string? Name { get; set; }
        public int? TypeId { get; set; }

        public virtual Type? Type { get; set; }
        public virtual ICollection<Trip> Trips { get; set; }
        public virtual ICollection<Wagon> Wagons { get; set; }
    }
}
