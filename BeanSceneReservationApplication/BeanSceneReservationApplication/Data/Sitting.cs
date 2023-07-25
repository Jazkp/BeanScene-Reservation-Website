namespace BeanSceneReservationApplication.Data
{
    public class Sitting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SittingType? SittingType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
        public bool Active { get; set; }
        public List<Reservation>? Reservations { get; set; } = new List<Reservation>();

    }
}
