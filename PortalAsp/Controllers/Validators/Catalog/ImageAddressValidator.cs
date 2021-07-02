using System;

namespace PortalAsp.Controllers.Validators.Catalog
{
    public class ImageAddressValidator
    {
        public static bool CheckExtension(string extension)
        {
            string[] permittedExtensions = { ".bmp", ".jpg", ".jpeg", ".gif", ".png", ".svg" };
            string res = Array.Find(permittedExtensions, el => el == extension.ToLower());
            if (String.IsNullOrWhiteSpace(res)) return false;
            return true;
        }
    }
}
