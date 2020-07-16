using System;
using System.IO;
using System.Threading.Tasks;
using RoDSStar.Logic.Helpers;
using Serilog;
using Serilog.Events;

namespace RoDSStar.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(
                    $@"..\..\Logs\log_{Guid.NewGuid().ToString().Substring(0,8)}.txt",
                    fileSizeLimitBytes: 1_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .CreateLogger();

            Console.WriteLine("Kérem adja meg a beolvasandó fájl útvonalát!");
            string path = Console.ReadLine();

            while (!File.Exists(path))
            {
                Console.WriteLine("A fájl nem létezik. Kérem adja meg újra!");
                path = Console.ReadLine();
            }

            try
            {
                Log.Information($"A beolvasás elkezdődik! Fájl: {path}");
                var orders = await FileHandling.ReadAsync(path);
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
                Log.Fatal(ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Váratlan hiba!");
                Log.Fatal(ex.ToString());
            }
        }
    }
}
