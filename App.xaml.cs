namespace Booger
{
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using RestoreWindowPlace;
    using Syncfusion.Licensing;
    using Syncfusion.SfSkinManager;
    using Syncfusion.Themes.FluentDark.WPF;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.Application" />
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    public partial class App : Application
    {
        /// <summary>
        /// The window place
        /// </summary>
        private WindowPlace _windowPlace;

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

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A
        /// <see cref="T:System.Windows.StartupEventArgs" />
        /// that contains the event data.</param>
        protected override void OnStartup( StartupEventArgs e )
        {
            base.OnStartup( e );
            try
            {
                var _key = ConfigurationManager.AppSettings[ "UI" ];
                SyncfusionLicenseProvider.RegisterLicense( _key );

                // TODO 1: See README.md and get your OpenAI API key: https://platform.openai.com/account/api-keys
                // TODO 2: You can modify ChatViewModel to set default selected language: _selectedLang = LangList[..]
                // TODO 3: Give my article 5 stars:) at https://www.codeproject.com/Tips/5377103/ChatGPT-API-in-Csharp-WPF-XAML-MVVM
                string _openaiApiKey;
                if( e.Args?.Length > 0
                    && e.Args[ 0 ].StartsWith( '/' ) )
                {
                    // OpenAI API key from command line parameter such as "/sk-Ih...WPd" after removing '/'
                    _openaiApiKey = e.Args[ 0 ].Remove( 0, 1 );
                }
                else
                {
                    // Put your key from above here instead of using a command line parameter in the 'if' block
                    _openaiApiKey = _key;
                }

                // Programmatically switch between SqlHistoryRepo and EmptyHistoryRepo
                // If you have configured SQL Server, try SqlHistoryRepo
                //IHistoryRepo historyRepo = new SqlHistoryRepo();x
                IHistoryRepo _historyRepo = new EmptyHistoryRepo( );
                var _chatGptService = new ChatGptService( _openaiApiKey );
                var _mainViewModel = new MainViewModel( _historyRepo, _chatGptService );
                var _mainWindow = new MainWindow( _mainViewModel );
                SetupRestoreWindowPlace( _mainWindow );
                _mainWindow.Show( );
            }
            catch( Exception ex )
            {
                MessageBox.Show( ex.ToString( ), "Booger will exit on error" );
                Current?.Shutdown( );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Exit" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs" />
        /// that contains the event data.</param>
        protected override void OnExit( ExitEventArgs e )
        {
            base.OnExit( e );
            try
            {
                _windowPlace?.Save( );
            }
            catch( Exception )
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Setups the restore window place.
        /// </summary>
        /// <param name="mainWindow">The main window.</param>
        private void SetupRestoreWindowPlace( MainWindow mainWindow )
        {
            var _windowPlaceConfigFilePath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory,
                "Booger.config" );

            _windowPlace = new WindowPlace( _windowPlaceConfigFilePath );
            _windowPlace.Register( mainWindow );

            // This logic works but I don't like the window being maximized
            //if (!File.Exists(windowPlaceConfigFilePath))
            //{
            //    For the first time, maximize the window,
            //    so it won't go off the screen on laptop
            //    WindowPlacement will take care of future runs
            //    mainWindow.WindowState = WindowState.Maximized;
            //}
        }

        /// <summary>
        /// Registers the theme.
        /// </summary>
        private void RegisterTheme( )
        {
            var _theme = new FluentDarkThemeSettings
            {
                PrimaryBackground = new SolidColorBrush( Color.FromRgb( 20, 20, 20 ) ),
                PrimaryColorForeground = new SolidColorBrush( Color.FromRgb( 0, 120, 212 ) ),
                PrimaryForeground = new SolidColorBrush( Color.FromRgb( 222, 222, 222 ) ),
                BodyFontSize = 12,
                HeaderFontSize = 16,
                SubHeaderFontSize = 14,
                TitleFontSize = 14,
                SubTitleFontSize = 126,
                BodyAltFontSize = 10,
                FontFamily = new FontFamily( "Segoe UI" )
            };

            SfSkinManager.RegisterThemeSettings( "FluentDark", _theme );
            SfSkinManager.ApplyStylesOnApplication = true;
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