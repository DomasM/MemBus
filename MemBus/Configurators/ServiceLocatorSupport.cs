using MemBus.Setup;
using Microsoft.Practices.ServiceLocation;

namespace MemBus.Configurators
{
    /// <summary>
    /// Add ServiceLocator Support
    /// </summary>
    public class ServiceLocatorSupport : ISetup<IConfigurableBus>
    {
        private readonly IServiceLocator locator;

        public ServiceLocatorSupport(IServiceLocator locator)
        {
            this.locator = locator;
        }

        public void Accept(IConfigurableBus setup)
        {
            setup.AddService(locator);
            setup.AddResolver(new ServiceLocationBasedResolver());
        }
    }
}