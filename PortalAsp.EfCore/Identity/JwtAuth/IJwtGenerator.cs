using Microsoft.AspNetCore.Identity;

namespace PortalAsp.EfCore.Identity.JwtAuth
{
    public interface IJwtGenerator
    {
        string CreateToken(string id);
    }
}
