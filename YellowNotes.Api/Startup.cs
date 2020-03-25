using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using YellowNotes.Core;
using YellowNotes.Core.Repositories;
using YellowNotes.Core.Email;
using YellowNotes.Core.Services;
using YellowNotes.Api.Filters;
using Microsoft.AspNetCore.HttpOverrides;

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
            var allowedHosts = Configuration.GetSection("CorsSettings:AllowedHosts")
                .Get<string[]>();
            var allowedMethods = Configuration.GetSection("CorsSettings:AllowedMethods")
                .Get<string[]>();
                
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
            services.AddMvcCore(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateFilter));
            })
            .AddApiExplorer()
            .AddDataAnnotations();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                        .GetBytes(Configuration.GetValue<string>("JwtSecretProd"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            services.AddDbContextPool<DatabaseContext>(options =>
                options.UseMySql(Configuration.GetValue<string>("ConnectionStringProd")));

            services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfigurationProd"));
            services.AddSingleton<IEmailService, EmailService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto
            });
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
