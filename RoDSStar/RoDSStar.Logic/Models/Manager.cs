using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoDSStar.Logic.Helpers;

namespace RoDSStar.Logic.Models
{
    /// <summary>
    /// Üzleti logikát megvalósító osztály
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Fájlkezelő
        /// </summary>
        private readonly FileHandling fileHandling;
        /// <summary>
        /// Munkaállomások paraméterei
        /// Gépek neve
        /// Gépek száma, Gyerek bicikli - , Felnőtt bicikli - , Serdülő bicikli munkaideje
        /// </summary>
        private readonly Dictionary<string, Tuple<int, int, int, int>> workStationsParameters = new Dictionary<string, Tuple<int, int, int, int>>
        {
            { "Vagó", new Tuple<int,int,int,int>(6, 5, 8, 6) },
            { "Hajlító", new Tuple<int,int,int,int>(2, 10, 16, 15)},
            { "Hegesztő", new Tuple<int,int,int,int>(3, 8, 12, 10) },
            { "Tesztelő", new Tuple<int,int,int,int>(1, 5, 5, 5) },
            { "Festő", new Tuple<int,int,int,int>(4, 12, 20, 15) },
            { "Csomagoló", new Tuple<int,int,int,int>(3, 10, 15, 12) }
        };

        /// <summary>
        /// Munkaállomások
        /// </summary>
        public IList<WorkStation> WorkStations { get; set; }
        /// <summary>
        /// Megrendelések
        /// </summary>
        public IList<Order> Orders { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileHandling">Fájl kezelő osztály</param>
        public Manager(FileHandling fileHandling)
        {
            this.fileHandling = fileHandling;
            WorkStations = new List<WorkStation>();
            foreach(var parameters in workStationsParameters)
            {
                WorkStations.Add(new WorkStation(parameters.Key, parameters.Value.Item1)
                {
                    ChildrenBycicleTime = parameters.Value.Item2,
                    AdultBycicleTime = parameters.Value.Item3,
                    TeenagerBycicleTime = parameters.Value.Item4,
                });
            }
        }

        /// <summary>
        /// Fájl beolvasása
        /// </summary>
        /// <returns></returns>
        public async Task ReadAsync()
        {
            Orders = await fileHandling.ReadAsync();
        }

        /// <summary>
        /// Fő folyamat
        /// </summary>
        /// <returns>A fájlok útvonalával</returns>
        public async Task<string> Simulation()
        {
            foreach(var order in Orders)
            {
                order.SetPriority(Orders.Sum(x => x.Count * x.ProfitPerPrice));
            }

            Orders = Orders.OrderByDescending(x => x.Priority).ToList();
            
            foreach(var workStation in WorkStations)
            {
                foreach(var order in Orders)
                {
                    workStation.PutProductsOnMachines(order);
                }
            }

            await fileHandling.WriteAsync(Orders, WorkStations);

            return "";
        }
    }
}
