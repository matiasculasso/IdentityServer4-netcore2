﻿using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using SS.Web.Security.Entities;

namespace SS.Web.Security.Configuration
{
    public class IdentityServerInitialConfiguration
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
	            new IdentityResources.Email(),
			};
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "My API"),
				new ApiResource("sscore", "SSCore Application")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "api1" }
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "core.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes =
                    {
	                    "api1",
	                    "sscore"
					}
                },

                // OpenID Connect implcit flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
					AllowAccessTokensViaBrowser = true,
                    ClientSecrets = 
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = { "http://localhost:5002/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = 
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
	                    IdentityServerConstants.StandardScopes.Email,
						"api1"
                    },
                    AllowOfflineAccess = true
                },
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Alice"),
                        new Claim("website", "https://alice.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password",

                    Claims = new List<Claim>
                    {
                        new Claim("name", "Bob"),
                        new Claim("website", "https://bob.com")
                    }
                }
            };
        }

		public static List<OpenIdConnectIdentityProvider> GetExternalProviders ()
		{
			return new List<OpenIdConnectIdentityProvider>
			{
				new OpenIdConnectIdentityProvider("oidc", "demo identity server")
				{
					ClientId = "implicit",
					Authority = "https://demo.identityserver.io/",
					SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
					AllowClaimsFromUserInfoEndpoint = true,
				},
				new OpenIdConnectIdentityProvider("google", "Google Authentication")
				{
					SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,
					ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com",
					ClientSecretName = "3gcoTrEDPPJ0ukn_aYYT6PWo"
				}
			};
		}
    }
}