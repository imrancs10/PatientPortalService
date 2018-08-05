using AsyncPoco;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;
using SWN.Messaging.Msmq;
using PatientPortalService.Api.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using System.Net.Http;
using System.Text;

namespace SWN.MobileService
{
    public class Startup
    {
        const string QueuePath = "MessageQueuePath:OutboundTextQueue";
        const string statusQueueConfig = "MessageQueuePath:StatusQueue";
        const string FirebaseServerKey = "FirebaseServerKey";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                         .ReadFrom.Configuration(Configuration)
                         .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            AddJwtAuthentication(services);
            services.AddMvc();

            services.AddSingleton<IMessageConsumerService, MessageConsumerService>();
            //services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<Func<HttpClient>>
            (
               () => new HttpClient()
            );

            var dbConnectionString = Configuration.GetValue<string>("Swn402Database:ConnectionString");
            var dbProviderName = Configuration.GetValue<string>("Swn402Database:ProviderName");
            var dbTimeout = Configuration.GetValue<int>("Swn402Database:DbQueryTimeout");
            services.AddSingleton<IDatabase>(_ => new Database(dbConnectionString, dbProviderName) { CommandTimeout = dbTimeout });
            AddSwagger(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
            app.UseAuthentication();
            app.UseMvc();
            ConfigureSwagger(app);
        }

        private void AddSwagger(IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "SWN Mobile Service",
                    Version = "v1",
                    Description = "Api(s) & Background services for handling mobile messages."
                });

                var xmlFile = "XmlDocumentation.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });
        }

        private void ConfigureSwagger(IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SWN Mobile Service V1");
            });
        }

        private void AddJwtAuthentication(IServiceCollection services)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>("JwtToken:Secret")));
            var clockSkew = TimeSpan.FromSeconds(Configuration.GetValue<int>("JwtToken:ClockSkew"));
            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = signingKey,
                ClockSkew = clockSkew,
                RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                AuthenticationType = "JWT",
                ValidateAudience = false,
                ValidateIssuer = false
            };

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = tokenValidationParameters;
            });
        }
    }
}
