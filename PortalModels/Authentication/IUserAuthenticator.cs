using System.Threading.Tasks;

namespace PortalModels.Authentication
{
    public interface IUserAuthenticator
    {
        Task<LoginSuccessfulData> LogInOrReturnNullAsync(AuthenticationUserData user);
        Task<GeneralResult> SignUpAsync(AuthenticationUserData user);
        Task<bool> CheckRoleAsync(string userId, string userRole);
    }
}
