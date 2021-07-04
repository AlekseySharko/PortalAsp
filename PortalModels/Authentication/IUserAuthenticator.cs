using System.Threading.Tasks;

namespace PortalModels.Authentication
{
    public interface IUserAuthenticator
    {
        Task<bool> LogIn(AuthenticationUserData user);
        Task<GeneralResult> SignUp(AuthenticationUserData user);
    }
}
