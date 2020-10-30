using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdentityServer4Management.Infrastructure.Persistence
{
    //fake veriler
    public class SeedDatabase
    {
        public static void EnsureSeedData(IServiceProvider provider)
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            provider.GetRequiredService<AppIdentityDbContext>().Database.Migrate();
            provider.GetRequiredService<AppPersistedDbContext>().Database.Migrate();
            provider.GetRequiredService<AppConfigurationDbContext>().Database.Migrate();

            var context = provider.GetRequiredService<AppConfigurationDbContext>();

            if(!context.Clients.Any())
            {
                var clients = new List<Client>();
                configuration.GetSection("IdentityServer:Clients").Bind(clients);
                foreach (var item in clients)
                    context.Clients.Add(item.ToEntity());

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                var apiResources = new List<ApiResource>();
                configuration.GetSection("IdentityServer:ApiResources").Bind(apiResources);
                foreach (var item in apiResources)
                    context.ApiResources.Add(item.ToEntity());

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                var identityResources = new List<IdentityResource>();
                configuration.GetSection("IdentityServer:IdentityResources").Bind(identityResources);
                foreach (var item in identityResources)
                    context.IdentityResources.Add(item.ToEntity());

                context.SaveChanges();
            }
        }
    }
}
