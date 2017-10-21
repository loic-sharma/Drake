using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drake.Core;
using Drake.Indexing;
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

            // TODO: Configs are nice
            var databasePath = "/Users/loshar/Code/Drake/drake.db";
            var repositoryStore = "/Users/loshar/Code/Drake/store";

            var repositoryManager = new RepositoryManager(repositoryStore);

            var optionsBuilder = new DbContextOptionsBuilder<DrakeContext>();
            optionsBuilder.UseSqlite($"Data Source={databasePath}");

            using (var db = new DrakeContext(optionsBuilder.Options))
            {
                var indexer = new Drake.Indexing.Indexer(db, repositoryManager);

                await indexer.Index(repositoryUri);
            }
        }
    }
}
