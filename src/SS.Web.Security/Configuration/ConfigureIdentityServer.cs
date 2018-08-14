using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer4;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace SS.Web.Security.Configuration
{
    public static class IdentityServerConfiguration
    {
	    public static IServiceCollection ConfigureIdentityServer(this IServiceCollection services, string connectionString, AppSettings settings)
	    {
			var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

			var cert = new X509Certificate2(Convert.FromBase64String(settings.SigningCertificate), "", X509KeyStorageFlags.MachineKeySet);

			// configure identity server with in-memory stores, keys, clients and scopes
			services.AddIdentityServer()
				.AddSigningCredential(cert)
				.AddCustomUserStore()
				// this adds the config data from DB (clients, resources)
				.AddConfigurationStore(options =>
				{
					options.ConfigureDbContext = builder =>
						builder.UseSqlServer(connectionString,
							sql => sql.MigrationsAssembly(migrationsAssembly));
				})
				// this adds the operational data from DB (codes, tokens, consents)
				.AddOperationalStore(options =>
				{
					options.ConfigureDbContext = builder =>
						builder.UseSqlServer(connectionString,
							sql => sql.MigrationsAssembly(migrationsAssembly));

					// this enables automatic token cleanup. this is optional.
					options.EnableTokenCleanup = true;
					options.TokenCleanupInterval = 30;
				});

			services.AddAuthentication()
				.AddGoogle("Google", options =>
				{
					options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

					options.ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com";
					options.ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo";
				})
				.AddOpenIdConnect("oidc", "OpenID Connect", options =>
				{
					options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
					options.SignOutScheme = IdentityServerConstants.SignoutScheme;

					options.Authority = "https://demo.identityserver.io/";
					options.ClientId = "implicit";

					options.TokenValidationParameters = new TokenValidationParameters
					{
						NameClaimType = "name",
						RoleClaimType = "role"
					};
				});

			return services;
	    }
    }
}
