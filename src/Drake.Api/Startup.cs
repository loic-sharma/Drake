using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Drake.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Drake.Api
{
    public class Startup
    {
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
        }

        private void ConfigureDatabaseOptions(DbContextOptionsBuilder options)
        {
            // TODO: Configs are nice.
            var databasePath = "/Users/loshar/Code/Drake/drake.db";

            options.UseSqlite($"Data Source={databasePath}");
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
