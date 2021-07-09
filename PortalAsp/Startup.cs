using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using PortalAsp.Configuration;
using PortalAsp.EfCore.Catalog;
using PortalAsp.Middleware;

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
            app.UseDeveloperExceptionPage();
            app.UseMiddleware<HttpsOnlyMiddleware>();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            if (env.IsDevelopment())
            {
                app.UseCors("Angular");
            }

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
