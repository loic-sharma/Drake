using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Drake.Core
{
    public class DrakeContextFactory : IDesignTimeDbContextFactory<DrakeContext>
    {
        // TODO: Configs are nice
        private const string DatabasePath = "/Users/loshar/Code/Drake/drake.db";

        public DrakeContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DrakeContext>();
            optionsBuilder.UseSqlite($"Data Source={DatabasePath}");

            return new DrakeContext(optionsBuilder.Options);
        }
    }
}