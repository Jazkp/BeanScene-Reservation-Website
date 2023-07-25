namespace BeanSceneReservationApplication.Models.APIModel
{
    public class MongoDBSettings
    {
        public string ConnectionURI { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string Products { get; set; } = null!;
        public string Orders { get; set; } = null!;

    }
}
