using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YellowNotes.Api
{
    public class AllowedHosts
    {
        public string[] allowedHost {get; set; }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AllowedHosts allowed;
            using (FileStream fs = File.OpenRead("appsettngs.json"))
                {
                allowed = await JsonSerializer.DeserializeAsync<AllowedHosts>(fs);
                }

            services.AddCors(options =>
            {
            options.AddDefaultPolicy(
                builder =>
                {
                   
                    builder.WithOrigins(allowed.allowedHost);
                });

            options.AddPolicy("AnotherPolicy",
                builder =>
                {
                    builder.WithOrigins(allowed.allowedHost)
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
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
