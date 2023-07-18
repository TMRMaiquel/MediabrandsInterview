using INT.Application.Authorization;
using INT.Application.Setup;
using INT.Cross.Cutting.IoC;

namespace INT.Distributed.Service
{
    public class Startup
    {
        #region Constructor

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Miembros

        public IConfiguration Configuration { get; }

        #endregion

        #region Métodos

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Se registra la configuración global de la aplicación
            var configurationSection = Configuration.GetSection(nameof(AppOptions));
            services.Configure<AppOptions>(configurationSection);

            //Se configura el CORS
            CORSSetup.ConfigurarCORS(services);

            //Se configura los controladores
            AppSetup.ConfigurarControladores(services);

            //Se configura la restriccion para que las peticiones del cliente siempre viajen por HTTPS
            AppSetup.ConfigurarHSTS(services, configurationSection);

            return IoCFactory.Instance.CurrentContainer.CreateServiceProvider(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //Incluimos seguridad en las cabeceras de las solicitudes HTTP
            //app.UseMiddleware<SecurityHeadMiddleware>();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(CORSSetup.PolicyCORS);
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #endregion
    }
}
