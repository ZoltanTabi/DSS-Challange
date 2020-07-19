using System;

namespace RoDSStar.Logic.Models
{
    /// <summary>
    /// Munkagép logjait leíró osztály
    /// </summary>
    public class MachineLog
    {
        /// <summary>
        /// Megrendelés megkezdése
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// Megrendelés vége
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// Megrendelés azonosítója
        /// </summary>
        public string OrderId { get; set; }
    }
}
