using BeanSceneReservationApplication.Data;

namespace BeanSceneReservationApplication.Models.ReservationModel
{
    public class EditReservationVM
    {
        public RestaurantTable[] Tables { get; set; }
        public Reservation Reservation { get; set; }
        public ReservationTableInfo[] TableInfos { get; set; }
        public List<int> ReservedTableIds { get; set; }
    }

    public class ReservationTableInfo
    {
        public int TableId { get; set; }
        public bool Selected { get; set; }
    }
}
