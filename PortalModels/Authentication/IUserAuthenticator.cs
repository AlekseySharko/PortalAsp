using System.Threading.Tasks;

namespace PortalModels.Authentication
{
    public interface IUserAuthenticator
    {
        Task<LoginSuccessfulData> LogInOrReturnNull(AuthenticationUserData user);
        Task<GeneralResult> SignUp(AuthenticationUserData user);
        Task<bool> CheckRole(string userId, string userRole);
    }
}
