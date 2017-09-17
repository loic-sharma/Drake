using System;

namespace Drake.Indexer
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Accept repositoryStore from args
            var repositoryStore = "/Users/loshar/Code/Drake/store";
            var repositoryManager = new RepositoryManager(repositoryStore);

            repositoryManager.Download(args[0]);
        }
    }
}
