using MongoDB.Bson.Serialization.Attributes;

namespace CurrencyConversor.Domain.Models
{
    public  class Currency
    {
        public Currency(string code, string name)
        {
            Code = code;
            Name = name;
        }

        [BsonId]
        public string Code { get; }

        [BsonElement("Name")]
        public string Name { get; }

        public override bool Equals(object obj)
        {
            return obj is Currency currency && currency.Code == Code;
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        public override string ToString()
        {
            return Code + " - " + Name;
        }
    }
}
