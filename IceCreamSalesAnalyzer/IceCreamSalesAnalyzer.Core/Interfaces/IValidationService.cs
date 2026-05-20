using IceCreamSalesAnalyzer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamSalesAnalyzer.Core.Interfaces
{
    public interface IValidationService
    {
        List<ValidationError> Validate(List<SalesRecord> records);
    }
}
