using IceCreamSalesAnalyzer.Core.Interfaces;
using IceCreamSalesAnalyzer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamSalesAnalyzer.Core.Services
{
    public class CsvReader : ICsvReader<SalesRecord>
    {
        public async Task<List<SalesRecord>> ReadAsync(string path)
        {
            var records = new List<SalesRecord>();

            using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            using var reader = new StreamReader(stream);

            bool isHeader = true;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                var columns = line.Split(',');

                var record = new SalesRecord
                {
                    Date = DateTime.Parse(columns[0]),
                    SKU = columns[1],
                    UnitPrice = decimal.Parse(columns[2]),
                    Quantity = int.Parse(columns[3]),
                    TotalPrice = decimal.Parse(columns[4])
                };

                records.Add(record);
            }

            return records;
        }
    }
}
