using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PortalAsp.Core.Configuration;
using PortalAsp.Core.Middleware;
using PortalAsp.EfCore.Catalog;

namespace PortalAsp
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CatalogContext>(opts =>
            {
                opts.UseSqlServer(Configuration["ConnectionStrings:CatalogConnection"]);
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: "Angular",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
                    });
            });
            services.AddEfCatalogRepositories();
            services.AddEfIdentityAndAuthentication(Configuration);
            services.AddSwaggerV1();
            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<HttpsOnlyMiddleware>();
            app.UseStaticFiles();
            app.UseRouting();
            if (env.IsDevelopment())
            {
                app.UseCors("Angular");
            }
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("", async context =>
                {
                    await Task.CompletedTask;
                    context.Response.Redirect("/swagger");
                }); //Temporary swagger redirect
            });
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Globy Api");
            });
        }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
    }
}
