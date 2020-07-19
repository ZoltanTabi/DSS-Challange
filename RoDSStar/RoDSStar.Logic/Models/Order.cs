using RoDSStar.Logic.Enums;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoDSStar.Logic.Models
{
    /// <summary>
    /// Megrendelések osztálya
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Azonosító
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Megrendelt termék típusa
        /// </summary>
        public IList<Product> Products { get; set; }
        /// <summary>
        /// Megrendelés darabszáma
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Megrendelés határideje
        /// </summary>
        public DateTime Deadline { get; set; }
        /// <summary>
        /// Termékenkénti profit
        /// </summary>
        public int ProfitPerPrice { get; set; }
        /// <summary>
        /// Késés esetén fizetendő büntetés naponként
        /// </summary>
        public int PenaltForDelayPerDay { get; set; }
        /// <summary>
        /// Prioritás
        /// </summary>
        public double Priority { get; set; }

        /// <summary>
        /// Beállítja a Prioritást
        /// </summary>
        /// <param name="totalProfit"></param>
        public void SetPriority(double totalProfit)
        {
            double result = 0;

            switch(Products.First().Type)
            {
                case ProductType.ChildrenBicycle:
                    result = 50;
                    break;
                case ProductType.AdultBicycle:
                    result = 76;
                    break;
                case ProductType.TeenagerBicycle:
                    result = 63;
                    break;
            }

            result *= Count;
            result /= 960;
            DateTime date = DateTime.Parse("2020.07.20. 6:00");
            date = date.AddDays(result);
            result = (date - Deadline).TotalDays;
            result = result <= 1 ? 1 : result;
            result *= PenaltForDelayPerDay;
            result /= totalProfit;
            result *= 100;

            Log.Information($"{Id} {result}");

            Priority = result;
        }
    }
}
