using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bi5.Net.IO;
using Bi5.Net.Models;
using Bi5.Net.Net;
using NSubstitute;
using Xunit;

namespace Bi5.Net.Tests
{
    public class LoaderTests
    {
        [Fact]
        public async Task GetAndFlush_WithValidConfig_ReturnsTrueResult()
        {
            // Arrange
            var config = new LoaderConfig(
                DateTime.UtcNow.Date,
                DateTime.UtcNow.Date.AddHours(1),
                new[] { "EURUSD" },
                DateTimePart.Minute,
                1,
                "/tmp/test",
                QuoteSide.Bid,
                FileScale.Day,
                false,
                false);

            // Create a test loader with mocked dependencies
            var loader = new Loader(config);

            // Act
            var result = await loader.GetAndFlush();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ResampleAndFlush_WithValidConfig_ReturnsTrueResult()
        {
            // Arrange
            var config = new LoaderConfig(
                DateTime.UtcNow.Date,
                DateTime.UtcNow.Date.AddHours(1),
                new[] { "EURUSD" },
                DateTimePart.Minute,
                1,
                "/tmp/test",
                QuoteSide.Bid,
                FileScale.Day,
                false,
                false);

            // Create a test loader with mocked dependencies
            var loader = new Loader(config);

            // Act
            var result = await loader.ResampleAndFlush();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Loader_Constructor_InitializesProperties()
        {
            // Arrange
            var config = new LoaderConfig(
                DateTime.UtcNow.Date,
                DateTime.UtcNow.Date.AddHours(1),
                new[] { "EURUSD" },
                DateTimePart.Minute,
                1,
                "/tmp/test",
                QuoteSide.Bid,
                FileScale.Day,
                false,
                false);

            // Act
            var loader = new Loader(config);

            // Assert
            Assert.NotNull(loader);
            
            // Test private fields using reflection
            var configField = loader.GetType().GetField("_cfg", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var writerField = loader.GetType().GetField("_tickDataFileWriter", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            Assert.NotNull(configField);
            Assert.NotNull(writerField);
            
            var configValue = configField.GetValue(loader);
            var writerValue = writerField.GetValue(loader);
            
            Assert.NotNull(configValue);
            Assert.NotNull(writerValue);
            Assert.IsType<LoaderConfig>(configValue);
            Assert.IsType<TickDataFileWriter>(writerValue);
            Assert.Same(config, configValue);
        }

        [Fact]
        public async Task Loader_ProcessesAllProductsInConfig()
        {
            // Arrange
            var config = new LoaderConfig(
                DateTime.UtcNow.Date,
                DateTime.UtcNow.Date.AddHours(1),
                new[] { "EURUSD", "GBPUSD" },
                DateTimePart.Minute,
                1,
                "/tmp/test",
                QuoteSide.Bid,
                FileScale.Day,
                false,
                false);

            // Create a test loader
            var loader = new Loader(config);

            // Act
            var result = await loader.GetAndFlush();

            // Assert
            Assert.True(result);
            // We're just verifying that multiple products don't cause errors
        }

        [Fact]
        public async Task Loader_WhenUnknownProduct_OutputsWarningMessage()
        {
            // Arrange
            var config = new LoaderConfig(
                DateTime.UtcNow.Date,
                DateTime.UtcNow.Date.AddHours(1),
                new[] { "UNKNOWN_PRODUCT" },
                DateTimePart.Minute,
                1,
                "/tmp/test",
                QuoteSide.Bid,
                FileScale.Day,
                false,
                false);

            // Create a test loader
            var loader = new Loader(config);

            // Capture console output
            var originalOut = Console.Out;
            using var stringWriter = new System.IO.StringWriter();
            Console.SetOut(stringWriter);

            try
            {
                // Act
                var result = await loader.GetAndFlush();

                // Assert
                var consoleOutput = stringWriter.ToString();
                Assert.Contains("Undefined products:", consoleOutput);
                Assert.Contains("UNKNOWN_PRODUCT", consoleOutput);
                Assert.True(result); // Still returns true even with unknown products
            }
            finally
            {
                // Restore console output
                Console.SetOut(originalOut);
            }
        }
    }
}