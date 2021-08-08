using MongoDB.Bson.Serialization.Attributes;

namespace CurrencyConversor.Domain.Models
{
    public class User
    {
        [BsonId]
        public string UserId { get; set; }
    }
}
