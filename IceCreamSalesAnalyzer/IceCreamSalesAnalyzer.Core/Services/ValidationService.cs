using IceCreamSalesAnalyzer.Core.Interfaces;
using IceCreamSalesAnalyzer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamSalesAnalyzer.Core.Services
{
    public class ValidationService : IValidationService
    {
        public List<ValidationError> Validate(List<SalesRecord> records)
        {
            var errors = new List<ValidationError>();

            for (int i = 0; i < records.Count; i++)
            {
                var row = records[i];

                if (row.UnitPrice * row.Quantity != row.TotalPrice)
                {
                    errors.Add(new ValidationError
                    {
                        RowNumber = i + 1,
                        Message = "UnitPrice * Quantity != TotalPrice"
                    });
                }

                if (row.Quantity < 1)
                {
                    errors.Add(new ValidationError
                    {
                        RowNumber = i + 1,
                        Message = "Quantity < 1"
                    });
                }

                if (row.UnitPrice < 0)
                {
                    errors.Add(new ValidationError
                    {
                        RowNumber = i + 1,
                        Message = "UnitPrice < 0"
                    });
                }

                if (row.TotalPrice < 0)
                {
                    errors.Add(new ValidationError
                    {
                        RowNumber = i + 1,
                        Message = "TotalPrice < 0"
                    });
                }
            }

            return errors;
        }
    }
}
