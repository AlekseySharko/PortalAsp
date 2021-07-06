using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PortalAsp.EfCore.Identity.JwtAuth;

namespace PortalAsp.Configuration
{
    public static class AuthenticationConfigurator
    {
        public static void AddEfAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureIdentity(configuration);
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "https://localhost:5000",
                    ValidAudiences = new List<string>
                    {
                        "https://localhost:5000",
                        "http://localhost:4200"
                    },
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[
                        "Authentication:JwtKey"]))
                };
            });
        }
    }
}
