using BeanSceneReservationApplication.Data;

namespace BeanSceneReservationApplication.Models.CalendarModel
{
    public class CreateSittingsVM
    {
        public string Name { get; set; }
        public string TypeName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Capacity { get; set; }
        public bool Active { get; set; }
        public DateTime BatchStartDate { get; set; }
        public DateTime BatchEndDate { get; set; }
        public string[] DaysOfWeek { get; set; }
    }
}
