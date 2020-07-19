using RoDSStar.Logic.Enums;
using System;

namespace RoDSStar.Logic.Models
{
    /// <summary>
    /// Termék leíró osztály
    /// </summary>
    public class Product
    {
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// Ekkor kerül át másik munkaállomásra
        /// </summary>
        public DateTime? DoneTime { get; set; }
        /// <summary>
        /// Termék típusa
        /// </summary>
        public ProductType Type { get; set; }
    
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Termék típusa</param>
        public Product (ProductType type)
        {
            Type = type;
        }
    }
}
