using System.Text.RegularExpressions;
using PortalModels.Authentication;

namespace PortalModels.Validators.Authentication
{
    public class AuthenticationValidator
    {
        public static ValidationResult ValidateOnSignUp(AuthenticationUserData user)
        {
            ValidationResult result = new ValidationResult
            {
                IsValid = true,
                Message = ""
            };

            if (user is null)
            {
                result.IsValid = false;
                result.Message += "User is null";
                return result;
            }

            if (string.IsNullOrWhiteSpace(user.UserName) || user.UserName.Length < 5 || user.UserName.Length > 30)
            {
                result.IsValid = false;
                result.Message += "UserName must be 5-30 characters long. ";
            }

            if (user.UserName is null || Regex.Match(user.UserName, ".* +.*|.*'+.*|.*\"+.*", RegexOptions.ECMAScript).Success)
            {
                result.IsValid = false;
                result.Message += "UserName can't contain spaces and quotes(\",')";
            }

            if (!IsValidEmail(user.Email))
            {
                result.IsValid = false;
                result.Message += "Email is invalid";
            }

            return result;
        }

        public static ValidationResult ValidateOnLogIn(AuthenticationUserData user)
        {
            ValidationResult result = new ValidationResult
            {
                IsValid = true,
                Message = ""
            };

            if (user == null)
            {
                result.IsValid = false;
                result.Message += "User is null";
                return result;
            }

            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.Password))
            {
                result.IsValid = false;
                result.Message += "Wrong login or password. ";
            }

            return result;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
