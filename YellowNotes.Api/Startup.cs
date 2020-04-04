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
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Text.Json;
using AutoMapper;
using YellowNotes.Core.Dtos;

namespace YellowNotes.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var keyListName = "DevelopmentKeys";
            if (Environment.IsProduction())
            {
                keyListName = "ProductionKeys";
            }

            var keyList = Configuration.GetSection(keyListName).Get<string[]>();

            var allowedHosts = Configuration.GetSection("CorsSettings:AllowedHosts")
                .Get<string[]>();
            var allowedMethods = Configuration.GetSection("CorsSettings:AllowedMethods")
                .Get<string[]>();

            services.AddAutoMapper(typeof(Mapping));

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

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateFilter));
                options.Filters.Add(typeof(ValidateTokenFilter)); 
                options.Filters.Add(new AuthorizeFilter());
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>(x =>
                ActivatorUtilities.CreateInstance<UserService>(x, keyList[0]));
            services.AddTransient<INoteRepository, NoteRepository>();
            services.AddTransient<INoteService, NoteService>();

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
                        .GetBytes(Configuration.GetValue<string>(keyList[0]))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            services.AddDbContextPool<DatabaseContext>(options =>
                options.UseMySql(Configuration.GetValue<string>(keyList[1])));

            var emailConfig = JsonSerializer.Deserialize<EmailConfiguration>(
                Configuration.GetValue<string>(keyList[2]));

            services.Configure<EmailConfiguration>(options =>
            {
                options.SmtpServer = emailConfig.SmtpServer;
                options.SmtpPort = emailConfig.SmtpPort;
                options.SmtpUsername = emailConfig.SmtpUsername;
                options.SmtpPassword = emailConfig.SmtpPassword;
            });
            services.AddSingleton<IEmailService, EmailService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
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
