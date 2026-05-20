using IceCreamSalesAnalyzer.Core.Models;
using IceCreamSalesAnalyzer.Core.Services;

namespace IceCreamSalesAnalyzer.Tests;

public class ValidationServiceTests
{
    [Fact]
    public void Should_Return_Error_When_Quantity_Is_Invalid()
    {
        var service = new ValidationService();

        var records = new List<SalesRecord>
        {
            new SalesRecord
            {
                Quantity = 0,
                UnitPrice = 100,
                TotalPrice = 0
            }
        };

        var result = service.Validate(records);

        Assert.Single(result);
    }
}