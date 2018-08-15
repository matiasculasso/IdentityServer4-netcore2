namespace SS.Web.Security.Entities
{
	public class LDAPIdentityProvider : ExternalIdentityProvider
	{
		protected LDAPIdentityProvider()
		{
		}

		public LDAPIdentityProvider(string authenticationScheme, string displayName) :
			base(AuthenticationProtocol.LDAP, authenticationScheme, displayName)
		{
		}
	}
}