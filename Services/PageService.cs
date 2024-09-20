
namespace Booger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public class PageService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageService"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public PageService( IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <value>
        /// The service provider.
        /// </value>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the page.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public FrameworkElement GetPage( Type type )
        {
            return ServiceProvider.GetService( type ) as FrameworkElement;
        }
    }
}
