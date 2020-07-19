﻿using RoDSStar.Logic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Machines = new List<Machine>();
            for (var i = 0; i < capacity; ++i)
            {
                Machines.Add(new Machine(name + "-" + (i + 1).ToString()));
            }
        }

        public void CutterPutProductMachines(List<Order> orders)
        {
            switch (orders.Count)
            {
                case 1:
                    var order = orders.First();
                    var workTime = OrderWorkTime(order.Products.First());
                    var queue = new Queue<Product>(order.Products);
                    var i = 0;
                    while (queue.Any())
                    {
                        Machines[i % Capacity].Process(queue.Dequeue(), workTime, order.Id);
                        ++i;
                    }
                    break;
                    //case 2:
                    //    break;
            }
        }

        public void PutProductsOnMachines(List<Order> orders)
        {
            switch (orders.Count)
            {
                case 1:
                    var order = orders.First();
                    var workTime = OrderWorkTime(order.Products.First());
                    var queue = new Queue<Product>(order.Products);
                    var i = 0;
                    while (queue.Any())
                    {
                        Machines[i % Capacity].Process(queue.Dequeue(), workTime, order.Id);
                        ++i;
                    }
                    break;
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