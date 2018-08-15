using System;
using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SS.Web.Security.Configuration;
using SS.Web.Security.Data;

namespace SS.Web.Security
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding database...");

	        Console.WriteLine("Seeding Identity Server initial data...");
			using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {               
				var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();               
                EnsureSeedData(context);
            }
	        Console.WriteLine("Done seeding initial data.");

	        Console.WriteLine("Seeding External Identity Providers...");
			using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
	        {
		        var ssContext = scope.ServiceProvider.GetRequiredService<SSDbContext>();
		        EnsureSeedExternalIdps(ssContext);
	        }
			Console.WriteLine("Done seeding External Identity Providers...");

			Console.WriteLine();
        }

	    private static void EnsureSeedExternalIdps(SSDbContext context)
	    {
		    if (context.ExternalIdentityProvider.Any())
		    {
			    return;
		    }

		    Console.WriteLine("OIDC External Idps being populated");
		    foreach (var oidcProvider in IdentityServerInitialConfiguration.GetExternalProviders().ToList())
		    {
			    context.OpenIdConnectIdentityProvider.Add(oidcProvider);
		    }
		    context.SaveChanges();
	    }

		private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                Console.WriteLine("Clients being populated");
                foreach (var client in IdentityServerInitialConfiguration.GetClients().ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Clients already populated");
            }

            if (!context.IdentityResources.Any())
            {
                Console.WriteLine("IdentityResources being populated");
                foreach (var resource in IdentityServerInitialConfiguration.GetIdentityResources().ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }

            if (!context.ApiResources.Any())
            {
                Console.WriteLine("ApiResources being populated");
                foreach (var resource in IdentityServerInitialConfiguration.GetApiResources().ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("ApiResources already populated");
            }
        }
    }
}
