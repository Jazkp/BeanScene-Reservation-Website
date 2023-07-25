namespace BeanSceneReservationApplication.Data
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Area> Areas { get; set; } = new List<Area>();
    }
}
