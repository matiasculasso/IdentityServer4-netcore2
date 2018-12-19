using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SS.Web.Security.Entities
{
	public abstract class ExternalIdentityProvider
	{		
		public int Id { get; set; }

		protected ExternalIdentityProvider()
		{
		}

		protected ExternalIdentityProvider(AuthenticationProtocol authenticationProtocol, string authenticationScheme, string displayName)
		{
			this.AuthenticationProtocol = authenticationProtocol;
			this.DisplayName = displayName;
			this.AuthenticationScheme = authenticationScheme;
		}

		[Required]
		protected AuthenticationProtocol AuthenticationProtocol { get; set; }

		[Required]
		[StringLength(50)]
		public string AuthenticationScheme { get; set; }

		[Required]
		[StringLength(200)]
		public string DisplayName { get; set; }

		[NotMapped]
		public AuthenticationProtocol Protocol => AuthenticationProtocol;

		/// <summary>
		/// The alternate ID source for this identity provider's user identifiers.
		/// </summary>
		[StringLength(450)]
		public string AlternateIdSourceKey { get; set; }

		/// <summary>
		/// Identifies the claim containing this identity provider's user identifers.
		/// </summary>
		[StringLength(512)]
		public string AlternateIdClaimType { get; set; }
	}
}