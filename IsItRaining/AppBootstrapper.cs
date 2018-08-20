using Caliburn.Micro;
using IsItRaining.Pages;
using IsItRaining.Services;
using System;
using System.Collections.Generic;
using System.Windows;

namespace IsItRaining
{
    public class AppBootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer _container = new SimpleContainer();

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);

            RegisterServices();

            DisplayRootViewFor<ShellViewModel>();
        }

        private void RegisterServices()
        {
            _container.PerRequest<HomePageViewModel>();

            _container.Singleton<IGpsLocatorService, DeviceGpsLocatorService>();
            _container.Singleton<IWeatherService, OpenWeatherMapService>();
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var instance = _container.GetInstance(serviceType, key);
            if (instance == null)
            {
                return base.GetInstance(serviceType, key);
            }

            return instance;
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetAllInstances(serviceType);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
