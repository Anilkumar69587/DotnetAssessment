using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamSalesAnalyzer.Core.Models
{
    public class ValidationError
    {
        public int RowNumber { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
