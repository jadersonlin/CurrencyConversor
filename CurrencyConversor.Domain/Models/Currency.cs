namespace CurrencyConversor.Domain.Models
{
    public  class Currency
    {
        public Currency(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public string Code { get; }

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
