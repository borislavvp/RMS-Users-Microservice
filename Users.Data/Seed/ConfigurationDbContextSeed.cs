using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Data.Configuration;

namespace Users.Data.Seed
{
    public static class ConfigurationDbContextSeed
    {
        public static async Task SeedData(IConfiguration configuration,IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            var grantsContext = scope.ServiceProvider.GetService<PersistedGrantDbContext>();
            context.Database.Migrate();
            grantsContext.Database.Migrate();
            await SeedAsync(configuration,context);
        }

        private static async Task SeedAsync(IConfiguration configuration,ConfigurationDbContext context)
        {
            context.Set<Client>().RemoveRange(context.Clients);
            context.Set<ClientCorsOrigin>().RemoveRange(context.ClientCorsOrigins);

            foreach (var client in Config.Clients(configuration))
            {
                context.Clients.Add(client.ToEntity());
            }
            await context.SaveChangesAsync();
     
            context.Set<IdentityResource>().RemoveRange(context.IdentityResources);
            foreach (var resource in Config.IdentityResources)
            {
                context.IdentityResources.Add(resource.ToEntity());
            }
            await context.SaveChangesAsync();

            context.Set<ApiScope>().RemoveRange(context.ApiScopes);
            foreach (var scope in Config.ApiScopes)
            {
                context.ApiScopes.Add(scope.ToEntity());
            }
            await context.SaveChangesAsync();

            context.Set<ApiResource>().RemoveRange(context.ApiResources);
            foreach (var resource in Config.ApiResources)
            {
                context.ApiResources.Add(resource.ToEntity());
            }
            await context.SaveChangesAsync();
        }
    }
}
