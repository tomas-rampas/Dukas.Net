using System;
using System.Linq;
using Bi5.Net.Models;
using Xunit;

namespace Bi5.Net.Tests.Models
{
    public class LoaderConfigTests
    {
        [Fact]
        public void Constructor_InitializesProperties()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1, 0, 0, 0);
            var endDate = new DateTime(2023, 1, 2, 0, 0, 0);
            var products = new[] { "EURUSD", "GBPUSD" };
            var timeFrameMajorScale = DateTimePart.Hour;
            uint timeFrameMinorScale = 4;
            var outputFolder = "/tmp/test";
            var quoteSide = QuoteSide.Ask;
            var fileScale = FileScale.Day;
            var writeHeader = true;
            var gzipResult = true;
            byte threads = 8;

            // Act
            var config = new LoaderConfig(
                startDate,
                endDate,
                products,
                timeFrameMajorScale,
                timeFrameMinorScale,
                outputFolder,
                quoteSide,
                fileScale,
                writeHeader,
                gzipResult,
                threads);

            // Assert
            Assert.Equal(startDate, config.StartDate);
            Assert.Equal(new DateTime(2023, 1, 2, 23, 59, 59), config.EndDate); // End date should be adjusted to end of day
            Assert.Equal(products, config.Products);
            Assert.Equal(timeFrameMajorScale, config.TimeFrameMajorScale);
            Assert.Equal(timeFrameMinorScale, config.TimeFrameMinorScale);
            Assert.Equal(outputFolder, config.OutputFolder);
            Assert.Equal(quoteSide, config.QuoteSide);
            Assert.Equal(fileScale, config.FileScale);
            Assert.Equal(writeHeader, config.WriteHeader);
            Assert.Equal(gzipResult, config.GzipResult);
            Assert.Equal(threads, config.Threads);
            Assert.False(config.UseMarketDate); // Default value
        }

        [Fact]
        public void Constructor_WithNonMidnightEndDate_DoesNotAdjustEndDate()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1, 10, 0, 0);
            var endDate = new DateTime(2023, 1, 2, 15, 30, 45); // Non-midnight time

            // Act
            var config = new LoaderConfig(
                startDate,
                endDate,
                new[] { "EURUSD" },
                DateTimePart.Minute,
                1,
                "/tmp",
                QuoteSide.Bid,
                FileScale.Day,
                false,
                false);

            // Assert
            Assert.Equal(endDate, config.EndDate); // End date should not be adjusted
        }

        [Fact]
        public void Constructor_WithEmptyProducts_AllowsEmptyEnumerable()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 2);
            var products = Array.Empty<string>();

            // Act
            var config = new LoaderConfig(
                startDate,
                endDate,
                products,
                DateTimePart.Minute,
                1,
                "/tmp",
                QuoteSide.Bid,
                FileScale.Day,
                false,
                false);

            // Assert
            Assert.NotNull(config.Products);
            Assert.Empty(config.Products);
        }

        [Fact]
        public void Constructor_WithDefaultThreads_SetsDefaultThreadCount()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 2);

            // Act
            var config = new LoaderConfig(
                startDate,
                endDate,
                new[] { "EURUSD" },
                DateTimePart.Minute,
                1,
                "/tmp",
                QuoteSide.Bid,
                FileScale.Day,
                false,
                false);

            // Assert
            Assert.Equal((byte)4, config.Threads); // Default thread count is 4
        }

        [Fact]
        public void Constructor_WithExplicitThreads_SetsThreadCount()
        {
            // Arrange
            var startDate = new DateTime(2023, 1, 1);
            var endDate = new DateTime(2023, 1, 2);
            byte threads = 16;

            // Act
            var config = new LoaderConfig(
                startDate,
                endDate,
                new[] { "EURUSD" },
                DateTimePart.Minute,
                1,
                "/tmp",
                QuoteSide.Bid,
                FileScale.Day,
                false,
                false,
                threads);

            // Assert
            Assert.Equal(threads, config.Threads);
        }
    }
}