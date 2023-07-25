namespace BeanSceneReservationApplication.Data
{
    public class Reservation
    {
        public int Id { get; set; }
        public Person Person { get; set; }
        public ReservationSource Source { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public int Guests { get; set; }
        public string? Notes { get; set; }
        //Following two fields will only be given a value if the name/phone given for the booking is
        //different to the name/phone given by a guest that has an existing Person object with the
        //same email used for the reservation
        public string? PersonName { get; set; }
        public string? PersonPhone { get; set; }

        public ICollection<RestaurantTable>? AssignedTables { get; set; }
    }
}
