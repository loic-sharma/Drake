using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Drake.Analyzers
{
    public class CommitAnalyzer
    {
        private const int LengthOfCommitHash = 40;

        public async Task<Dictionary<string, int>> CountCommitsAsync(string repositoryPath)
        {
            var paths = new Dictionary<string, int>();

            using (var process = Process.Start(CreateProcessStartInfo(repositoryPath)))
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    // Each line is in the format "<commit hash> <path>"
                    var line = await process.StandardOutput.ReadLineAsync();

                    // Skip empty lines.
                    if (line.Length <= LengthOfCommitHash + 1) continue;

                    // Trim off the hash and the space.
                    var path = line.Substring(LengthOfCommitHash + 1);

                    paths.TryGetValue(path, out var count);

                    paths[path] = count + 1;
                }
            }

            return paths.Where(p => File.Exists(Path.Combine(repositoryPath, p.Key)))
                        .ToDictionary(p => p.Key, p => p.Value);
        }

        private ProcessStartInfo CreateProcessStartInfo(string path)
        {
            return new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,

                WorkingDirectory = path,

                RedirectStandardError = true,
                RedirectStandardOutput = true,

                FileName = "/usr/bin/git",
                Arguments = "rev-list --objects --all",
            };
        }
    }
}