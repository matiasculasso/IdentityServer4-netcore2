using System.ComponentModel.DataAnnotations;

namespace SS.Web.Security.Entities
{
	public class SAMLIdentityProvider : ExternalIdentityProvider
	{
		protected SAMLIdentityProvider()
		{
		}

		public SAMLIdentityProvider(string authenticationScheme, string displayName) :
			base(AuthenticationProtocol.SAML, authenticationScheme, displayName)
		{
		}
			
		[Required]
		public string JsonConfiguration { get; set; }
	}
}