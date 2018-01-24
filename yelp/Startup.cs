using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using yelp.Models;


namespace yelp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddSession();
            // mySQL connection string
            // services.Configure<MySqlOptions>(Configuration.GetSection("DBInfo"));
            // Dapper Factory connection
            // services.AddScoped<DbConnector>();
            services.AddDbContext<YelpContext>(MySqlOptions => MySqlOptions.UseMySQL(Configuration["DBInfo:ConnectionString"]));
            // services.AddDbContext<YelpContext>(options => options.UseNpgsql(Configuration["PostGresInfo:ConnectionString"]));
            services.Configure<GoogleMapSettings>(Configuration.GetSection("GoogleMapsAPI"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc();
        }
        // database connection
        public IConfiguration Configuration { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
    }
}
