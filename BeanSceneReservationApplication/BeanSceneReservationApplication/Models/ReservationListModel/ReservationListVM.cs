using BeanSceneReservationApplication.Data;

namespace BeanSceneReservationApplication.Models.ReservationListModel
{
    public class ReservationListVM
    {
        public Sitting Sitting { get; set; }
        public List<ReservationStatus> Statuses { get; set; }
        public List<ReservationSource> Sources { get; set; }
    }
}
