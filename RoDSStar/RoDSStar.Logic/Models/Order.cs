using System;
using System.Collections.Generic;

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
        /// Megrendelés priorítása
        /// </summary>
        /// <param name="totalProfit">A megrendelésekből álló összprofit</param>
        /// <returns>Megrendelés prioritása</returns>
        public double GetPriority(double totalProfit)
        {
            return 0;
        }
    }
}
