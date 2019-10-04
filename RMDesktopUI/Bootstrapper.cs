using AutoMapper;
using Caliburn.Micro;
using RM.WPF.Library.Api;
using RM.WPF.Library.Helpers;
using RM.WPF.Library.Models;
using RMDesktopUI.Helpers;
using RMDesktopUI.Models;
using RMDesktopUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RMDesktopUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();

            ConventionManager.AddElementConvention<PasswordBox>(
            PasswordBoxHelper.BoundPasswordProperty,
            "Password",
            "PasswordChanged");
        }

        protected override void Configure()
        {
            _container
                .Instance(_container)
                .Instance(ConfigureAutoMapper())
                .PerRequest<ISaleEndpoint, SaleEndpoint>()
                .PerRequest<IProductEndpoint, ProductEndpoint>();

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<IApiHelper, ApiHelper>()
                .Singleton<IConfigHelper, ConfigHelper>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>();

            GetType().Assembly.GetTypes()
                .Where(t => t.IsClass)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(t => _container.RegisterPerRequest(t, t.ToString(), t));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        private IMapper ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(x =>
            {
                x.CreateMap<ProductModel, ProductDisplayModel>();
                x.CreateMap<CartItemModel, CartItemDisplayModel>();
            });

            return config.CreateMapper();
        }
    }
}
