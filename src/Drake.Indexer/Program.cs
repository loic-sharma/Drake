using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drake.Analyzers;
using Drake.Core;
using Microsoft.EntityFrameworkCore;

namespace Drake.Indexer
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var repositoryUri = args[0];

            // TODO: Accept repositoryStore from args
            var repositoryStore = "/Users/loshar/Code/Drake/store";
            var repositoryManager = new RepositoryManager(repositoryStore);

            using (var db = new DrakeContext())
            {
                var repository = db
                    .Repositories
                    .Where(r => r.Uri == repositoryUri)
                    .Include(r => r.Files)
                    .FirstOrDefault();

                if (repository == null)
                {
                    Console.WriteLine($"First time seeing repository {repositoryUri}, downloading...");

                    var repositoryPath = await repositoryManager.Download(repositoryUri);

                    Console.WriteLine("Analyzing...");

                    repository = new Repository
                    {
                        Uri = repositoryUri,
                        LastUpdate = DateTimeOffset.Now,
                        Files = new List<File>()
                    };

                    db.Repositories.Add(repository);

                    await AnalyzeFiles(repository, repositoryPath);
                    await db.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine($"Already seen repository {repositoryUri}");

                    foreach (var file in repository.Files.OrderByDescending(f => f.Commits))
                    {
                        Console.WriteLine($"{file.Path}: {file.Commits}");
                    }

                    Console.WriteLine("Updating local repository copy...");

                    var repositoryPath = await repositoryManager.Download(repositoryUri);

                    Console.WriteLine("Analyzing...");

                    repository.LastUpdate = DateTimeOffset.Now;

                    db.Files.RemoveRange(repository.Files);

                    await AnalyzeFiles(repository, repositoryPath);
                    await db.SaveChangesAsync();
                }
            }
        }

        private static async Task AnalyzeFiles(Repository repository, string repositoryPath)
        {
            var analyzer = new CommitAnalyzer();

            foreach (var result in await analyzer.CountCommitsAsync(repositoryPath))
            {
                repository.Files.Add(new File
                {
                    Path = result.Key,
                    Commits = result.Value,
                });

                Console.WriteLine($"{result.Key}: {result.Value}");
            }
        }
    }
}
