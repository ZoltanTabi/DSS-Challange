﻿using RoDSStar.Logic.Enums;
using Serilog;
using System.Collections.Generic;
using System.Linq;

namespace RoDSStar.Logic.Models
{
    /// <summary>
    /// Munkaállomásokat leíró osztályszerkezet
    /// </summary>
    public class WorkStation
    {
        /// <summary>
        /// Munka gépek listája
        /// </summary>
        public IList<Machine> Machines { get; set; }
        /// <summary>
        /// Work station's name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Munka gépek száma
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// Gyerek bicikli munka ideje
        /// </summary>
        public int ChildrenBycicleTime { get; set; }
        /// <summary>
        /// Felnőtt bicikli munka ideje
        /// </summary>
        public int AdultBycicleTime { get; set; }
        /// <summary>
        /// Serdülő bicikli munka ideje
        /// </summary>
        public int TeenagerBycicleTime { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Munka gépek nevei</param>
        /// <param name="capacity">Munka gépek száma</param>
        public WorkStation(string name, int capacity)
        {
            Capacity = capacity;
            Name = name;
            Machines = new List<Machine>();
            for (var i = 0; i < capacity; ++i)
            {
                Machines.Add(new Machine(name + "-" + (i + 1).ToString()));
            }
        }

        /// <summary>
        /// Munka szalagra rakása a terméknek
        /// </summary>
        /// <param name="order">Megrendelés</param>
        public void PutProductsOnMachines(Order order)
        {
            Log.Information($"{order.Id} számú megrendelés feldolgozása a {Name} gépeknél.");
            var workTime = OrderWorkTime(order.Products.First());
            var queue = new Queue<Product>(order.Products);
            var i = 0;
            while (queue.Any())
            {
                Machines[i % Capacity].Process(queue.Dequeue(), workTime, order.Id);
                ++i;
            }
            Log.Information($"{order.Id} számú megrendelés befejezése a {Name} gépeknél.");
        }

        /// <summary>
        /// The work is end
        /// </summary>
        public void EndWork()
        {
            foreach(var machine in Machines)
            {
                var lastLog = machine.Logs.Last();
                lastLog.EndTime = machine.DoneTime.Value;
            }
        }

        private int OrderWorkTime(Product product)
        {
            return product.Type switch
            {
                ProductType.ChildrenBicycle => ChildrenBycicleTime,
                ProductType.AdultBicycle => AdultBycicleTime,
                ProductType.TeenagerBicycle => TeenagerBycicleTime,
                _ => 0,
            };
        }
    }
}
