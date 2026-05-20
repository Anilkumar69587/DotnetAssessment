using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCreamSalesAnalyzer.Infrastructure.Benchmarks
{
    [MemoryDiagnoser]
    public class SalesBenchmark
    {
        [Benchmark]
        public void RunBenchmark()
        {
        }
    }
}
