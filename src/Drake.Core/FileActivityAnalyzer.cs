using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Drake.Core
{
    public class FileActivityAnalyzer : IAnalyzer
    {
        private const int LengthOfCommitHash = 40;

        private ProcessStartInfo _processStartInfo = new ProcessStartInfo()
        {
            UseShellExecute = false,
            CreateNoWindow = true,

            // TODO: Don't use hardcoded example
            WorkingDirectory = "/Users/loshar/Code/KestrelHttpServer",

            RedirectStandardError = true,
            RedirectStandardOutput = true,

            FileName = "/usr/bin/git",
            Arguments = "rev-list --objects --all",
        };

        public async Task<IEnumerable<AnalysisResult>> AnalyzeAsync(string path)
        {
            var files = new Dictionary<string, int>();

            // TODO: Create ProcessStartInfo that uses "path" parameter
            using (var process = Process.Start(_processStartInfo))
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    // Each line is in the format "<commit hash> <path>"
                    var line = await process.StandardOutput.ReadLineAsync();

                    // Skip empty lines.
                    if (line.Length <= LengthOfCommitHash + 1) continue;

                    // Trim off the hash and the space.
                    var file = line.Substring(LengthOfCommitHash + 1);

                    files.TryGetValue(file, out var count);

                    files[file] = count + 1;
                }
            }

            return files.Select(f => new AnalysisResult
                                {
                                    Path = f.Key,
                                    Weight = f.Value,
                                });
        }
    }
}