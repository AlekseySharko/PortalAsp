using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PortalAsp.EfCore.Catalog;

namespace PortalAsp
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "Angular",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200/").AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                    });
            });
            services.AddControllers().AddNewtonsoftJson();
            services.AddDbContext<CatalogContext>(opts =>
            {
                opts.UseSqlServer(Configuration["ConnectionStrings:CatalogConnection"]);
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseCors("Angular");
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }

        public Startup(IConfiguration config)
        {
            Configuration = config;
        }
    }
}
