using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ServerAuth.Models
{
    public class Watch
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement]
        public string Brand { get; set; }

        [BsonElement]
        public int Price { get; set; }
    }
    public class WatchDTO
    {
        
        public string Id { get; set; }

        [BsonElement]
        public string Brand { get; set; }

        [BsonElement]
        public int Price { get; set; }
    }
}
