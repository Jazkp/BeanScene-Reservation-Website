using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BeanSceneReservationApplication.Models.APIModel
{
    public class OrderDataDB
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; } = null!;
        
        public decimal Price { get; set; }
        public int Quantity { get; set; } 
        public string Table { get; set; } = null!;
        public string TimeStamp { get; set; } = null!;
    }
}
