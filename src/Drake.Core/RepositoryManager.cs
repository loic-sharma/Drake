using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Drake.Core
{
    // TODO: This class is not thread-safe!
    public class RepositoryManager
    {
        private const string GitExtension = ".git";

        private readonly string _repositoryStore;

        public RepositoryManager(string repositoryStore)
        {
            _repositoryStore = repositoryStore ?? throw new ArgumentNullException(nameof(repositoryStore));
        }

        public Task<string> Download(string repositoryUri)
        {
            return Download(new Uri(repositoryUri));
        }

        // This "clones" or "pulls" as necessary.
        public async Task<string> Download(Uri repositoryUri)
        {
            if (repositoryUri == null) throw new ArgumentNullException(nameof(repositoryUri));

            if (repositoryUri.Scheme != "https")
            {
                throw new ArgumentException(
                    $"Malformed git URI. Expected 'https' scheme, actual: '{repositoryUri.Scheme}'",
                    nameof(repositoryUri));
            }

            if (!repositoryUri.TryParseRepositoryName(out var repositoryName))
            {
                throw new ArgumentException(
                    $"Malformed git URI. Failed to parse repository name",
                    nameof(repositoryUri));
            }

            var repositoryPath = Path.Combine(_repositoryStore, repositoryName);

            Console.WriteLine(repositoryPath);

            if (Directory.Exists(repositoryPath))
            {
                Console.WriteLine("Pulling");

                await ExecuteProcess(CreatePullProcessInfo(repositoryPath));
            }
            else
            {
                Console.WriteLine("Cloning");

                await ExecuteProcess(CreateCloneProcessInfo(repositoryUri, repositoryName));
            }

            return repositoryPath;
        }

        public string ReadAllText(string path)
        {
            path = Path.Combine(_repositoryStore, path);

            // TODO: Comment, throws FileNotFoundException
            return System.IO.File.ReadAllText(path);
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

        private ProcessStartInfo CreateCloneProcessInfo(Uri repositoryUri, string repositoryName)
        {
            return new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,

                WorkingDirectory = _repositoryStore,

                RedirectStandardError = true,
                RedirectStandardOutput = true,

                FileName = "/usr/bin/git",
                Arguments = $"clone {repositoryUri} {repositoryName}",
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