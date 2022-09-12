using Architecture.Models;

namespace Architecture.Service
{
    public interface IUserService
    {
        bool IsValidUserInformation(LoginModel model);
    }
}
