using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drake.Analyzers
{
    public struct AnalysisResult
    {
        public string Path { get; set; }

        public float Weight { get; set; }
    }

    public interface IAnalyzer
    {
        Task<IEnumerable<AnalysisResult>> AnalyzeAsync(string path);
    }
}
