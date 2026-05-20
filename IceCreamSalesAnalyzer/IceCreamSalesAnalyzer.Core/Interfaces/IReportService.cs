using IceCreamSalesAnalyzer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamSalesAnalyzer.Core.Interfaces
{
    public interface IReportService
    {
        void GenerateReports(List<SalesRecord> records);
    }
}
