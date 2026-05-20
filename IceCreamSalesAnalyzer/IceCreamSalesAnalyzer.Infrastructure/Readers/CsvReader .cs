using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamSalesAnalyzer.Infrastructure.Readers
{
    public class CsvReader : ICsvReader<SalesRecord>
    {
        public async Task<List<SalesRecord>> ReadAsync(string path)
        {
            var records = new List<SalesRecord>();

            using var reader = new StreamReader(path);

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

                var parts = line.Split(',');

                var record = new SalesRecord
                {
                    Date = DateTime.Parse(parts[0]),
                    SKU = parts[1],
                    UnitPrice = decimal.Parse(parts[2]),
                    Quantity = int.Parse(parts[3]),
                    TotalPrice = decimal.Parse(parts[4])
                };

                records.Add(record);
            }

            return records;
        }
    }
}
