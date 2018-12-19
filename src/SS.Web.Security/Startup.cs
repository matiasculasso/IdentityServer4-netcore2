using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SS.Web.Security.Configuration;
using SS.Web.Security.Data;
using System.Reflection;

namespace SS.Web.Security
{
	public class Startup
	{
		private IConfigurationRoot Configuration { get; }

		private readonly AppSettings _appSettings;

		public Startup(ILoggerFactory loggerFactory, IHostingEnvironment env)
		{
			Configuration = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			_appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
			services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

			var connectionString = Configuration.GetConnectionString("DefaultConnection");
			var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

			services.AddDbContext<SSDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly)));
				
			services.ConfigureIdentityServer(connectionString, _appSettings);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();			
			}

			if (_appSettings.RunDatabaseMigrations)
			{
				RunMigrations(app);
			}

			if (_appSettings.SeedDatabase)
			{
				SeedData.EnsureSeedData(app.ApplicationServices);
			}

			app.UseIdentityServer();
			app.UseStaticFiles();
			app.UseMvcWithDefaultRoute();
		}

		private static void RunMigrations(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				var configContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
				configContext.Database.Migrate();

				var persistedGrantDbContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
				persistedGrantDbContext.Database.Migrate();

				var ssGrantDbContext = serviceScope.ServiceProvider.GetRequiredService<SSDbContext>();
				ssGrantDbContext.Database.Migrate();
			}
		}
	}
}