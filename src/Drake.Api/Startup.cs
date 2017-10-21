using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drake.Core;
using Drake.Indexing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Drake.Api
{
    public class Startup
    {
        // TODO: Configs are nice
        private const string DatabasePath = "/Users/loshar/Code/Drake/drake.db";
        private const string RepositoryStorePath = "/Users/loshar/Code/Drake/store";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DrakeContext>(ConfigureDatabaseOptions);
            services.AddMvc();

            services.AddSingleton<RepositoryManager>(p => new RepositoryManager(RepositoryStorePath));
            services.AddScoped<Indexer>();
        }

        private void ConfigureDatabaseOptions(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DatabasePath}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
