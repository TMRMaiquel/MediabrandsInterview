using System.IO;
using INT.Presentation.Authorization;
using INT.Presentation.Configuration;
using INT.Presentation.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace INT.Presentation
{
    public class Startup
    {
        #region Constructor

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion

        #region Miembros

        public IConfiguration Configuration { get; }

        #endregion

        #region Métodos

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Se registra la configuración global de la aplicación
            var configurationSection = Configuration.GetSection(nameof(AppOptions));
            services.Configure<AppOptions>(configurationSection);

            //Se configura la restriccion para que las peticiones del cliente siempre viajen por HTTPS
            AppConfiguration.ConfigureHSTS(services, configurationSection);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Starts "npm start" command using Shell extension.
                //app.Shell("npm start");
                app.Shell("node --max_old_space_size=8048 ./node_modules/@angular/cli/bin/ng serve");
            }
            else
            {
                if (AppConfiguration.EnableHSTS)
                {
                    app.UseHsts();
                }
            }

            //Incluimos seguridad en las cabeceras de las solicitudes HTTP
            app.UseMiddleware<SecurityHeadMiddleware>();

            // Microsoft.AspNetCore.StaticFiles: API for starting the application from wwwroot.
            // Uses default files as index.html.
            app.UseDefaultFiles();
            // Uses static file for the current path.
            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //La aplicación trabaja directamente con los archivos estaticos generados por angular, de tal forma que el servidor que aloja estos archivos (kestrel)
            //no tendrá conocimiento de esas rutas y cuando se refresque la página devovlerá errores de tipo 404 (No encuentra la página solicitada).
            //Por tal motivo debe combinar las rutas solicitadas generadas por los archivos estáticos con su propia estructura de rutas.
            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            });
        }

        #endregion
    }
}
