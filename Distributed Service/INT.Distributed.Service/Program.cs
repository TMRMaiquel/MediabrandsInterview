using Microsoft.AspNetCore;

namespace INT.Distributed.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddCommandLine(args)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseSetting("detailedErrors", "true")
                .CaptureStartupErrors(true)
                .UseConfiguration(config)
                .UseIISIntegration()
                .ConfigureKestrel((context, options) =>
                {
                    options.AddServerHeader = false;
                    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
                    options.ConfigureHttpsDefaults(s => s.SslProtocols = System.Security.Authentication.SslProtocols.Tls12);
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();
        }
    }
}
