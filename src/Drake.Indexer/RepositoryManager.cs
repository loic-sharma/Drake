using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Drake.Indexer
{
    public class RepositoryManager
    {
        private string _repositoryStore;
        private const string GitExtension = ".git";

        public RepositoryManager(string repositoryStore)
        {
            _repositoryStore = repositoryStore ?? throw new ArgumentNullException(nameof(repositoryStore));
        }

        public Task<bool> Download(string repositoryUri)
        {
            return Download(new Uri(repositoryUri));
        }

        // This "clones" or "pulls" as necessary.
        public Task<bool> Download(Uri repositoryUri)
        {
            if (repositoryUri == null) throw new ArgumentNullException(nameof(repositoryUri));

            if (repositoryUri.Scheme != "https")
            {
                throw new ArgumentException(
                    $"Malformed git URI. Expected 'https' scheme, actual: '{repositoryUri.Scheme}'",
                    nameof(repositoryUri));
            }

            if (!TryParseRepositoryName(repositoryUri, out var repositoryName))
            {
                throw new ArgumentException("Malformed git URI", nameof(repositoryUri));
            }

            var repositoryPath = Path.Combine(_repositoryStore, repositoryName);

            Console.WriteLine(repositoryPath);

            if (Directory.Exists(repositoryPath))
            {
                Console.WriteLine("Pulling");

                return ExecuteProcess(CreatePullProcessInfo(repositoryPath));
            }
            else
            {
                Console.WriteLine("Cloning");

                return ExecuteProcess(CreateCloneProcessInfo(repositoryUri));
            }
        }

        private bool TryParseRepositoryName(Uri repositoryUri, out string repositoryName)
        {
            var repositorySegment = repositoryUri.Segments.LastOrDefault();

            if (repositorySegment.Length > GitExtension.Length)
            {
                var extension = repositorySegment.Substring(repositorySegment.Length - GitExtension.Length);

                if (extension == GitExtension)
                {
                    repositoryName = repositorySegment.Substring(0, repositorySegment.Length - GitExtension.Length);

                    return true;
                }
            }

            repositoryName = null;

            return false;
        }

        private ProcessStartInfo CreatePullProcessInfo(string path)
        {
            return new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,

                WorkingDirectory = path,

                RedirectStandardError = true,
                RedirectStandardOutput = true,

                FileName = "/usr/bin/git",
                Arguments = "pull",
            };
        }

        private ProcessStartInfo CreateCloneProcessInfo(Uri repositoryUri)
        {
            return new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,

                WorkingDirectory = _repositoryStore,

                RedirectStandardError = true,
                RedirectStandardOutput = true,

                FileName = "/usr/bin/git",
                Arguments = $"clone {repositoryUri}",
            };
        }

        private async Task<bool> ExecuteProcess(ProcessStartInfo processStartInfo)
        {
            using (var process = Process.Start(processStartInfo))
            {
                while (!process.StandardOutput.EndOfStream)
                {
                    var line = await process.StandardOutput.ReadLineAsync();

                    Console.WriteLine(line);

                    if (!process.StandardError.EndOfStream)
                    {
                        Console.WriteLine(await process.StandardOutput.ReadLineAsync());
                    }
                }
            }

            // TODO: Figure out how to do error handling
            return true;
        }
    }
}