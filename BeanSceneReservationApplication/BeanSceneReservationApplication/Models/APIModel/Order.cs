using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace BeanSceneReservationApplication.Models.APIModel
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

            public string Table { get; set; }
            public string TimeStamp { get; set; }
            public decimal TotalPrice { get; set; }

        public List<Item> Items { get; set; } = new List<Item>();
        public class Item
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public string Size { get; set; }
            public bool Vegan { get; set; }
        }

    }
}
