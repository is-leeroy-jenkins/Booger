
namespace Booger
{
    using System;
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;

    public class PageService
    {
        public PageService(
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        
        public T GetPage<T>()
            where T : class
        {
            return ServiceProvider.GetService<T>() ?? throw new InvalidOperationException("Cannot find specified Page service");
        }

        public FrameworkElement GetPage(Type type)
        {
            return ServiceProvider.GetService(type) as FrameworkElement;
        }
    }
}
