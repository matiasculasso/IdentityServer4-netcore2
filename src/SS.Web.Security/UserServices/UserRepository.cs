using System.Collections.Generic;
using System.Linq;
using System;

namespace SS.Web.Security.UserServices
{
    public class UserRepository : IUserRepository
    {
        // some dummy data. Replce this with your user persistence. 
        private readonly List<CustomUser> _users = new List<CustomUser>
        {
	        new CustomUser{
		        SubjectId = "111111",
		        UserName = "admin",
		        Password = "Admin123.",
		        Email = "admin@seaburysolutions.com"
	        },
	        new CustomUser{
		        SubjectId = "222222",
		        UserName = "bob",
		        Password = "bob",
		        Email = "bob@test.com"
	        },
		};

		public List<CustomUser> GetUsers()
		{
			return _users;
		}

		public bool ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);
            if (user != null)
            {
                return user.Password.Equals(password);
            }

            return false;
        }

        public CustomUser FindBySubjectId(string subjectId)
        {
            return _users.FirstOrDefault(x => x.SubjectId == subjectId);
        }

        public CustomUser FindByUsername(string username)
        {
            return _users.FirstOrDefault(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
