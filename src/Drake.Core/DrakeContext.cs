using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Drake.Core
{
    public class DrakeContext : DbContext
    {
        public DbSet<Repository> Repositories { get; set; }
        public DbSet<File> Files { get; set; }

        public DrakeContext(DbContextOptions<DrakeContext> options)
            : base(options)
        { }
    }
}