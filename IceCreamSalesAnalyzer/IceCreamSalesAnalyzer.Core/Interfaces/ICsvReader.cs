using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamSalesAnalyzer.Core.Interfaces
{
    public interface ICsvReader<T>
    {
        Task<List<T>> ReadAsync(string path);
    }
}
