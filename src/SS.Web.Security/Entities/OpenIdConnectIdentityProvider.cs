using System.ComponentModel.DataAnnotations;

namespace SS.Web.Security.Entities
{
	public class OpenIdConnectIdentityProvider : ExternalIdentityProvider
	{
		protected OpenIdConnectIdentityProvider()
		{
		}

		public OpenIdConnectIdentityProvider(string authenticationScheme, string displayName) :
			base(AuthenticationProtocol.OpenIDConnect, authenticationScheme, displayName)
		{
		}

		[Required]
		[StringLength(200)]
		public string Authority { get; set; }

		[Required]
		[StringLength(50)]
		public string SignInScheme { get; set; }

		[Required]
		public bool RequireHttpsMetadata { get; set; }

		[Required]
		[StringLength(200)]
		public string ClientId { get; set; }

		[StringLength(100)]
		public string ClientSecretName { get; set; }

		[Required]
		public bool SaveTokens { get; set; }

		[Required]
		public bool? AllowClaimsFromUserInfoEndpoint { get; set; }

	}
}