namespace SS.Web.Security.Configuration
{
    public class AppSettings
    {
		public bool RunDatabaseMigrations { get; set; }
		public bool SeedDatabase { get; set; }
		public string SigningCertificate { get; set; }
	}
}
