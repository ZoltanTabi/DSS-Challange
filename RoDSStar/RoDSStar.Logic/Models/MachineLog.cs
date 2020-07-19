using System;

namespace RoDSStar.Logic.Models
{
    /// <summary>
    /// The class of the logs for the machines
    /// </summary>
    public class MachineLog
    {
        /// <summary>
        /// The start of the order
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// The end of the order
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// The id of the order
        /// </summary>
        public string OrderId { get; set; }
    }
}
