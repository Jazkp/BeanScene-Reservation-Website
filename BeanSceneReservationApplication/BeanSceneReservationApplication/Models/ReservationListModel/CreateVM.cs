namespace BeanSceneReservationApplication.Models.ReservationListModel
{
    public class CreateVM
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Duration { get; set; }
        public int Guests { get; set; }
        public DateTime StartTime { get; set; }
        public string SourceName { get; set; }
        public string? Notes { get; set; }
    }
}
