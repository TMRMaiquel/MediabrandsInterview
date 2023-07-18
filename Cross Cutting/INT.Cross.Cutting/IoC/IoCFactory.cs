namespace INT.Cross.Cutting.IoC
{
    public sealed class IoCFactory
    {
        #region Singleton

        private static readonly IoCFactory instance = new IoCFactory();

        /// <summary>
        /// Consigue una instancia singleton de IoCFactory
        /// </summary>
        public static IoCFactory Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Consigue la actual configuración del IContainer
        /// <remarks>
        /// En este momento solo se configuro el contenedor para LightInject
        /// </remarks>
        /// </summary>
        public IoCLightInjectContainerBase CurrentContainer { get; }

        #endregion

        #region Constructor

        static IoCFactory() { }

        private IoCFactory()
        {
            CurrentContainer = new IoCLightInjectContainerBase();
        }

        #endregion
    }
}
