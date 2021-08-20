using System;
using Bi5.Net.Utils;
using Xunit;

namespace Bi5.Net.Tests
{
    public class ArrayUtilsTest
    {
        [Fact]
        public void Checking_Wrong_Byte_Array_Test()
        {
            var bytes = new byte[] {0};
            Assert.Throws<ArgumentException>(()=> bytes.ToTickArray(DateTime.Now, 0));
        }
    }
}