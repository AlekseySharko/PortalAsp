using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortalAsp.EfCore.Identity;
using PortalModels.Authentication;

namespace PortalAsp.Core.Configuration
{
    public static class IdentityConfigurator
    {
        public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDataContext>(opts =>
                opts.UseSqlServer(configuration[
                    "ConnectionStrings:IdentityConnection"]));

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>();
            services.Configure<IdentityOptions>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
            });

            services.AddScoped<IUserAuthenticator, EfCoreUserAuthenticator>();
        }
    }
}
