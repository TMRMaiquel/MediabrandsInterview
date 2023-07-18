using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace INT.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .CaptureStartupErrors(true)
                   .ConfigureKestrel((context, options) =>
                   {
                       options.AddServerHeader = false;
                       options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
                       options.ConfigureHttpsDefaults(s => s.SslProtocols = System.Security.Authentication.SslProtocols.Tls12);
                   }).UseStartup<Startup>();
    }
}
