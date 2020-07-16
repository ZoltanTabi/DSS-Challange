using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RoDSStar.Model
{
    /// <summary>
    /// Fájl kezelését végző osztály
    /// </summary>
    public static class FileHandling
    {
        /// <summary>
        /// Fájl beolvasását és Order osztállyá alakítását végző függvény
        /// </summary>
        /// <param name="path">Fájl elérési útvonala</param>
        /// <returns>Order osztályt tartalmazó lista</returns>
        public static async Task<IEnumerable<Order>> ReadAsync(string path)
        {
            IList<Order> records = new List<Order>();
            var lines = (await File.ReadAllLinesAsync(path)).Skip(1);

            foreach(var line in lines)
            {
                var splitLine = line.Split(';');
                records.Add(new Order
                {
                    Id = splitLine[0],
                    Product = (ProductType)Enum.Parse(typeof(ProductType), splitLine[1]),
                    Count = int.Parse(splitLine[2].Replace(" ", String.Empty)),
                    Deadline = DeadlineConverter(splitLine[3]),
                    ProfitPerPrice = int.Parse(splitLine[4].Replace(" ", String.Empty)),
                    PenaltForDelayPerDay = int.Parse(splitLine[5].Replace(" ", String.Empty).Trim()),
                });
            }

            return records;
        }

        private static DateTime DeadlineConverter(string date)
        {
            var splitDate = date.Split(".:".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (splitDate.Length != 4) throw new ApplicationException("Helytelen határidő forma!");

            var success = int.TryParse(splitDate[0], out int month);
            success = int.TryParse(splitDate[1], out int day) && success;
            success = int.TryParse(splitDate[2], out int hour) && success;
            success = int.TryParse(splitDate[3].Trim(), out int minute) && success;

            if (!success) throw new ApplicationException("Helytelen határidő forma!");

            DateTime deadline = new DateTime(DateTime.Now.Year, month, day, hour, minute, 0);

            if (deadline < DateTime.Now) deadline.AddYears(1);

            return deadline;
        }
    }
}
