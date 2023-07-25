namespace BeanSceneReservationApplication.Models.ReservationListModel
{
    public class DataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Duration { get; set; }
        public int Guests { get; set; }
        public string Start { get; set; }
        public string Source { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; }
        public string[] Tables { get; set; }

    }
}
