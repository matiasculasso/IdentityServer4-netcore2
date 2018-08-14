using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace SS.Web.Security.UserServices
{
    public class CustomProfileService : IProfileService
    {
        protected readonly ILogger Logger;


        protected readonly IUserRepository _userRepository;

        public CustomProfileService(IUserRepository userRepository, ILogger<CustomProfileService> logger)
        {
            _userRepository = userRepository;
            Logger = logger;
        }


        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            Logger.LogDebug("Get profile called for subject {subject} from client {client} with claim types {claimTypes} via {caller}",
	            sub,
                context.Client.ClientName ?? context.Client.ClientId,
                context.RequestedClaimTypes,
                context.Caller);

            var user = _userRepository.FindBySubjectId(context.Subject.GetSubjectId());

			//add custom claims here
            var claims = new List<Claim>
            {
                new Claim("role", "sysadmin"),
                new Claim("role", "manager"),
                new Claim("username", user.UserName),
                new Claim("email", user.Email)
            };

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = _userRepository.FindBySubjectId(sub);
            context.IsActive = user != null;
        }
    }
}