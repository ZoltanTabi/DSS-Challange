using System;
using Xunit;
using RoDSStar.Logic.Helpers;
using System.IO;

namespace RoDSStar.Test
{
    public class UnitTest
    {
        [Fact]
        public async void Test()
        {
            var content = System.IO.File.ReadAllText(@"..\..\..\..\..\Samples\Sample1.csv");

            var result = await FileHandling.ReadAsync(@"..\..\..\..\..\Samples\Sample1.csv");
        }
    }
}
