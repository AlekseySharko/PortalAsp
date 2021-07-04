using PortalModels;
using PortalModels.Authentication;
using System.Threading.Tasks;

namespace PortalAsp.EfCore.Identity
{
    public class EfCoreUserAuthenticator : IUserAuthenticator
    {
        public async Task<bool> LogIn(AuthenticationUserData user)
        {
            return false;
        }

        public async Task<GeneralResult> SignUp(AuthenticationUserData user)
        {
            return new GeneralResult
            {
                Success = true
            };
        }
    }
}
