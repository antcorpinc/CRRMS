using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CRRMS.Data;
using CRRMS.Web.Services;
using Microsoft.Extensions.Configuration;
using CRRMS.Web.Models;
using AutoMapper;
using CRRMS.Web.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CRRMS.Web
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                            .SetBasePath(_env.ContentRootPath)
                            .AddJsonFile("config.json")
                            //.AddEnvironmentVariables()
                            ;

            _config = builder.Build();

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton(_config);

            // Info : If your organization has other env like testing 
            if (_env.IsEnvironment("Development") || _env.IsEnvironment("Testing"))
                services.AddScoped<IMailService, DebugMailService>();
            else
            {
                //IMplement  real service
            }

            services.AddIdentity<WorldUser, IdentityRole>(config =>
             {
                 config.User.RequireUniqueEmail = true;
                 config.Password.RequiredLength = 8;
                 config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                 config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                 {
                     OnRedirectToLogin= async ctx =>
                     {
                         if(ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode==200)
                         {
                             ctx.Response.StatusCode = 401;
                         }
                         else
                         {
                             ctx.Response.Redirect(ctx.RedirectUri);
                         }
                         //Let the task complete
                         await Task.Yield();
                     }
                 };
             }
            )
            .AddEntityFrameworkStores<WorldContext>();

            services.AddDbContext<WorldContext>();

            services.AddScoped<IWorldRepository, WorldRepository>();

            services.AddTransient<GeoCoordsService>();

            services.AddTransient<WorldContextSeedData>();

            services.AddLogging();

            services.AddMvc(config=>
            {
                if (_env.IsProduction())
                {
                    config.Filters.Add(new RequireHttpsAttribute());
                }
            });
          //  services.AddDbContext<CRRMSContext>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env, ILoggerFactory factory
            ,WorldContextSeedData seeder)
        {
            // loggerFactory.AddConsole();
            //  if(env.IsEnvironment("Development"))

            Mapper.Initialize(config =>
            {
                config.CreateMap<TripViewModel, Trip>().ReverseMap();
                config.CreateMap<StopViewModel, Stop>().ReverseMap();
            });

              if (env.IsDevelopment())
              {
                  app.UseDeveloperExceptionPage();
                 factory.AddDebug(LogLevel.Information);
              }
            else
            {
                factory.AddDebug(LogLevel.Error);
            }
          //  app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseIdentity();


            //  app.UseMvc(config =>
            //  {
            //  config.MapRoute(
            //   name:"Default",
            //   template:"{controller}/{action}/{id?}",
            //   defaults: new {controller="App",action="Index"}
            //  );
            //  });

            app.UseMvc(config =>
            {
                config.MapRoute(
                name: "Default",
                template: "{controller}/{action}/{id?}",
                defaults: new { controller = "App", action = "Index" }
                );
            });

            // Info:Make the async wait to complete -- like sync.
            seeder.EnsureSeedData().Wait();


            // app.Run(async (context) =>
            // {
            //     await context.Response.WriteAsync("Hello World!");
            // });
        }
    }
}
