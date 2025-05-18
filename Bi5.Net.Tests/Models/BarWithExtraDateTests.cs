using System;
using Bi5.Net.Models;
using Xunit;

namespace Bi5.Net.Tests.Models
{
    public class BarWithExtraDateTests
    {
        [Fact]
        public void Properties_CanBeSet()
        {
            // Arrange
            var bar = new Bar
            {
                Timestamp = new DateTime(2023, 1, 15, 10, 0, 0),
                Open = 1.1234,
                High = 1.1245,
                Low = 1.1230,
                Close = 1.1240,
                Volume = 100.5,
                Ticks = 50
            };
            var dateNoTime = new DateTime(2023, 1, 15, 0, 0, 0); // Just the date part

            // Act
            var barWithExtraDate = new BarWithExtraDate
            {
                Bar = bar,
                BarDateNoTime = dateNoTime
            };

            // Assert
            Assert.Equal(bar, barWithExtraDate.Bar);
            Assert.Equal(dateNoTime, barWithExtraDate.BarDateNoTime);
        }

        [Fact]
        public void Constructor_DefaultInitialization_HasDefaultValues()
        {
            // Act
            var barWithExtraDate = new BarWithExtraDate();

            // Assert
            Assert.Null(barWithExtraDate.Bar);
            Assert.Equal(default, barWithExtraDate.BarDateNoTime);
        }

        [Fact]
        public void Properties_AreInitOnly()
        {
            // Verify that all properties have init setters
            var barProperty = typeof(BarWithExtraDate).GetProperty("Bar");
            var dateProperty = typeof(BarWithExtraDate).GetProperty("BarDateNoTime");

            // Check if they have setters (init or set)
            Assert.True(barProperty.CanWrite);
            Assert.True(dateProperty.CanWrite);

            // Check if the setters are init-only by checking the backing field
            var barBackingField = typeof(BarWithExtraDate).GetField("<Bar>k__BackingField", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var dateBackingField = typeof(BarWithExtraDate).GetField("<BarDateNoTime>k__BackingField", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            Assert.NotNull(barBackingField);
            Assert.NotNull(dateBackingField);
        }
    }
}
