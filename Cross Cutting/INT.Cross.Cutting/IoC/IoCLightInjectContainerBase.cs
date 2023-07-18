using INT.Application.Service;
using INT.Domain;
using INT.Infraestructure.Data;
using INT.Infraestructure.Data.UnitOfWork;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace INT.Cross.Cutting.IoC
{
    public class IoCLightInjectContainerBase : IContainerBase
    {
        #region Constructor

        public IoCLightInjectContainerBase()
        {
            ContainersDictionary = new Dictionary<string, ServiceContainer>();
            //Crea el contenedor raiz para (unidades de trabajo, servicios de dominio, servicios de aplicación u otros repositorios)
            this.ServiceContainer = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false })
            {
                ScopeManagerProvider = new PerLogicalCallContextScopeManagerProvider()
            };
            ContainersDictionary.Add("RootContainer", this.ServiceContainer);
            ConfigureContainer(this.ServiceContainer);
        }

        #endregion

        #region Miembros

        public IDictionary<string, ServiceContainer> ContainersDictionary { get; set; }

        public ServiceContainer ServiceContainer { get; set; }

        #endregion

        #region Métodos

        private void ConfigureContainer(ServiceContainer serviceBuilder)
        {
            //REGISTRAMOS LAS UNIDADES DE TRABAJO
            serviceBuilder.Register<IUnitOfWorkBase, UnitOfWorkBase>();
            serviceBuilder.Register<IUnitOfWorkAdmin, UnitOfWorkAdmin>();
            //REGISTRAMOS LOS SERVICIOS
            serviceBuilder.Register<IEmployeeService, EmployeeService>();
            serviceBuilder.Register<IUsuarioService, UsuarioService>();
        }

        public void RegisterType(Type type)
        {
            try
            {
                ServiceContainer rootContainer = this.ContainersDictionary["RootContainer"];

                if (rootContainer != null)
                {
                    rootContainer.Register(type);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TDependency Resolve<TDependency>()
        {
            try
            {
                ServiceContainer rootContainer = this.ContainersDictionary["RootContainer"];

                return rootContainer.GetInstance<TDependency>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TDependency Resolve<TDependency, P1>(P1 arg1)
        {
            try
            {
                ServiceContainer rootContainer = this.ContainersDictionary["RootContainer"];

                return rootContainer.GetInstance<P1, TDependency>(arg1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TDependency Resolve<TDependency, P1, P2>(P1 arg1, P2 arg2)
        {
            try
            {
                ServiceContainer rootContainer = this.ContainersDictionary["RootContainer"];

                return rootContainer.GetInstance<P1, P2, TDependency>(arg1, arg2);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TDependency Resolve<TDependency, P1, P2, P3>(P1 arg1, P2 arg2, P3 arg3)
        {
            try
            {
                ServiceContainer rootContainer = this.ContainersDictionary["RootContainer"];

                return rootContainer.GetInstance<P1, P2, P3, TDependency>(arg1, arg2, arg3);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TDependency Resolve<TDependency, P1, P2, P3, P4>(P1 arg1, P2 arg2, P3 arg3, P4 arg4)
        {
            try
            {
                ServiceContainer rootContainer = this.ContainersDictionary["RootContainer"];

                return rootContainer.GetInstance<P1, P2, P3, P4, TDependency>(arg1, arg2, arg3, arg4);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object Resolve(Type type)
        {
            try
            {
                ServiceContainer rootContainer = this.ContainersDictionary["RootContainer"];

                return rootContainer.GetInstance(type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IServiceProvider CreateServiceProvider(IServiceCollection services)
        {
            try
            {
                ServiceContainer rootContainer = this.ContainersDictionary["RootContainer"];
                return rootContainer.CreateServiceProvider(services);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
