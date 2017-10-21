using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drake.Core;
using Microsoft.EntityFrameworkCore;

namespace Drake.Indexing
{
    public class Indexer
    {
        private readonly DrakeContext _db;
        private readonly RepositoryManager _repositoryManager;

        // TODO: Add logger
        public Indexer(DrakeContext db, RepositoryManager manager)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _repositoryManager = manager ?? throw new ArgumentNullException(nameof(manager));
        }

        public async Task<Repository> Index(string repositoryUri)
        {
            var repository = await _db
                .Repositories
                .Where(r => r.Uri == repositoryUri)
                .Include(r => r.Files)
                .FirstOrDefaultAsync();

            if (repository == null)
            {
                return await DownloadAndAnalyze(repositoryUri);
            }
            else
            {
                return await Update(repository);
            }
        }

        private async Task<Repository> DownloadAndAnalyze(string repositoryUri)
        {
            // TODO: Use logger instead of Console
            Console.WriteLine($"First time seeing repository {repositoryUri}, downloading...");

            var repositoryPath = await _repositoryManager.Download(repositoryUri);

            Console.WriteLine("Analyzing...");

            var repository = new Repository
            {
                Uri = repositoryUri,
                LastUpdate = DateTimeOffset.Now,
                Files = new List<File>()
            };

            _db.Repositories.Add(repository);

            await AnalyzeFiles(repository, repositoryPath);
            await _db.SaveChangesAsync();

            return repository;
        }

        private async Task<Repository> Update(Repository repository)
        {
            Console.WriteLine($"Already seen repository {repository.Uri}");

            foreach (var file in repository.Files.OrderByDescending(f => f.Commits))
            {
                Console.WriteLine($"{file.Path}: {file.Commits}");
            }

            Console.WriteLine("Updating local repository copy...");

            var repositoryPath = await _repositoryManager.Download(repository.Uri);

            Console.WriteLine("Analyzing...");

            repository.LastUpdate = DateTimeOffset.Now;

            _db.Files.RemoveRange(repository.Files);

            await AnalyzeFiles(repository, repositoryPath);
            await _db.SaveChangesAsync();

            return repository;
        }

        private async Task AnalyzeFiles(Repository repository, string repositoryPath)
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