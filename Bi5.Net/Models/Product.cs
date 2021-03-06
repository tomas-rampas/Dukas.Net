namespace Bi5.Net.Models
{
    /// <summary>
    /// Stores required atts of the Dukascopy product
    /// </summary>
    public class Product
    {
        public Product(string name, byte decimals, string type)
        {
            Name = name;
            Decimals = decimals;
            Type = type;
        }
        public string Name { get; }
        public byte Decimals { get; }
        public string Type { get; }
    }
}