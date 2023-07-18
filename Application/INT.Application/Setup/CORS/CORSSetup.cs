using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace INT.Application.Setup
{
    public static class CORSSetup
    {
        public static string PolicyCORS = "policyCORS";

        public static void ConfigurarCORS(IServiceCollection services)
        {
            try
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(name: PolicyCORS,
                                      builder =>
                                      {
                                          builder.WithOrigins("http://localhost:4200")
                                          .AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                                      });
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
