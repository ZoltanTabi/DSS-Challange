using System;
using Xunit;
using RoDSStar.Model;
using System.IO;

namespace Test
{
    public class UnitTest
    {
        [Fact]
        public void Test()
        {
            var fileHandling = new FileHandling();

            var content = System.IO.File.ReadAllText(@"C:\Users\T�bi Zolt�n\source\repos\DSS-Challange\Samples\Sample1.csv");

            var result = fileHandling.Read(@"C:\Users\T�bi Zolt�n\source\repos\DSS-Challange\Samples\Sample1.csv");
        }
    }
}
