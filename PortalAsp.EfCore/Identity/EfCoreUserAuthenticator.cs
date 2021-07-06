using PortalModels;
using PortalModels.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PortalAsp.EfCore.Identity.JwtAuth;

namespace PortalAsp.EfCore.Identity
{
    public class EfCoreUserAuthenticator : IUserAuthenticator
    {

        private UserManager<IdentityUser> UserManager { get; }
        private SignInManager<IdentityUser> SignInManager { get; }
        private IJwtGenerator JwtGenerator { get; }

        public EfCoreUserAuthenticator(UserManager<IdentityUser> usrManager, SignInManager<IdentityUser> signInManager, IJwtGenerator jwtGenerator)
        {
            UserManager = usrManager;
            SignInManager = signInManager;
            JwtGenerator = jwtGenerator;
        }

        public async Task<LoginSuccessfulData> LogInOrReturnNull(AuthenticationUserData authUser)
        {
            var newUser = await UserManager.FindByNameAsync(authUser.UserName);
            if (newUser == null)
            {
                return null;
            }

            var result = await SignInManager.CheckPasswordSignInAsync(newUser, authUser.Password, false);
            if (result.Succeeded)
            {
                return new LoginSuccessfulData
                {
                    UserName = newUser.UserName,
                    Image = null,
                    Token = JwtGenerator.CreateToken(newUser.Id)
                };
            }

            return null;
        }

        public async Task<GeneralResult> SignUp(AuthenticationUserData singUpData)
        {
            IdentityUser newUser =
                new IdentityUser { UserName = singUpData.UserName, Email = singUpData.Email };
            IdentityResult newUserResult =
                await UserManager.CreateAsync(newUser, singUpData.Password);

            if (newUserResult.Succeeded)
            {
                return new GeneralResult {Success = true};
            }
            GeneralResult returnResult = new GeneralResult{Success = false, Message = ""};
            foreach (IdentityError error in newUserResult.Errors)
            {
                returnResult.Message += error.Description + " ";
            }
            return returnResult;
        }

        public async Task<bool> CheckRole(string userId, string userRole)
        {
            if (userId is null || userRole is null)
                return false;

            IdentityUser user = await UserManager.FindByIdAsync(userId);
            if (await UserManager.IsInRoleAsync(user, userRole))
                return true;

            return false;
        }
    }
}
