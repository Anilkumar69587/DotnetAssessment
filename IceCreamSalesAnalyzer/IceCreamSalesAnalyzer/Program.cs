using IceCreamSalesAnalyzer.Core.Interfaces;
using IceCreamSalesAnalyzer.Core.Models;
using IceCreamSalesAnalyzer.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<ICsvReader<SalesRecord>, CsvReader>();

        services.AddSingleton<IValidationService, ValidationService>();

        services.AddSingleton<IReportService, ReportService>();
    })
    .Build();

try
{
    var csvReader =
        host.Services.GetRequiredService<ICsvReader<SalesRecord>>();

    var validationService =
        host.Services.GetRequiredService<IValidationService>();

    var reportService =
        host.Services.GetRequiredService<IReportService>();

    var records = await csvReader.ReadAsync("sales.csv");

    var validRecords = new List<SalesRecord>();

    for (int i = 0; i < records.Count; i++)
    {
        var record = records[i];

        bool isValid = true;

        if (record.UnitPrice * record.Quantity != record.TotalPrice)
            isValid = false;

        if (record.Quantity < 1)
            isValid = false;

        if (record.UnitPrice < 0)
            isValid = false;

        if (record.TotalPrice < 0)
            isValid = false;

        if (isValid)
        {
            validRecords.Add(record);
        }
    }

    var validationErrors = validationService.Validate(records);

    Console.WriteLine("=================================");
    Console.WriteLine("VALIDATION ERRORS");

    foreach (var error in validationErrors)
    {
        Console.WriteLine(
            $"Row: {error.RowNumber} | Error: {error.Message}");
    }

    reportService.GenerateReports(records);
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"File not found: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error: {ex.Message}");
}