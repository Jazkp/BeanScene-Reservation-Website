using BeanSceneReservationApplication.Data;

namespace BeanSceneReservationApplication.Models.ReservationModel
{
    public class ReservationEventVM
    {
        public string Title { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool Editable { get; set; }

        public string BackgroundColor { get; set; }

        public ReservationExtendedProps ExtendedProps { get; set; }
    }
    public class ReservationExtendedProps
    {
        public int SittingId { get; set; }
        public int SeatsRemaining { get; set; }
    }
}
