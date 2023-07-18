using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace INT.Presentation.Configuration
{
    public class AppConfiguration
    {
        #region Constructor

        public AppConfiguration() { }

        #endregion

        #region Miembros

        public static bool EnableHSTS { get; set; }

        #endregion

        #region Métodos

        public static void ConfigureHSTS(IServiceCollection services, IConfigurationSection configurationSection)
        {
            try
            {
                if (configurationSection != null)
                {
                    var appOptions = new AppOptions();
                    configurationSection.Bind(appOptions);

                    if (appOptions.Environment != null)
                    {
                        EnableHSTS = appOptions.Environment.EnableHSTS;

                        if (EnableHSTS)
                        {
                            services.AddHsts(opciones =>
                            {
                                opciones.Preload = true;
                                opciones.IncludeSubDomains = true;
                                opciones.MaxAge = TimeSpan.FromDays(365);
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
