using BeanSceneReservationApplication.Data;

namespace BeanSceneReservationApplication.Models.CalendarModel
{
    public class EventViewModel
    {

        public string Title { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public bool Editable { get; set; }

        public string BackgroundColor { get; set; }

        public ExtendedProps ExtendedProps { get; set; }

    }

    public class ExtendedProps
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public bool Active { get; set; }
        public List<object> Reservations { get; set; }
    }
}
