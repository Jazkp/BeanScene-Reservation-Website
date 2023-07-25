namespace BeanSceneReservationApplication.Data
{
    public class RestaurantTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Reservation> Reservations { get; set;}
    }
}
