

namespace Booger
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    [ SuppressMessage( "ReSharper", "BadSquareBracketsSpaces" ) ]
    public class ApplicationHostService : IHostedService
    {
        public ApplicationHostService(
            IServiceProvider serviceProvider,
            ChatStorageService chatStorageService,
            ConfigurationService configurationService)
        {
            ServiceProvider = serviceProvider;
            ChatStorageService = chatStorageService;
            ConfigurationService = configurationService;
        }

        public IServiceProvider ServiceProvider { get; }
        public ChatStorageService ChatStorageService { get; }
        public ConfigurationService ConfigurationService { get; }



        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!File.Exists(GlobalValues.JsonConfigurationFilePath))
                ConfigurationService.Save();

            ChatStorageService.Initialize();

            MarkdownWpfRenderer.LinkNavigate += (s, e) =>
            {
                try
                {
                    if (e.Link != null)
                        Process.Start("Explorer.exe", new string[] { e.Link });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Cannot open link: {ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            if (!Application.Current.Windows.OfType<MainWindow>( ).Any( ) )
            {
                var _window = (MainWindow)ServiceProvider.GetService( typeof( MainWindow ) );
                _window?.Show( );
                _window?.Navigate<MainPage>( );
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            ChatStorageService.Dispose();

            return Task.CompletedTask;
        }
    }
}
