using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace INT.Application.Setup
{
    public class AppSetup
    {
        #region Constructor

        public AppSetup() { }

        #endregion

        #region Miembros

        public static bool EnableHSTS { get; set; }

        #endregion

        #region Métodos

        public static void ConfigurarControladores(IServiceCollection services)
        {
            try
            {
                services.AddControllers()
                    .AddControllersAsServices()
                    .AddNewtonsoftJson(jsnOpt =>
                    {
                        jsnOpt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        jsnOpt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ConfigurarHSTS(IServiceCollection services, IConfigurationSection configurationSection)
        {
            try
            {
                if (configurationSection != null)
                {
                    var appOpciones = new AppOptions();
                    configurationSection.Bind(appOpciones);

                    if (appOpciones.Environment != null)
                    {
                        EnableHSTS = appOpciones.Environment.EnableHSTS;

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
