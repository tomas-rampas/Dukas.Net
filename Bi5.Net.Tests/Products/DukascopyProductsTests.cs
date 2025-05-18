using System.Linq;
using Bi5.Net.Models;
using Bi5.Net.Products;
using Xunit;
using static Bi5.Net.Products.Names;

namespace Bi5.Net.Tests.Products
{
    public class DukascopyProductsTests
    {
        [Fact]
        public void Catalogue_IsInitialized()
        {
            // Act
            var catalogue = DukascopyProducts.Catalogue;

            // Assert
            Assert.NotNull(catalogue);
            Assert.NotEmpty(catalogue);
        }

        [Fact]
        public void Catalogue_ContainsExpectedProducts()
        {
            // Arrange
            var expectedProducts = new[]
            {
                EURUSD, GBPUSD, USDJPY, AUDUSD, USDCHF, XAUUSD, BTCUSD
            };

            // Act
            var catalogue = DukascopyProducts.Catalogue;

            // Assert
            foreach (var productName in expectedProducts)
            {
                Assert.Contains(productName, catalogue.Keys);
                Assert.NotNull(catalogue[productName]);
            }
        }

        [Fact]
        public void FXProducts_HaveCorrectDecimals()
        {
            // Arrange
            var catalogue = DukascopyProducts.Catalogue;

            // Act & Assert
            // FX products with JPY have 3 decimals
            Assert.Equal(3, catalogue[USDJPY].Decimals);
            Assert.Equal(3, catalogue[EURJPY].Decimals);
            Assert.Equal(3, catalogue[GBPJPY].Decimals);

            // Other FX products have 5 decimals
            Assert.Equal(5, catalogue[EURUSD].Decimals);
            Assert.Equal(5, catalogue[GBPUSD].Decimals);
            Assert.Equal(5, catalogue[USDCHF].Decimals);
        }

        [Fact]
        public void MetalsProducts_HaveCorrectDecimals()
        {
            // Arrange
            var catalogue = DukascopyProducts.Catalogue;

            // Act & Assert
            Assert.Equal(3, catalogue[XAUUSD].Decimals);
            Assert.Equal(3, catalogue[XAGUSD].Decimals);
        }

        [Fact]
        public void CryptoProducts_HaveCorrectDecimals()
        {
            // Arrange
            var catalogue = DukascopyProducts.Catalogue;

            // Act & Assert
            Assert.Equal(5, catalogue[BTCUSD].Decimals);
            Assert.Equal(5, catalogue[ETHUSD].Decimals);
            Assert.Equal(5, catalogue[LTCUSD].Decimals);
        }

        [Fact]
        public void Products_HaveCorrectTypes()
        {
            // Arrange
            var catalogue = DukascopyProducts.Catalogue;
            
            // Act & Assert
            // FX products
            Assert.Equal("FX", catalogue[EURUSD].Type);
            Assert.Equal("FX", catalogue[GBPUSD].Type);
            Assert.Equal("FX", catalogue[USDJPY].Type);
            
            // CFD products
            Assert.Equal("CFD", catalogue[XAUUSD].Type);
            Assert.Equal("CFD", catalogue[BTCUSD].Type);
            Assert.Equal("CFD", catalogue[BRENTCMDUSD].Type);
            Assert.Equal("CFD", catalogue[DEUIDXEUR].Type);
        }

        [Fact]
        public void Catalogue_IsReadOnly()
        {
            // Arrange
            var catalogue = DukascopyProducts.Catalogue;
            
            // Act & Assert
            // We can't directly try to add to the dictionary, but we can check
            // that the class inherits from ReadOnlyDictionary
            Assert.Contains("ReadOnlyDictionary", catalogue.GetType().BaseType.Name);
        }

        [Fact]
        public void ProductGroups_HaveExpectedCounts()
        {
            // Arrange
            var catalogue = DukascopyProducts.Catalogue;
            
            // Act
            var fxProducts = catalogue.Values.Where(p => p.Type == "FX").ToList();
            var cfdProducts = catalogue.Values.Where(p => p.Type == "CFD").ToList();
            
            // Assert
            // Check that we have correct counts for each product type
            Assert.Equal(17, fxProducts.Count); // 17 FX pairs
            Assert.Equal(18, cfdProducts.Count); // 18 CFD products
        }
    }
}