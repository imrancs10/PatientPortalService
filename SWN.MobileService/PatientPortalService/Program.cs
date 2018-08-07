using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using PatientPortalService.Api.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SWN.MobileService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isService = true;

            if (Environment.UserInteractive || Debugger.IsAttached)
            {
                isService = false;
            }

            var pathToContentRoot = Directory.GetCurrentDirectory();

            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                pathToContentRoot = Path.GetDirectoryName(pathToExe);
            }

            var config = new ConfigurationBuilder()
                      .SetBasePath(pathToContentRoot)
                      .AddJsonFile("hosting.json", optional: false, reloadOnChange: true)
                      .Build();

            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                 .UseContentRoot(pathToContentRoot)
                 .UseConfiguration(config)
                 .UseStartup<Startup>();

            var host = webHostBuilder.Build();
            StartMessageConsumerService(host);

            if (Environment.UserInteractive)
            {
                host.Run();
            }
            else
            {
                host.RunAsService();
            }
        }
        
        private static void StartMessageConsumerService(IWebHost host)
        {
            var service = host.Services.GetRequiredService<IMessageConsumerService>();
            service.ConsumeMessages();
        }
    }
}
