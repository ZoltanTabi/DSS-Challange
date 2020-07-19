using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoDSStar.Logic.Models
{
    /// <summary>
    /// Munka gépeket leíró osztály
    /// </summary>
    public class Machine
    {
        public readonly DateTime startDate = DateTime.Parse("2020.07.20. 6:00");
        /// <summary>
        /// Machine's name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A munkagép ekkortól lesz elérhető
        /// </summary>
        public DateTime? DoneTime { get; set; }
        /// <summary>
        /// A munkagép logja
        /// </summary>
        public IList<MachineLog> Logs { get; set; } = new List<MachineLog>();
        /// <summary>
        /// Aktuálisan csinált megrendelés
        /// </summary>
        public string CurrentOrderId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Machine's name</param>
        public Machine (string name)
        {
            Name = name;
        }

        public void Process(Product product, int workTime, string orderId)
        {
            var currentTime = (DoneTime == null && product.DoneTime == null) ? startDate
                            : (DoneTime == null) ? product.DoneTime.Value
                            : (product.DoneTime == null) ? DoneTime.Value
                            : DateTime.Compare(DoneTime.Value, product.DoneTime.Value) < 0 ? product.DoneTime.Value : DoneTime.Value;

            if (product.StartTime == null)
            {
                product.StartTime = currentTime;
            }

            if (CurrentOrderId == null)
            {
                CurrentOrderId = orderId;
                Logs.Add(new MachineLog
                {
                    OrderId = orderId,
                    StartTime = currentTime
                });
            }
            else if (CurrentOrderId != orderId)
            {
                var lastLog = Logs.Last();
                lastLog.EndTime = DoneTime.Value;
                CurrentOrderId = orderId;
                Logs.Add(new MachineLog
                {
                    OrderId = orderId,
                    StartTime = currentTime
                });
            }

            if (currentTime.AddMinutes(workTime).Hour >= 22)
            {
                currentTime = currentTime.AddDays(1);
                currentTime = DateTime.Parse($"{currentTime.Year}.{currentTime.Month}.{currentTime.Day}. 6:00");
            }
            currentTime = currentTime.AddMinutes(workTime);

            DoneTime = currentTime;
            product.DoneTime = currentTime;
        }
    }
}
