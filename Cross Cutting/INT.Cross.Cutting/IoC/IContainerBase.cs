using LightInject;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace INT.Cross.Cutting.IoC
{
    /// <summary>
    /// Contrato base para localizar y registrar las dependencies
    /// </summary>
    public interface IContainerBase
    {
        /// <summary>
        /// Resuelve con apoyo de LigthInject la instanciación de la dependencia.
        /// </summary>
        TDependency Resolve<TDependency>();
        TDependency Resolve<TDependency, P1>(P1 arg1);
        TDependency Resolve<TDependency, P1, P2>(P1 arg1, P2 arg2);
        TDependency Resolve<TDependency, P1, P2, P3>(P1 arg1, P2 arg2, P3 arg3);
        TDependency Resolve<TDependency, P1, P2, P3, P4>(P1 arg1, P2 arg2, P3 arg3, P4 arg4);

        /// <summary>
        /// Resuelve la instanciación a través del tipo que tiene la dependencia.
        /// </summary>
        object Resolve(Type type);

        /// <summary>
        /// Registra la dependencia dentro en un servicio que se encargue de su alojamiento.
        /// </summary>
        void RegisterType(Type type);
    }
}
