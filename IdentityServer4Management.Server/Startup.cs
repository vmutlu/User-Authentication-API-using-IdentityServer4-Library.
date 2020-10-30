using IdentityServer4Management.Infrastructure.Extensions;
using IdentityServer4Management.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer4Management.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
               .AddDatabaseConfiguration(Configuration.GetConnectionString("MyConnection"))
                .AddIdendityConfig(Configuration) // buda yazmýþ oldugum extensions
                .AddServices<ApplicationUser>(); //addservices servicecollection içersinde yazmýþ oldugum extension metod

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()));

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseIdentityServer();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                     name: "default",
                     template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
