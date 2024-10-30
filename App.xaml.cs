// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="App.xaml.cs" company="Terry D. Eppler">
//    Booger is a quick & dirty WPF application that interacts with OpenAI GPT-3.5 Turbo API
//    based on NET6 and written in C-Sharp.
// 
//    Copyright ©  2024  Terry D. Eppler
// 
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the “Software”),
//    to deal in the Software without restriction,
//    including without limitation the rights to use,
//    copy, modify, merge, publish, distribute, sublicense,
//    and/or sell copies of the Software,
//    and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
// 
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
// 
//    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//    INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT.
//    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//    DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//    DEALINGS IN THE SOFTWARE.
// 
//    You can contact me at: terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   App.xaml.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using CommunityToolkit.Mvvm.Input;
    using System.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using RestoreWindowPlace;
    using Syncfusion.Licensing;
    using ConfigurationManager = System.Configuration.ConfigurationManager;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.Application" />
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    public partial class App : Application
    {
        /// <summary>
        /// The window place
        /// </summary>
        private static WindowPlace _windowPlace;

        /// <summary>
        /// The controls
        /// </summary>
        public static string[ ] Controls =
        {
            "ComboBoxAdv",
            "MetroComboBox",
            "MetroDatagrid",
            "ToolBarAdv",
            "ToolStrip",
            "MetroCalendar",
            "CalendarEdit",
            "PivotGridControl",
            "MetroPivotGrid",
            "MetroMap",
            "EditControl",
            "CheckListBox",
            "MetroEditor",
            "DropDownButtonAdv",
            "MetroDropDown",
            "GridControl",
            "MetroGridControl",
            "TabControlExt",
            "MetroTabControl",
            "MetroTextInput",
            "MenuItemAdv",
            "ButtonAdv",
            "Carousel",
            "ColorEdit",
            "SfChart",
            "SfChart3D",
            "SfHeatMap",
            "SfMap",
            "SfDataGrid",
            "SfTextBoxExt",
            "SfCircularProgressBar",
            "SfLinearProgressBar",
            "SfTextInputLayout",
            "SfSpreadsheet",
            "SfSpreadsheetRibbon",
            "SfCalculator",
            "SfMultiColumnDropDownControl"
        };

        /// <summary>
        /// The host
        /// </summary>
        private static readonly IHost _host = Host
            .CreateDefaultBuilder( )
            .ConfigureAppConfiguration( config =>
            {
                var _path = Path.Combine( FileSystemUtils.GetEntryPointFolder( ),
                    GlobalValues.JsonConfigurationFilePath );

                config
                    .AddJsonFile( _path, true, true )
                    .AddEnvironmentVariables( );
            } )
            .ConfigureServices( ( context, services ) =>
            {
                services.AddHostedService<ApplicationHostService>( );
                services.AddSingleton<AppGlobalData>( );
                services.AddSingleton<PageService>( );
                services.AddSingleton<NoteService>( );
                services.AddSingleton<ChatService>( );
                services.AddSingleton<ChatPageService>( );
                services.AddSingleton<ChatStorageService>( );
                services.AddSingleton<ConfigurationService>( );
                services.AddSingleton<SmoothScrollingService>( );
                services.AddSingleton<TitleGenerationService>( );
                services.AddSingleton<LanguageService>( );
                services.AddSingleton<ColorModeService>( );
                services.AddSingleton<MainWindow>( );
                services.AddSingleton<MainPage>( );
                services.AddSingleton<ConfigurationPage>( );
                services.AddSingleton<AppWindowModel>( );
                services.AddSingleton<MainPageModel>( );
                services.AddSingleton<ConfigPageModel>( );
                services.AddScoped<ChatPage>( );
                services.AddScoped<ChatPageModel>( );
                services.AddTransient<MarkdownWpfRenderer>( );
                services.Configure<AppConfig>( o =>
                {
                    context.Configuration.Bind( o );
                } );
            } )
            .Build( );

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Cannot find service of specified type</exception>
        public static T GetService<T>( )
            where T : class
        {
            return _host.Services.GetService( typeof( T ) ) as T
                ?? throw new Exception( "Cannot find service of specified type" );
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" />
        /// that contains the event data.</param>
        protected override async void OnStartup( StartupEventArgs e )
        {
            if( !EnsureAppSingletion( ) )
            {
                Current.Shutdown( );
                return;
            }

            var _key = ConfigurationManager.AppSettings[ "UI" ];
            SyncfusionLicenseProvider.RegisterLicense( _key );
            await _host.StartAsync( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" />
        /// that contains the event data.</param>
        protected override async void OnExit( ExitEventArgs e )
        {
            await _host.StopAsync( );
            _windowPlace?.Save( );
            _host.Dispose( );
        }

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        public static string AppName
        {
            get
            {
                return nameof( Booger );
            }
        }

        /// <summary>
        /// The show application command
        /// </summary>
        public static IRelayCommand _showAppCommand =
            new RelayCommand( App.ShowApp );

        /// <summary>
        /// The hide application command
        /// </summary>
        public static IRelayCommand _hideAppCommand =
            new RelayCommand( App.HideApp );

        /// <summary>
        /// The close application command
        /// </summary>
        public static IRelayCommand _closeAppCommand =
            new RelayCommand( App.CloseApp );

        /// <summary>
        /// Setups the restore window place.
        /// </summary>
        /// <param name="mainWindow">The main window.</param>
        private static void SetupRestoreWindowPlace( Window mainWindow )
        {
            var _windowPlaceConfigFilePath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory,
                "Booger.config" );

            _windowPlace = new WindowPlace( _windowPlaceConfigFilePath );
            _windowPlace.Register( mainWindow );
        }

        /// <summary>
        /// Shows the application.
        /// </summary>
        public static void ShowApp( )
        {
            var _mainWindow = Current.MainWindow;
            if( _mainWindow == null )
            {
                return;
            }

            _mainWindow.Show( );
            if( _mainWindow.WindowState == WindowState.Minimized )
            {
                _mainWindow.WindowState = WindowState.Normal;
            }

            if( !_mainWindow.IsActive )
            {
                _mainWindow.Activate( );
            }

            App.SetupRestoreWindowPlace( _mainWindow );
        }

        /// <summary>
        /// Hides the application.
        /// </summary>
        public static void HideApp( )
        {
            var _mainWindow = Current.MainWindow;
            if( _mainWindow == null )
            {
                return;
            }

            _windowPlace?.Save( );
            _mainWindow.Hide( );
        }

        /// <summary>
        /// Closes the application.
        /// </summary>
        public static void CloseApp( )
        {
            _windowPlace?.Save( );
            Current.Shutdown( );
        }

        /// <summary>
        /// Ensures the application singletion.
        /// </summary>
        /// <returns>
        /// </returns>
        public bool EnsureAppSingletion( )
        {
            var _singletonEvent = new EventWaitHandle( false, EventResetMode.AutoReset, "Booger",
                out var _createdNew );

            if( _createdNew )
            {
                Task.Run( ( ) =>
                {
                    while( true )
                    {
                        _singletonEvent.WaitOne( );
                        Dispatcher.Invoke( ( ) =>
                        {
                            App.ShowApp( );
                        } );
                    }
                } );

                return true;
            }
            else
            {
                _singletonEvent.Set( );
                return false;
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected void Fail( Exception ex )
        {
            var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}
