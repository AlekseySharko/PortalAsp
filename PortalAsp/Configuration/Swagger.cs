using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace PortalAsp.Configuration
{
    public static class Swagger
    {
        public static void AddSwaggerV1(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Globy api",
                    Version = "v1"
                });
            });
        }
    }
}
