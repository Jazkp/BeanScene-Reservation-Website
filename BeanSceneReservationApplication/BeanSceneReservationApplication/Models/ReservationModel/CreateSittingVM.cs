using BeanSceneReservationApplication.Data;

namespace BeanSceneReservationApplication.Models.ReservationModel
{
    public class CreateSittingVM
    {
        public Sitting Sitting { get; set; }
        public List<SittingType> SittingType { get; set; }
    }
}
