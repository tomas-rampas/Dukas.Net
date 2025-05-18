using Bi5.Net.Models;
using Xunit;

namespace Bi5.Net.Tests.Models
{
    public class ProductTests
    {
        [Fact]
        public void Constructor_InitializesProperties()
        {
            // Arrange
            var name = "EURUSD";
            byte decimals = 5;
            var type = "forex";

            // Act
            var product = new Product(name, decimals, type);

            // Assert
            Assert.Equal(name, product.Name);
            Assert.Equal(decimals, product.Decimals);
            Assert.Equal(type, product.Type);
        }

        [Fact]
        public void Constructor_WithDifferentValues_InitializesPropertiesCorrectly()
        {
            // Arrange
            var name = "XAUUSD";
            byte decimals = 3;
            var type = "metals";

            // Act
            var product = new Product(name, decimals, type);

            // Assert
            Assert.Equal(name, product.Name);
            Assert.Equal(decimals, product.Decimals);
            Assert.Equal(type, product.Type);
        }

        [Fact]
        public void Properties_AreReadOnly()
        {
            // Verify that all properties are get-only (read-only)
            var nameProperty = typeof(Product).GetProperty("Name");
            var decimalsProperty = typeof(Product).GetProperty("Decimals");
            var typeProperty = typeof(Product).GetProperty("Type");

            Assert.False(nameProperty.CanWrite);
            Assert.False(decimalsProperty.CanWrite);
            Assert.False(typeProperty.CanWrite);
        }
    }
}