namespace BeanSceneReservationApplication.Models.ReservationListModel
{
    public class AreaVM
    {
        public string Name { get; set; }
        public List<TableVM> TableVMs { get; set; }
    }

    public class TableVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Assigned { get; set; }
    }
}
