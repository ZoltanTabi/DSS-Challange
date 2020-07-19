using System;
using System.IO;
using System.Threading.Tasks;
using RoDSStar.Logic.Helpers;
using RoDSStar.Logic.Models;
using Serilog;
using Serilog.Events;

namespace RoDSStar.ConsoleApp
{
    class Program
    {
        static async Task Main()
        {
            string filesPath = @"..\..\";//@"C:\Program Files\RoDSStar\";
            string postFix = $"{DateTime.Now:H_mm_ss}_{Guid.NewGuid().ToString().Substring(0, 8)}";
            // Logger class configuration
            // More information: https://github.com/serilog/serilog-aspnetcore
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(
                    $@"{filesPath}Logs\log_{postFix}.txt",
                    fileSizeLimitBytes: 1_000_000,
                    rollOnFileSizeLimit: true,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1),
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message:lj} {NewLine}")
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
                var manager = new Manager(new FileHandling(path, filesPath, postFix));
                await manager.ReadAsync();

                await manager.Simulation();
                Log.Information("Munka vége!");
                Console.WriteLine("A munka befejeződött!");
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
                Log.Warning(ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Váratlan hiba!");
                Log.Error(ex.ToString());
            }
        }
    }
}
