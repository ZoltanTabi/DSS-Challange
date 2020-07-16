using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace RoDSStar.Model
{
    public class FileHandling
    {
        public IEnumerable<Order> Read(string path)
        {
            IEnumerable<Order> records;
            using (var reader = new StreamReader(path))
            {
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                csv.Configuration.IgnoreBlankLines = false;
                csv.Configuration.RegisterClassMap<OrderMap>();
                csv.ReadHeader();
                records = csv.GetRecords<Order>();
            }

            return records;
        }
    }

    public sealed class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Map(m => m.Id);
            Map(m => m.Product).ConvertUsing(row => (ProductType)Enum.Parse(typeof(ProductType), row.GetField("Product")));
            Map(m => m.Count);
            Map(m => m.Deadline).ConvertUsing(row => DeadlineConverter(row.GetField("Deadline")));
            Map(m => m.ProfitPerPrice);
            Map(m => m.PenaltForDelayPerDay);
        }

        private static DateTime DeadlineConverter(string date)
        {
            var splitDate = date.Split(".:".ToCharArray());

            if (splitDate.Length != 4) throw new ApplicationException("Helytelen határidő forma!");

            var success = int.TryParse(splitDate[0], out int month);
            success = int.TryParse(splitDate[1], out int day) && success;
            success = int.TryParse(splitDate[2], out int hour) && success;
            success = int.TryParse(splitDate[3], out int minute) && success;

            if (!success) throw new ApplicationException("Helytelen határidő forma!");

            DateTime deadline = new DateTime(DateTime.Now.Year, month, day, hour, minute, 0);

            if (deadline < DateTime.Now) deadline.AddYears(1);

            return deadline;
        }
    }
}
