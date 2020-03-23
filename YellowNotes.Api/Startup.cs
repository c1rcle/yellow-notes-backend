using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YellowNotes.Api
{
   

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {    
            var allowedHosts = Configuration.GetSection("CORS-Settings:AllowedHosts").Get<string[]>();
            var allowedMethods = Configuration.GetSection("CORS-Settings:AllowedMethods").Get<string[]>();
            services.AddCors(options =>
            {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins(allowedHosts)
                                        .AllowAnyHeader()
                                        .WithMethods(allowedMethods);
                });
            });
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
