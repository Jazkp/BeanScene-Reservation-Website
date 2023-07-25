using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace BeanSceneReservationApplication.Models.APIModel
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; } = null!;

        [BsonElement("Sittings")]
        [JsonPropertyName("Sittings")]
        public List<string> Sittings { get; set; } = null!;

        public string Sizes { get; set; } = null!;
        public decimal Prices { get; set; }
        public bool Vegan { get; set; }
    }
}
