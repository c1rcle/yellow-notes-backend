using System.Data;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YellowNotes.Core;
using YellowNotes.Core.Email;
using YellowNotes.Core.Services;

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
            var allowedHosts = Configuration.GetSection("CorsSettings:AllowedHosts").Get<string[]>();
            var allowedMethods = Configuration.GetSection("CorsSettings:AllowedMethods").Get<string[]>();
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
            services.AddDbContextPool<DatabaseContext>(options => 
            options.UseMySql(Configuration.GetValue<string>("ConnectionString")));
            
            services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfiguration"));
            services.AddSingleton<IEmailService, EmailService>();
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
