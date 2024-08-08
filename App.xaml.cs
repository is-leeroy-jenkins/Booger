namespace Booger
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using CommunityToolkit.Mvvm.Input;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public partial class App : Application
    {
        private static readonly IHost host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(config =>
            {
                string path = Path.Combine(
                    FileSystemUtils.GetEntryPointFolder(),
                    GlobalValues.JsonConfigurationFilePath);

                config
                    .AddJsonFile(path, true, true)
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                services.AddSingleton<AppGlobalData>();
                services.AddSingleton<PageService>();
                services.AddSingleton<NoteService>();
                services.AddSingleton<ChatService>();
                services.AddSingleton<ChatPageService>();
                services.AddSingleton<ChatStorageService>();
                services.AddSingleton<ConfigurationService>();
                services.AddSingleton<SmoothScrollingService>();
                services.AddSingleton<TitleGenerationService>();

                services.AddSingleton<LanguageService>();
                services.AddSingleton<ColorModeService>();

                services.AddSingleton<AppWindow>();
                services.AddSingleton<MainPage>();
                services.AddSingleton<ConfigPage>();

                services.AddSingleton<AppWindowModel>();
                services.AddSingleton<MainPageModel>();
                services.AddSingleton<ConfigPageModel>();

                services.AddScoped<ChatPage>();
                services.AddScoped<ChatPageModel>();

                services.AddTransient<MarkdownWpfRenderer>();

                services.Configure<AppConfig>(o =>
                {
                    context.Configuration.Bind(o);
                });
            })
            .Build();

        public static T GetService<T>()
            where T : class
        {
            return (host.Services.GetService(typeof(T)) as T) ?? throw new Exception("Cannot find service of specified type");
        }

        protected override async void OnStartup(StartupEventArgs e)
        {

            if (!EnsureAppSingletion())
            {
                Application.Current.Shutdown();
                return;
            }

            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await host.StopAsync();

            host.Dispose();
        }

        public static string AppName => nameof(Booger);


        public static IRelayCommand ShowAppCommand =
            new RelayCommand(ShowApp);
        public static IRelayCommand HideAppCommand =
            new RelayCommand(HideApp);
        public static IRelayCommand CloseAppCommand =
            new RelayCommand(CloseApp);

        public static void ShowApp()
        {
            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
                return;

            mainWindow.Show();

            if (mainWindow.WindowState == WindowState.Minimized)
                mainWindow.WindowState = WindowState.Normal;

            if (!mainWindow.IsActive)
                mainWindow.Activate( );
        }

        public static void HideApp()
        {
            Window mainWindow = Application.Current.MainWindow;
            if (mainWindow == null)
                return;

            mainWindow.Hide( );
        }

        public static void CloseApp()
        {
            Application.Current.Shutdown( );
        }


        public bool EnsureAppSingletion( )
        {
            EventWaitHandle singletonEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "Booger", out bool createdNew);

            if (createdNew)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        singletonEvent.WaitOne();

                        Dispatcher.Invoke(() =>
                        {
                            ShowApp();
                        });
                    }
                });

                return true;
            }
            else
            {
                singletonEvent.Set();
                return false;
            }
        }
    }
}
