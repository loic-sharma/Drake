using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Drake.Core
{
    public class DrakeContext : DbContext
    {
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<File> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite("Data Source=drake.db"); // TODO: Config
        }
    }

    public class Repository
    {
        public int RepositoryId { get; set; }
        public string Uri { get; set; }
        public DateTimeOffset LastUpdate { get; set; }

        public string Name
        {
            get
            {
                var name = new Uri(Uri).AbsolutePath;

                if (name.EndsWith(".git"))
                {
                    name = name.Substring(0, name.Length - ".git".Length);
                }

                return name;
            }
        }

        public List<File> Files { get; set; }
    }

    public class File
    {
        public int FileId { get; set; }
        public string Path { get; set; }
        public int Commits { get; set; }

        public int RepositoryId { get; set; }
        public Repository Repository { get; set; }
    }
}