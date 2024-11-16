// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 10-30-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        10-30-2024
// ******************************************************************************************
// <copyright file="ConfigPage.xaml.cs" company="Terry D. Eppler">
//   An open source windows (wpf) application for interacting with Chat GPT that's developed
//   in C-Sharp under the MIT license
// 
//    Copyright ©  2020-2024 Terry D. Eppler
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
//    You can contact me at:  terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   ConfigPage.xaml.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using CommunityToolkit.Mvvm.Input;

    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for ConfigPage.xaml
    /// </summary>
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public partial class ConfigurationPage : Page
    {
        /// <summary>
        /// The application window
        /// </summary>
        private protected MainWindow _appWindow;

        /// <summary>
        /// The view model
        /// </summary>
        private protected ConfigPageModel _viewModel;

        /// <summary>
        /// The view model
        /// </summary>
        private protected LanguageService _languageService;

        /// <summary>
        /// The application global data
        /// </summary>
        private protected ColorModeService _colorModeService;

        /// <summary>
        /// The page service
        /// </summary>
        private protected PageService _pageService;

        /// <summary>
        /// The note service
        /// </summary>
        private protected NoteService _noteService;

        /// <summary>
        /// The configuration service
        /// </summary>
        private protected ConfigurationService _configurationService;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Booger.ConfigurationPage" /> class.
        /// </summary>
        /// <param name="appWindow">The application window.</param>
        /// <param name="viewModel">The view model.</param>
        /// <param name="pageService">The page service.</param>
        /// <param name="noteService">The note service.</param>
        /// <param name="languageService">The language service.</param>
        /// <param name="colorModeService">The color mode service.</param>
        /// <param name="configurationService">The configuration service.</param>
        /// <param name="scrollingService">The scrolling service.</param>
        public ConfigurationPage( MainWindow appWindow, ConfigPageModel viewModel, 
            PageService pageService, NoteService noteService, 
            LanguageService languageService, ColorModeService colorModeService, 
            ConfigurationService configurationService, SmoothScrollingService scrollingService )
        {
            _appWindow = appWindow;
            _viewModel = viewModel;
            _pageService = pageService;
            _noteService = noteService;
            _languageService = languageService;
            _colorModeService = colorModeService;
            _configurationService = configurationService;
            DataContext = this;
            LoadSystemMessagesCore( );
            InitializeComponent( );
            scrollingService.Register( ConfigurationScrollViewer );
        }

        /// <summary>
        /// Gets the application window.
        /// </summary>
        /// <value>
        /// The application window.
        /// </value>
        public MainWindow AppWindow
        {
            get
            {
                return _appWindow;
            }
            set
            {
                _appWindow = value;
            }
        }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public ConfigPageModel ViewModel
        {
            get
            {
                return _viewModel;
            }
            set
            {
                _viewModel = value;
            }
        }

        /// <summary>
        /// Gets or sets the language service.
        /// </summary>
        /// <value>
        /// The language service.
        /// </value>
        public LanguageService LanguageService
        {
            get
            {
                return _languageService;
            }
            set
            {
                _languageService = value;
            }
        }

        /// <summary>
        /// Gets the page service.
        /// </summary>
        /// <value>
        /// The page service.
        /// </value>
        public PageService PageService
        {
            get
            {
                return _pageService;
            }
            set
            {
                _pageService = value;
            }
        }

        /// <summary>
        /// Gets the note service.
        /// </summary>
        /// <value>
        /// The note service.
        /// </value>
        public NoteService NoteService
        {
            get
            {
                return _noteService;
            }
            set
            {
                _noteService = value;
            }
        }

        /// <summary>
        /// Gets the chat service.
        /// </summary>
        /// <value>
        /// The chat service.
        /// </value>
        public ColorModeService ColorModeService
        {
            get
            {
                return _colorModeService;
            }
            set
            {
                _colorModeService = value;
            }
        }

        /// <summary>
        /// Gets the configuration service.
        /// </summary>
        /// <value>
        /// The configuration service.
        /// </value>
        public ConfigurationService ConfigurationService
        {
            get
            {
                return _configurationService;
            }
            set
            {
                _configurationService = value;
            }
        }

        /// <summary>
        /// Loads the system messages core.
        /// </summary>
        private void LoadSystemMessagesCore( )
        {
            _viewModel.SystemMessages.Clear( );
            foreach( var _msg in _configurationService.Configuration.SystemMessages )
            {
                _viewModel.SystemMessages.Add( new ValueWrapper<string>( _msg ) );
            }
        }

        /// <summary>
        /// Applies the system messages core.
        /// </summary>
        private void ApplySystemMessagesCore( )
        {
            _configurationService.Configuration.SystemMessages = _viewModel.SystemMessages
                .Select( wraper => wraper.Value )
                .ToArray( );
        }

        /// <summary>
        /// Goes to main page.
        /// </summary>
        [RelayCommand ]
        public void GoToMainPage( )
        {
            _appWindow.Navigate<MainPage>( );
        }

        [ RelayCommand ]
        public void AboutOpenChat( )
        {
            MessageBox.Show( Application.Current.MainWindow, $"""
                                                              {nameof( Booger )}, by is-leeroy-jenkins v{Assembly.GetEntryAssembly( )?.GetName( )?.Version}

                                                              A simple chat client based on OpenAI Chat completion API.

                                                              Repository: https://github.com/is-leeroy-jenkins/{nameof( Booger )}
                                                              /// <summary>
                                                              /// Initializes a new instance of the <see cref="$Program" /> class.
                                                              /// </summary>
                                                              """, $"About {nameof( Booger )}",
                MessageBoxButton.OK, MessageBoxImage.Information );
        }

        /// <summary>
        /// Loads the system messages.
        /// </summary>
        /// <returns></returns>
        [RelayCommand ]
        public Task LoadSystemMessages( )
        {
            LoadSystemMessagesCore( );
            return _noteService.ShowAndWaitAsync( "System messages loaded", 1500 );
        }

        /// <summary>
        /// Applies the system messages.
        /// </summary>
        /// <returns></returns>
        [RelayCommand ]
        public Task ApplySystemMessages( )
        {
            ApplySystemMessagesCore( );
            return _noteService.ShowAndWaitAsync( "System messages applied", 1500 );
        }

        /// <summary>
        /// Adds the system message.
        /// </summary>
        /// <returns></returns>
        [RelayCommand ]
        public void AddSystemMessage( )
        {
            _viewModel.SystemMessages.Add( new ValueWrapper<string>( "New system message" ) );
        }

        /// <summary>
        /// Removes the system message.
        /// </summary>
        /// <returns></returns>
        [RelayCommand ]
        public void RemoveSystemMessage( )
        {
            if( _viewModel.SystemMessages.Count > 0 )
            {
                _viewModel.SystemMessages.RemoveAt( _viewModel.SystemMessages.Count - 1 );
            }
        }

        /// <summary>
        /// Saves the configuration.
        /// </summary>
        /// <returns></returns>
        [RelayCommand ]
        public Task SaveConfiguration( )
        {
            _configurationService.Configuration.Language =
                _languageService.CurrentLanguage.ToString( );

            _configurationService.Configuration.ColorMode = _colorModeService.CurrentMode;
            _configurationService.Save( );
            return _noteService.ShowAndWaitAsync( "Configuration saved", 2000 );
        }

        /// <summary>
        /// Called when [calculator menu option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnCalculatorMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _calculator = new CalculatorWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Topmost = true
                };

                _calculator.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [file menu option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnFileMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _fileBrowser = new FileBrowser
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Topmost = true
                };

                _fileBrowser.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [folder menu option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnFolderMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _fileBrowser = new FileBrowser
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Topmost = true
                };

                _fileBrowser.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [control panel option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnControlPanelOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WinMinion.LaunchControlPanel( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [task manager option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnTaskManagerOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WinMinion.LaunchTaskManager( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [close option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnCloseOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                Application.Current.Shutdown( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [chrome option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// containing the event data.</param>
        private void OnChromeOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WebMinion.RunChrome( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [edge option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnEdgeOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WebMinion.RunEdge( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [firefox option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// containing the event data.</param>
        private void OnFirefoxOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WebMinion.RunFirefox( );
            }
            catch( Exception ex )
            {
                Fail( ex );
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