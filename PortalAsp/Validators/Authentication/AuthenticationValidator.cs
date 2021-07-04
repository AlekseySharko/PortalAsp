using System.Text.RegularExpressions;
using PortalModels.Authentication;

namespace PortalAsp.Validators.Authentication
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

            if (string.IsNullOrWhiteSpace(user.Login) || user.Login.Length < 5 || user.Login.Length > 30)
            {
                result.IsValid = false;
                result.Message += "Login must be 5-30 characters long. ";
            }

            if (user.Login is null || Regex.Match(user.Login, ".* +.*|.*'+.*|.*\"+.*", RegexOptions.ECMAScript).Success)
            {
                result.IsValid = false;
                result.Message += "Login can't contain spaces and quotes(\",')";
            }

            if (string.IsNullOrWhiteSpace(user.Password) ||
                user.Password.Length < 8 ||
                user.Password.Length > 30 ||
                !Regex.Match(user.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", RegexOptions.ECMAScript).Success)
            {
                result.IsValid = false;
                result.Message +=
                    "Password must be 8-30 characters long and contain at least 1 upper case character, 1 lower case character and one digit. ";
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

            if (string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.Password))
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
