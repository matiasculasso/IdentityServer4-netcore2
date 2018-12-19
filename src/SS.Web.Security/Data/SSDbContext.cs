using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SS.Web.Security;
using SS.Web.Security.Entities;

namespace SS.Web.Security.Data
{
	public class SSDbContext : DbContext
	{
		public SSDbContext(DbContextOptions<SSDbContext> options) : base(options)
		{
		}

		public DbSet<ExternalIdentityProvider> ExternalIdentityProvider { get; set; }
		public DbSet<OpenIdConnectIdentityProvider> OpenIdConnectIdentityProvider { get; set; }
		public DbSet<SAMLIdentityProvider> SAMLIdentityProvider { get; set; }
		public DbSet<LDAPIdentityProvider> LDAPIdentityProvider { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ExternalIdentityProvider>()
				.ToTable("ExternalIdentityProvider", "ss")
				.HasAlternateKey(x => x.AuthenticationScheme);

			modelBuilder.Entity<ExternalIdentityProvider>()
				.HasIndex(x => new { x.AuthenticationScheme });


			modelBuilder.Entity<OpenIdConnectIdentityProvider>();
			modelBuilder.Entity<SAMLIdentityProvider>();
			modelBuilder.Entity<LDAPIdentityProvider>();

			modelBuilder.Entity<ExternalIdentityProvider>()
				.HasDiscriminator<AuthenticationProtocol>("AuthenticationProtocol")
				.HasValue<OpenIdConnectIdentityProvider>(AuthenticationProtocol.OpenIDConnect)
				.HasValue<SAMLIdentityProvider>(AuthenticationProtocol.SAML)
				.HasValue<LDAPIdentityProvider>(AuthenticationProtocol.LDAP);

			base.OnModelCreating(modelBuilder);
		}
	}

	public static class SSDbContextExtensions
	{
		public static void Migrate(this SSDbContext ctx)
		{
			ctx.Database.Migrate();
		}
	}
}
