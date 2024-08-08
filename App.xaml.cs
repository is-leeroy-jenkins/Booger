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
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(config =>
            {
                string _path = Path.Combine(
                    FileSystemUtils.GetEntryPointFolder(),
                    GlobalValues.JsonConfigurationFilePath);

                config
                    .AddJsonFile(_path, true, true)
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
            return (App._host.Services.GetService(typeof(T)) as T) ?? throw new Exception("Cannot find service of specified type");
        }

        protected override async void OnStartup(StartupEventArgs e)
        {

            if (!EnsureAppSingletion())
            {
                Application.Current.Shutdown();
                return;
            }

            await App._host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await App._host.StopAsync();

            App._host.Dispose();
        }

        public static string AppName => nameof(Booger);


        public static IRelayCommand _showAppCommand =
            new RelayCommand(ShowApp);
        public static IRelayCommand _hideAppCommand =
            new RelayCommand(HideApp);
        public static IRelayCommand _closeAppCommand =
            new RelayCommand(CloseApp);

        public static void ShowApp()
        {
            Window _mainWindow = Application.Current.MainWindow;
            if (_mainWindow == null)
                return;

            _mainWindow.Show();

            if (_mainWindow.WindowState == WindowState.Minimized)
                _mainWindow.WindowState = WindowState.Normal;

            if (!_mainWindow.IsActive)
                _mainWindow.Activate( );
        }

        public static void HideApp()
        {
            Window _mainWindow = Application.Current.MainWindow;
            if (_mainWindow == null)
                return;

            _mainWindow.Hide( );
        }

        public static void CloseApp()
        {
            Application.Current.Shutdown( );
        }


        public bool EnsureAppSingletion( )
        {
            EventWaitHandle _singletonEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "Booger", out bool _createdNew);

            if (_createdNew)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                        _singletonEvent.WaitOne();

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
                _singletonEvent.Set();
                return false;
            }
        }
    }
}
