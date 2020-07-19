using RoDSStar.Logic.Enums;
using System;

namespace RoDSStar.Logic.Models
{
    /// <summary>
    /// Class of the product
    /// </summary>
    public class Product
    {
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// The time when a product is done on a work station
        /// </summary>
        public DateTime? DoneTime { get; set; }
        /// <summary>
        /// Type
        /// </summary>
        public ProductType Type { get; set; }
    
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type</param>
        public Product (ProductType type)
        {
            Type = type;
        }
    }
}
