using RoDSStar.Logic.Enums;
using Serilog;
using System.Collections.Generic;
using System.Linq;

namespace RoDSStar.Logic.Models
{
    /// <summary>
    /// Class of the worstation
    /// </summary>
    public class WorkStation
    {
        /// <summary>
        ///List of the work machines
        /// </summary>
        public IList<Machine> Machines { get; set; }
        /// <summary>
        /// Name of Work Station
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Quantity of the machines
        /// </summary>
        public int Capacity { get; set; }
        /// <summary>
        /// Processing time of the child bycicle
        /// </summary>
        public int ChildrenBycicleTime { get; set; }
        /// <summary>
        /// Processing time of the adult bycicle
        /// </summary>
        public int AdultBycicleTime { get; set; }
        /// <summary>
        /// Processing time of the teenager bycicle
        /// </summary>
        public int TeenagerBycicleTime { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Names of the machines</param>
        /// <param name="capacity">Quantity of the machines</param>
        public WorkStation(string name, int capacity)
        {
            Capacity = capacity;
            Machines = new List<Machine>();
            for (var i = 0; i < capacity; ++i)
            {
                Machines.Add(new Machine(name + "-" + (i + 1).ToString()));
            }
        }

        /// <summary>
        /// Putting the products on the machines.
        /// </summary>
        /// <param name="order">Order</param>
        public void PutProductsOnMachines(Order order)
        {
            Log.Information($"{order.Id} azonosítójú megrendelés megkezdése a {Name} gépeknél");
            var workTime = OrderWorkTime(order.Products.First());
            var queue = new Queue<Product>(order.Products);
            var i = 0;
            while (queue.Any())
            {
                Machines[i % Capacity].Process(queue.Dequeue(), workTime, order.Id);
                ++i;
            }
            Log.Information($"{order.Id} azonosítójú megrendelés befejezése a {Name} gépeknél");
        }

        /// <summary>
        /// Logs the end time
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
