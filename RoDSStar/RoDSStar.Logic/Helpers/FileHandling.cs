using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RoDSStar.Logic.Models;
using RoDSStar.Logic.Enums;
using Serilog;
using System.Text;

namespace RoDSStar.Logic.Helpers
{
    /// <summary>
    /// The managing class of the file reading and writing.
    /// </summary>
    public class FileHandling
    {
        private readonly string path;
        private readonly string filesPath;
        private readonly string postFix;

        public FileHandling(string path, string filesPath, string postFix)
        {
            this.path = path;
            this.filesPath = filesPath;
            this.postFix = postFix;
        }

        /// <summary>
        /// This function reads the file, and make an Order object from its contents.
        /// </summary>
        /// <param name="path">File path</param>
        /// <returns>A list of Order objects</returns>
        public async Task<IList<Order>> ReadAsync()
        {
            IList<Order> records = new List<Order>();
            var lines = (await File.ReadAllLinesAsync(path)).Skip(1);

            foreach(var line in lines)
            {
                var splitLine = line.Split(';');
                records.Add(new Order
                {
                    Id = splitLine[0],
                    Products = ProductsConverter(splitLine[1].Trim(), int.Parse(splitLine[2].Replace(" ", string.Empty))),
                    Count = int.Parse(splitLine[2].Replace(" ", string.Empty)),
                    Deadline = DeadlineConverter(splitLine[3]),
                    ProfitPerPrice = int.Parse(splitLine[4].Replace(" ", string.Empty)),
                    PenaltForDelayPerDay = int.Parse(splitLine[5].Replace(" ", string.Empty).Trim()),
                });
                Log.Information($"{splitLine[0]} azonosítójú megrendelés beolvasva!");
            }

            return records;
        }

        public async Task WriteAsync(IEnumerable<Order> orders, IList<WorkStation> workStations)
        {
            using (var orderWriter = new StreamWriter( new FileStream(@$"{filesPath}Megrendelesek{postFix}.csv", FileMode.Create), encoding: Encoding.UTF8))
            {
                await orderWriter.WriteAsync("Megrendelésszám;");
                await orderWriter.WriteAsync("Profit összesen;");
                await orderWriter.WriteAsync("Levont kötbér;");
                await orderWriter.WriteAsync("Munka megkezdése;");
                await orderWriter.WriteAsync("Készre jelentés ideje;");
                await orderWriter.WriteAsync("Megrendelés eredeti határideje;");
                await orderWriter.WriteLineAsync();
                foreach (var order in orders)
                {
                    var startTime = order.Products.First().StartTime.Value;
                    order.Products = order.Products.OrderByDescending(x => x.DoneTime).ToList();
                    var doneTime = order.Products.First().DoneTime.Value;
                    var delayDay = Convert.ToInt32(Math.Floor((doneTime - order.Deadline).TotalDays));
                    if (doneTime.Hour > order.Deadline.Hour || doneTime.Hour == order.Deadline.Hour && doneTime.Minute > order.Deadline.Minute)
                    {
                        ++delayDay;
                    }

                    var penalty = delayDay > 0 ? order.PenaltForDelayPerDay * delayDay : 0;

                    await orderWriter.WriteAsync($"{order.Id};");
                    await orderWriter.WriteAsync($"{order.Count * order.ProfitPerPrice};");
                    await orderWriter.WriteAsync($"{penalty};");
                    await orderWriter.WriteAsync($"{startTime:MM.d. H:mm};");
                    await orderWriter.WriteAsync($"{doneTime:MM.d. H:mm};");
                    await orderWriter.WriteAsync($"{order.Deadline:MM.d. H:mm};");
                    await orderWriter.WriteLineAsync();
                }
            }

            using (var workStationWriter = new StreamWriter(new FileStream(@$"{filesPath}Gepek{postFix}.csv", FileMode.Create), encoding: Encoding.UTF8))
            {
                await workStationWriter.WriteAsync("Dátum;");
                await workStationWriter.WriteAsync("Gép;");
                await workStationWriter.WriteAsync("Kezdő időpont;");
                await workStationWriter.WriteAsync("Záró időpont;");
                await workStationWriter.WriteAsync("Megrendelésszám;");
                await workStationWriter.WriteLineAsync();
                foreach (var order in orders)
                {
                    var startTime = order.Products.First().StartTime.Value;
                    order.Products = order.Products.OrderByDescending(x => x.DoneTime).ToList();
                    var doneTime = order.Products.First().DoneTime.Value;
                    var delayDay = Convert.ToInt32(Math.Floor((doneTime - order.Deadline).TotalDays));
                    if (doneTime.Hour > order.Deadline.Hour || doneTime.Hour == order.Deadline.Hour && doneTime.Minute > order.Deadline.Minute)
                    {
                        ++delayDay;
                    }

                    var penalty = delayDay > 0 ? order.PenaltForDelayPerDay * delayDay : 0;

                    await workStationWriter.WriteAsync($"{order.Id};");
                    await workStationWriter.WriteAsync($"{order.Count * order.ProfitPerPrice};");
                    await workStationWriter.WriteAsync($"{penalty};");
                    await workStationWriter.WriteAsync($"{startTime:MM.d. H:mm};");
                    await workStationWriter.WriteAsync($"{doneTime:MM.d. H:mm};");
                    await workStationWriter.WriteAsync($"{order.Deadline:MM.d. H:mm};");
                    await workStationWriter.WriteLineAsync();
                }
            }
        }

        /// <summary>
        /// Makes a DateTime from the date string
        /// </summary>
        /// <param name="date">The date format from the csv file.</param>
        /// <returns>The deadline in DateTime format.</returns>
        private DateTime DeadlineConverter(string date)
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

        /// <summary>
        /// Converts the products from string
        /// </summary>
        /// <param name="productType">The type of the product</param>
        /// <param name="count">The count of the products</param>
        /// <returns>List of products</returns>
        private IList<Product> ProductsConverter(string productType, int count)
        {
            IList<Product> result = new List<Product>();

            if (count <= 0) throw new ApplicationException("Egy megrendelés darabszáma nem lehet 0 vagy annál kisebb!");

            switch(productType)
            {
                case "GYB":
                    for(var i = 0; i < count; ++i)
                    {
                        result.Add(new Product(ProductType.ChildrenBicycle));
                    }
                    break;
                case "FB":
                    for (var i = 0; i < count; ++i)
                    {
                        result.Add(new Product(ProductType.AdultBicycle));
                    }
                    break;
                case "SB":
                    for (var i = 0; i < count; ++i)
                    {
                        result.Add(new Product(ProductType.TeenagerBicycle));
                    }
                    break;
                default:
                    throw new ApplicationException("Érvénytelen termék forma!");
            }

            return result;
        }
    }
}
