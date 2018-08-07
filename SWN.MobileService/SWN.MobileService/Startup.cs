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
using PatientPortalService.Api.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using System.Net.Http;
using System.Text;
using PatientPortalService.Api.Data;

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
            services.AddMvc();

            services.AddSingleton<IMessageConsumerService, MessageConsumerService>();
            //services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<Func<HttpClient>>
            (
               () => new HttpClient()
            );
            services.AddDbContext<PatientPortalContext>(options =>
                                    options.UseSqlServer(Configuration.GetConnectionString("MobileServiceDatabase")));
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
    }
}
