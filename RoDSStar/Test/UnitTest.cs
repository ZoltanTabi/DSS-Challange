using System;
using Xunit;
using RoDSStar.Model;
using System.IO;

namespace Test
{
    public class UnitTest
    {
        [Fact]
        public async void Test()
        {
            var fileHandling = new FileHandling();

            var content = System.IO.File.ReadAllText(@"..\..\..\..\..\Samples\Sample1.csv");

            var result = await fileHandling.ReadAsync(@"..\..\..\..\..\Samples\Sample1.csv");
        }
    }
}
