using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SS.Web.Security.Data;
using SS.Web.Security.Entities;

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

			var serviceProvider = services.BuildServiceProvider();
			var dbContext = serviceProvider.GetService(typeof(SSDbContext)) as SSDbContext;

			var externalProviders = dbContext.ExternalIdentityProvider.ToList();

			services.AddAuthentication().AddOpenIdConnect("ADFS", "ADFS", options =>
			{
				options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
				options.SignOutScheme = IdentityServerConstants.SignoutScheme;
				options.Authority = "https://win-sdokv09p8ic.adfs.domain.com/adfs";
				options.ClientId = "mvc-app";
				options.RequireHttpsMetadata = false;
				options.Scope.Add("openid");
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = false
				};
				options.GetClaimsFromUserInfoEndpoint = true;
			});

			foreach (var oidcProvider in externalProviders.OfType<OpenIdConnectIdentityProvider>())
			{
				if (oidcProvider.AuthenticationScheme == "google")
				{
					services.AddAuthentication().AddGoogle(oidcProvider.AuthenticationScheme, oidcProvider.DisplayName,
						options =>
						{
							options.ClientId = oidcProvider.ClientId;
							options.ClientSecret = oidcProvider.ClientSecretName;
							options.SignInScheme = oidcProvider.SignInScheme.Trim();
						});
					continue;
				}

				services.AddAuthentication().AddOpenIdConnect(oidcProvider.AuthenticationScheme, oidcProvider.DisplayName,
					options =>
					{
						options.GetClaimsFromUserInfoEndpoint = oidcProvider.AllowClaimsFromUserInfoEndpoint ?? false;
						options.SignInScheme = oidcProvider.SignInScheme.Trim();
						options.SignOutScheme = IdentityServerConstants.SignoutScheme;
						options.Authority = oidcProvider.Authority.Trim();
						options.ClientId = oidcProvider.ClientId.Trim();
						options.TokenValidationParameters = new TokenValidationParameters
						{
							NameClaimType = JwtClaimTypes.Name,
							RoleClaimType = JwtClaimTypes.Role
						};
					});
			}


			var samlProviders = externalProviders.OfType<SAMLIdentityProvider>().ToList();
			foreach (var samlProvider in samlProviders)
			{
				//TODO Add SAML Identity Providers 
			}

			//TODO LDAP Identity Providers
			var ldapIdentityProviders = externalProviders.OfType<LDAPIdentityProvider>().ToList();
			foreach (var samlProvider in samlProviders)
			{
				// TODO Add LDAP Identity Providers
			}

			return services;
	    }
    }
}
