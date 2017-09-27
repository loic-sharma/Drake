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
            using (var db = new DrakeContext())
            {
                var repository = db
                    .Repositories
                    .Where(r => r.RepositoryUri == args[0])
                    .Include(r => r.Files)
                    .FirstOrDefault();

                if (repository == null)
                {
                    Console.WriteLine($"First time seeing repository {args[0]}, downloading...");

                    // TODO: Accept repositoryStore from args
                    var repositoryStore = "/Users/loshar/Code/Drake/store";
                    var repositoryManager = new RepositoryManager(repositoryStore);

                    var repositoryPath = await repositoryManager.Download(args[0]);

                    Console.WriteLine("Analyzing...");

                    var repositoryEntity = new Repository
                    {
                        RepositoryUri = args[0],
                        LastPullTime = DateTimeOffset.Now,
                        Files = new List<File>()
                    };

                    var analyzer = new ExtensionAnalyzer(new FileActivityAnalyzer());

                    foreach (var result in await analyzer.AnalyzeAsync(repositoryPath))
                    {
                        repositoryEntity.Files.Add(new File
                        {
                            Path = result.Path,
                            Score = result.Weight,
                        });

                        Console.WriteLine($"{result.Path}: {result.Weight}");
                    }

                    db.Repositories.Add(repositoryEntity);

                    await db.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine($"Already seen repository {args[0]}");

                    foreach (var file in repository.Files.OrderByDescending(f => f.Score))
                    {
                        Console.WriteLine($"{file.Path}: {file.Score}");
                    }
                }
            }
        }
    }
}
