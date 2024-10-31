// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="MainPage.xaml.cs" company="Terry D. Eppler">
//     Booger is a quick & dirty WPF application that interacts with OpenAI GPT-3.5 Turbo API
//     based on NET6 and written in C-Sharp.
// 
//     Copyright ©  2022 Terry D. Eppler
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
//   MainPage.xaml.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using CommunityToolkit.Mvvm.Input;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.Controls.Page" />
    /// <seealso cref="T:System.Windows.Markup.IComponentConnector" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "HeuristicUnreachableCode" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    public partial class MainPage : Page
    {
        /// <summary>
        /// The application window
        /// </summary>
        private protected MainWindow _mainWindow;

        /// <summary>
        /// The view model
        /// </summary>
        private protected MainPageModel _mainPageModel;

        /// <summary>
        /// The application global data
        /// </summary>
        private protected AppGlobalData _appGlobalData;

        /// <summary>
        /// The page service
        /// </summary>
        private protected PageService _pageService;

        /// <summary>
        /// The note service
        /// </summary>
        private protected NoteService _noteService;

        /// <summary>
        /// The chat service
        /// </summary>
        private protected ChatService _chatService;

        /// <summary>
        /// The chat service
        /// </summary>
        private protected ChatPageService _chatPageService;

        /// <summary>
        /// The chat storage service
        /// </summary>
        private protected ChatStorageService _chatStorageService;

        /// <summary>
        /// The configuration service
        /// </summary>
        private protected ConfigurationService _configurationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        /// <param name="mainWindow">The application window.</param>
        /// <param name="mainPageModel">The view model.</param>
        /// <param name="appGlobalData">The application global data.</param>
        /// <param name="pageService">The page service.</param>
        /// <param name="noteService">The note service.</param>
        /// <param name="chatService">The chat service.</param>
        /// <param name="chatPageService">The chat page service.</param>
        /// <param name="storageService">The chat storage service.</param>
        /// <param name="configuration">The configuration service.</param>
        /// <param name="scrollingService">The smooth scrolling service.</param>
        public MainPage( MainWindow mainWindow, MainPageModel mainPageModel, AppGlobalData appGlobalData,
            PageService pageService, NoteService noteService, ChatService chatService,
            ChatPageService chatPageService, ChatStorageService storageService,
            ConfigurationService configuration,
            SmoothScrollingService scrollingService )
        {
            _mainWindow = mainWindow;
            _mainPageModel = mainPageModel;
            _appGlobalData = appGlobalData;
            _pageService = pageService;
            _noteService = noteService;
            _chatService = chatService;
            _chatPageService = chatPageService;
            _chatStorageService = storageService;
            _configurationService = configuration;
            DataContext = this;
            foreach( var _session in _chatStorageService.GetAllSessions( ) )
            {
                _appGlobalData.Sessions.Add( new ChatSessionModel( _session ) );
            }

            if( _appGlobalData.Sessions.Count == 0 )
            {
                NewSession( );
            }

            InitializeComponent( );
            SwitchPageToCurrentSession( );
            scrollingService.Register( SessionsScrollViewer );
        }

        /// <summary>
        /// Gets the application window.
        /// </summary>
        /// <value>
        /// The application window.
        /// </value>
        public MainWindow MainWindow
        {
            get
            {
                return _mainWindow;
            }
            set
            {
                _mainWindow = value;
            }
        }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public MainPageModel MainPageModel
        {
            get
            {
                return _mainPageModel;
            }
            set
            {
                _mainPageModel = value;
            }
        }

        /// <summary>
        /// Gets the application global data.
        /// </summary>
        /// <value>
        /// The application global data.
        /// </value>
        public AppGlobalData AppGlobalData
        {
            get
            {
                return _appGlobalData;
            }
            set
            {
                _appGlobalData = value;
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
        public ChatService ChatService
        {
            get
            {
                return _chatService;
            }
            set
            {
                _chatService = value;
            }
        }

        /// <summary>
        /// Gets the chat page service.
        /// </summary>
        /// <value>
        /// The chat page service.
        /// </value>
        public ChatPageService ChatPageService
        {
            get
            {
                return _chatPageService;
            }
            set
            {
                _chatPageService = value;
            }
        }

        /// <summary>
        /// Gets the chat storage service.
        /// </summary>
        /// <value>
        /// The chat storage service.
        /// </value>
        public ChatStorageService ChatStorageService
        {
            get
            {
                return _chatStorageService;
            }
            set
            {
                _chatStorageService = value;
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
        /// Goes to configuration page.
        /// </summary>
        [ RelayCommand ]
        public void GoToConfigPage( )
        {
            _mainWindow.Navigate<ConfigurationPage>( );
        }

        /// <summary>
        /// Resets the chat.
        /// </summary>
        [ RelayCommand ]
        public async Task ResetChat( )
        {
            if( AppGlobalData.SelectedSession != null )
            {
                var _sessionId = AppGlobalData.SelectedSession.Id;
                _chatService.Cancel( );
                _chatStorageService.ClearMessage( _sessionId );
                _mainPageModel.CurrentChat?.ViewModel.Messages.Clear( );
                await _noteService.ShowAndWaitAsync( "Chat has been reset.", 1500 );
            }
            else
            {
                await _noteService.ShowAndWaitAsync( "You need to select a session.", 1500 );
            }
        }

        /// <summary>
        /// Creates new session.
        /// </summary>
        [ RelayCommand ]
        public void NewSession( )
        {
            var _session = ChatSession.Create( );
            var _sessionModel = new ChatSessionModel( _session );
            _chatStorageService.SaveSession( _session );
            _appGlobalData.Sessions.Add( _sessionModel );
            _appGlobalData.SelectedSession = _sessionModel;
        }

        /// <summary>
        /// Deletes the session.
        /// </summary>
        /// <param name="session">The session.</param>
        [ RelayCommand ]
        public void DeleteSession( ChatSessionModel session )
        {
            if( _appGlobalData.Sessions.Count == 1 )
            {
                _noteService.Show( "You can't delete the last session.", 1500 );
                return;
            }

            var _index = _appGlobalData.Sessions.IndexOf( session );
            var _newIndex = Math.Max( 0, _index - 1 );
            _chatPageService.RemovePage( session.Id );
            _chatStorageService.DeleteSession( session.Id );
            _appGlobalData.Sessions.Remove( session );
            _appGlobalData.SelectedSession = _appGlobalData.Sessions[ _newIndex ];
        }

        /// <summary>
        /// Switches to next session.
        /// </summary>
        [ RelayCommand ]
        public void SwitchToNextSession( )
        {
            int _nextIndex;
            var _lastIndex = _appGlobalData.Sessions.Count - 1;
            if( _appGlobalData.SelectedSession != null )
            {
                _nextIndex = _appGlobalData.Sessions.IndexOf( _appGlobalData.SelectedSession ) + 1;
            }
            else
            {
                _nextIndex = 0;
            }

            _nextIndex = Math.Clamp( _nextIndex, 0, _lastIndex );
            _appGlobalData.SelectedSession = _appGlobalData.Sessions[ _nextIndex ];
        }

        /// <summary>
        /// Switches to previous session.
        /// </summary>
        [ RelayCommand ]
        public void SwitchToPreviousSession( )
        {
            int _previousIndex;
            var _lastIndex = _appGlobalData.Sessions.Count - 1;
            if( _appGlobalData.SelectedSession != null )
            {
                _previousIndex =
                    _appGlobalData.Sessions.IndexOf( _appGlobalData.SelectedSession ) - 1;
            }
            else
            {
                _previousIndex = 0;
            }

            _previousIndex = Math.Clamp( _previousIndex, 0, _lastIndex );
            _appGlobalData.SelectedSession = _appGlobalData.Sessions[ _previousIndex ];
        }

        /// <summary>
        /// Cycles the switch to next session.
        /// </summary>
        [ RelayCommand ]
        public void CycleSwitchToNextSession( )
        {
            int _nextIndex;
            var _lastIndex = _appGlobalData.Sessions.Count - 1;
            if( _appGlobalData.SelectedSession != null )
            {
                _nextIndex = _appGlobalData.Sessions.IndexOf( _appGlobalData.SelectedSession ) + 1;
            }
            else
            {
                _nextIndex = 0;
            }

            if( _nextIndex > _lastIndex )
            {
                _nextIndex = 0;
            }

            _appGlobalData.SelectedSession = _appGlobalData.Sessions[ _nextIndex ];
        }

        /// <summary>
        /// Cycles the switch to previous session.
        /// </summary>
        [ RelayCommand ]
        public void CycleSwitchToPreviousSession( )
        {
            int _previousIndex;
            var _lastIndex = _appGlobalData.Sessions.Count - 1;
            if( _appGlobalData.SelectedSession != null )
            {
                _previousIndex =
                    _appGlobalData.Sessions.IndexOf( _appGlobalData.SelectedSession ) - 1;
            }
            else
            {
                _previousIndex = 0;
            }

            if( _previousIndex < 0 )
            {
                _previousIndex = _lastIndex;
            }

            _appGlobalData.SelectedSession = _appGlobalData.Sessions[ _previousIndex ];
        }

        /// <summary>
        /// Deletes the current session.
        /// </summary>
        [ RelayCommand ]
        public void DeleteCurrentSession( )
        {
            if( _appGlobalData.SelectedSession != null )
            {
                DeleteSession( _appGlobalData.SelectedSession );
            }
        }

        /// <summary>
        /// Switches the page to current session.
        /// </summary>
        [ RelayCommand ]
        public void SwitchPageToCurrentSession( )
        {
            if( _appGlobalData.SelectedSession != null )
            {
                _mainPageModel.CurrentChat = _chatPageService.GetPage( _appGlobalData.SelectedSession.Id );
            }
        }
    }
}