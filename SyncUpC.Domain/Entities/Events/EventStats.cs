namespace SyncUpC.Domain.Entities.Events
{
    public class EventStats
    {
        public EventStats(int registered, int attendees, int maxCapacity)
        {
            Registered = registered;
            Attendees = attendees;
            MaxCapacity = maxCapacity;
        }

        public int Registered { get; set; }
        public int Attendees { get; set; }
        public int MaxCapacity { get; set; } // Necesario para cálculo

    }
}
