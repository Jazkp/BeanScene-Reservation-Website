namespace BeanSceneReservationApplication.Models.ReservationListModel
{
    public class SittingDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public int Capacity { get; set; }
        public bool Active { get; set; }
    }
}
