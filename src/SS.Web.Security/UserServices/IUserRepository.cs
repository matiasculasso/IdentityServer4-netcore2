using System.Collections.Generic;

namespace SS.Web.Security.UserServices
{
    public interface IUserRepository
    {
        bool ValidateCredentials(string username, string password);

        CustomUser FindBySubjectId(string subjectId);

        CustomUser FindByUsername(string username);

	    List<CustomUser> GetUsers();
    }
}
