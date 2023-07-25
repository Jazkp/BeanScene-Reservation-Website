using BeanSceneReservationApplication.Data;

namespace BeanSceneReservationApplication.Models.ReservationModel
{
    public class ReservationVM
    {
        public Reservation Reservation { get; set; }

        public Sitting Sitting { get; set; } 
    }
}
