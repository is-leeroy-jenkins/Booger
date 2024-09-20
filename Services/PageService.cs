
namespace Booger
{
    using System;
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;

    public class PageService
    {
        public PageService( IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public FrameworkElement GetPage( Type type )
        {
            return ServiceProvider.GetService( type ) as FrameworkElement;
        }
    }
}
