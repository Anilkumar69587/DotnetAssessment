using IceCreamSalesAnalyzer.Core.Interfaces;
using IceCreamSalesAnalyzer.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamSalesAnalyzer.Core.Services
{
    public class ReportService : IReportService
    {
        private readonly ILogger<ReportService> _logger;

        public ReportService(ILogger<ReportService> logger)
        {
            _logger = logger;
        }

        public void GenerateReports(List<SalesRecord> records)
        {
            GenerateTotalSales(records);

            GenerateMonthlySales(records);

            GeneratePopularItems(records);

            GenerateHighestRevenueItems(records);

            GenerateGrowthReport(records);
        }

        private void GenerateTotalSales(List<SalesRecord> records)
        {
            decimal totalSales = 0;

            foreach (var record in records)
            {
                totalSales += record.TotalPrice;
            }

            _logger.LogInformation("=================================");
            _logger.LogInformation($"TOTAL SALES : {totalSales}");
        }

        private void GenerateMonthlySales(List<SalesRecord> records)
        {
            var monthlySales = new Dictionary<string, decimal>();

            foreach (var record in records)
            {
                var month = record.Date.ToString("yyyy-MM");

                if (!monthlySales.ContainsKey(month))
                {
                    monthlySales[month] = 0;
                }

                monthlySales[month] += record.TotalPrice;
            }

            _logger.LogInformation("=================================");
            _logger.LogInformation("MONTH WISE SALES");

            foreach (var item in monthlySales)
            {
                _logger.LogInformation($"{item.Key} : {item.Value}");
            }
        }

        private void GeneratePopularItems(List<SalesRecord> records)
        {
            var monthlyItems = new Dictionary<string, Dictionary<string, List<int>>>();

            foreach (var record in records)
            {
                var month = record.Date.ToString("yyyy-MM");

                if (!monthlyItems.ContainsKey(month))
                {
                    monthlyItems[month] = new Dictionary<string, List<int>>();
                }

                if (!monthlyItems[month].ContainsKey(record.SKU))
                {
                    monthlyItems[month][record.SKU] = new List<int>();
                }

                monthlyItems[month][record.SKU].Add(record.Quantity);
            }

            _logger.LogInformation("=================================");
            _logger.LogInformation("MOST POPULAR ITEMS");

            foreach (var month in monthlyItems)
            {
                string popularItem = string.Empty;

                int maxQuantity = 0;

                List<int> orders = new();

                foreach (var item in month.Value)
                {
                    int totalQty = 0;

                    foreach (var qty in item.Value)
                    {
                        totalQty += qty;
                    }

                    if (totalQty > maxQuantity)
                    {
                        maxQuantity = totalQty;
                        popularItem = item.Key;
                        orders = item.Value;
                    }
                }

                int min = orders[0];
                int max = orders[0];
                int sum = 0;

                foreach (var qty in orders)
                {
                    if (qty < min)
                        min = qty;

                    if (qty > max)
                        max = qty;

                    sum += qty;
                }

                decimal average = (decimal)sum / orders.Count;

                _logger.LogInformation(
                    $"Month: {month.Key} | Item: {popularItem} | Total Qty: {maxQuantity} | Min: {min} | Max: {max} | Avg: {average:F2}");
            }
        }

        private void GenerateHighestRevenueItems(List<SalesRecord> records)
        {
            var revenueMap = new Dictionary<string, Dictionary<string, decimal>>();

            foreach (var record in records)
            {
                var month = record.Date.ToString("yyyy-MM");

                if (!revenueMap.ContainsKey(month))
                {
                    revenueMap[month] = new Dictionary<string, decimal>();
                }

                if (!revenueMap[month].ContainsKey(record.SKU))
                {
                    revenueMap[month][record.SKU] = 0;
                }

                revenueMap[month][record.SKU] += record.TotalPrice;
            }

            _logger.LogInformation("=================================");
            _logger.LogInformation("HIGHEST REVENUE ITEMS");

            foreach (var month in revenueMap)
            {
                string itemName = string.Empty;

                decimal highestRevenue = 0;

                foreach (var item in month.Value)
                {
                    if (item.Value > highestRevenue)
                    {
                        highestRevenue = item.Value;
                        itemName = item.Key;
                    }
                }

                _logger.LogInformation(
                    $"Month: {month.Key} | Item: {itemName} | Revenue: {highestRevenue}");
            }
        }

        private void GenerateGrowthReport(List<SalesRecord> records)
        {
            var salesMap = new Dictionary<string, Dictionary<string, decimal>>();

            foreach (var record in records)
            {
                var month = record.Date.ToString("yyyy-MM");

                if (!salesMap.ContainsKey(month))
                {
                    salesMap[month] = new Dictionary<string, decimal>();
                }

                if (!salesMap[month].ContainsKey(record.SKU))
                {
                    salesMap[month][record.SKU] = 0;
                }

                salesMap[month][record.SKU] += record.TotalPrice;
            }

            var months = new List<string>();

            foreach (var month in salesMap.Keys)
            {
                months.Add(month);
            }

            months.Sort();

            _logger.LogInformation("=================================");
            _logger.LogInformation("MONTH TO MONTH GROWTH");

            for (int i = 1; i < months.Count; i++)
            {
                var previousMonth = months[i - 1];
                var currentMonth = months[i];

                foreach (var item in salesMap[currentMonth])
                {
                    decimal currentValue = item.Value;

                    decimal previousValue = 0;

                    if (salesMap[previousMonth].ContainsKey(item.Key))
                    {
                        previousValue = salesMap[previousMonth][item.Key];
                    }

                    if (previousValue == 0)
                    {
                        continue;
                    }

                    decimal growth =
                        ((currentValue - previousValue) / previousValue) * 100;

                    _logger.LogInformation(
                        $"Item: {item.Key} | {previousMonth} -> {currentMonth} | Growth: {growth:F2}%");
                }
            }
        }
    }
}
