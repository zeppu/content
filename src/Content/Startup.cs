using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Content.Models.Components;

namespace Content
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var banner = new ContainerComponent("banner");
            banner.AddChild("title", (PrimitiveComponent) PrimitiveType.String);
            banner.AddChild("shouldShowTerms", (PrimitiveComponent) PrimitiveType.Boolean);
            banner.AddChild("alt", (PrimitiveComponent) PrimitiveType.String);

            var carousel = new ContainerComponent("carousel");
            carousel.AddChild("banners", banner, true);

            var page = new ContainerComponent("page");
            page.AddChild("headerBanner", banner);
            page.AddChild("body", (PrimitiveComponent) PrimitiveType.String);

            var repo = new ComponentRepository();
            repo.AddComponent(carousel);
            repo.AddComponent(page);

            // Add framework services.
            services.AddMvc();
            services.AddSingleton(repo);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
