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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Page" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public partial class MainPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        /// <param name="appWindow">The application window.</param>
        /// <param name="viewModel">The view model.</param>
        /// <param name="appGlobalData">The application global data.</param>
        /// <param name="pageService">The page service.</param>
        /// <param name="noteService">The note service.</param>
        /// <param name="chatService">The chat service.</param>
        /// <param name="chatPageService">The chat page service.</param>
        /// <param name="chatStorageService">The chat storage service.</param>
        /// <param name="configurationService">The configuration service.</param>
        /// <param name="smoothScrollingService">The smooth scrolling service.</param>
        public MainPage( MainWindow appWindow, MainPageModel viewModel, AppGlobalData appGlobalData,
            PageService pageService, NoteService noteService, ChatService chatService,
            ChatPageService chatPageService, ChatStorageService chatStorageService,
            ConfigurationService configurationService,
            SmoothScrollingService smoothScrollingService )
        {
            AppWindow = appWindow;
            ViewModel = viewModel;
            AppGlobalData = appGlobalData;
            PageService = pageService;
            NoteService = noteService;
            ChatService = chatService;
            ChatPageService = chatPageService;
            ChatStorageService = chatStorageService;
            ConfigurationService = configurationService;
            DataContext = this;
            foreach( var _session in ChatStorageService.GetAllSessions( ) )
            {
                AppGlobalData.Sessions.Add( new ChatSessionModel( _session ) );
            }

            if( AppGlobalData.Sessions.Count == 0 )
            {
                NewSession( );
            }

            InitializeComponent( );
            SwitchPageToCurrentSession( );
            smoothScrollingService.Register( sessionsScrollViewer );
        }

        /// <summary>
        /// Gets the application window.
        /// </summary>
        /// <value>
        /// The application window.
        /// </value>
        public MainWindow AppWindow { get; }

        /// <summary>
        /// Gets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public MainPageModel ViewModel { get; }

        /// <summary>
        /// Gets the application global data.
        /// </summary>
        /// <value>
        /// The application global data.
        /// </value>
        public AppGlobalData AppGlobalData { get; }

        /// <summary>
        /// Gets the page service.
        /// </summary>
        /// <value>
        /// The page service.
        /// </value>
        public PageService PageService { get; }

        /// <summary>
        /// Gets the note service.
        /// </summary>
        /// <value>
        /// The note service.
        /// </value>
        public NoteService NoteService { get; }

        /// <summary>
        /// Gets the chat service.
        /// </summary>
        /// <value>
        /// The chat service.
        /// </value>
        public ChatService ChatService { get; }

        /// <summary>
        /// Gets the chat page service.
        /// </summary>
        /// <value>
        /// The chat page service.
        /// </value>
        public ChatPageService ChatPageService { get; }

        /// <summary>
        /// Gets the chat storage service.
        /// </summary>
        /// <value>
        /// The chat storage service.
        /// </value>
        public ChatStorageService ChatStorageService { get; }

        /// <summary>
        /// Gets the configuration service.
        /// </summary>
        /// <value>
        /// The configuration service.
        /// </value>
        public ConfigurationService ConfigurationService { get; }

        /// <summary>
        /// Goes to configuration page.
        /// </summary>
        [ RelayCommand ]
        public void GoToConfigPage( )
        {
            AppWindow.Navigate<ConfigPage>( );
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
                ChatService.Cancel( );
                ChatStorageService.ClearMessage( _sessionId );
                ViewModel.CurrentChat?.ViewModel.Messages.Clear( );
                await NoteService.ShowAndWaitAsync( "Chat has been reset.", 1500 );
            }
            else
            {
                await NoteService.ShowAndWaitAsync( "You need to select a session.", 1500 );
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
            ChatStorageService.SaveSession( _session );
            AppGlobalData.Sessions.Add( _sessionModel );
            AppGlobalData.SelectedSession = _sessionModel;
        }

        /// <summary>
        /// Deletes the session.
        /// </summary>
        /// <param name="session">The session.</param>
        [ RelayCommand ]
        public void DeleteSession( ChatSessionModel session )
        {
            if( AppGlobalData.Sessions.Count == 1 )
            {
                NoteService.Show( "You can't delete the last session.", 1500 );
                return;
            }

            var _index = AppGlobalData.Sessions.IndexOf( session );
            var _newIndex = Math.Max( 0, _index - 1 );
            ChatPageService.RemovePage( session.Id );
            ChatStorageService.DeleteSession( session.Id );
            AppGlobalData.Sessions.Remove( session );
            AppGlobalData.SelectedSession = AppGlobalData.Sessions[ _newIndex ];
        }

        /// <summary>
        /// Switches to next session.
        /// </summary>
        [ RelayCommand ]
        public void SwitchToNextSession( )
        {
            int _nextIndex;
            var _lastIndex = AppGlobalData.Sessions.Count - 1;
            if( AppGlobalData.SelectedSession != null )
            {
                _nextIndex = AppGlobalData.Sessions.IndexOf( AppGlobalData.SelectedSession ) + 1;
            }
            else
            {
                _nextIndex = 0;
            }

            _nextIndex = Math.Clamp( _nextIndex, 0, _lastIndex );
            AppGlobalData.SelectedSession = AppGlobalData.Sessions[ _nextIndex ];
        }

        /// <summary>
        /// Switches to previous session.
        /// </summary>
        [ RelayCommand ]
        public void SwitchToPreviousSession( )
        {
            int _previousIndex;
            var _lastIndex = AppGlobalData.Sessions.Count - 1;
            if( AppGlobalData.SelectedSession != null )
            {
                _previousIndex =
                    AppGlobalData.Sessions.IndexOf( AppGlobalData.SelectedSession ) - 1;
            }
            else
            {
                _previousIndex = 0;
            }

            _previousIndex = Math.Clamp( _previousIndex, 0, _lastIndex );
            AppGlobalData.SelectedSession = AppGlobalData.Sessions[ _previousIndex ];
        }

        /// <summary>
        /// Cycles the switch to next session.
        /// </summary>
        [ RelayCommand ]
        public void CycleSwitchToNextSession( )
        {
            int _nextIndex;
            var _lastIndex = AppGlobalData.Sessions.Count - 1;
            if( AppGlobalData.SelectedSession != null )
            {
                _nextIndex = AppGlobalData.Sessions.IndexOf( AppGlobalData.SelectedSession ) + 1;
            }
            else
            {
                _nextIndex = 0;
            }

            if( _nextIndex > _lastIndex )
            {
                _nextIndex = 0;
            }

            AppGlobalData.SelectedSession = AppGlobalData.Sessions[ _nextIndex ];
        }

        /// <summary>
        /// Cycles the switch to previous session.
        /// </summary>
        [ RelayCommand ]
        public void CycleSwitchToPreviousSession( )
        {
            int _previousIndex;
            var _lastIndex = AppGlobalData.Sessions.Count - 1;
            if( AppGlobalData.SelectedSession != null )
            {
                _previousIndex =
                    AppGlobalData.Sessions.IndexOf( AppGlobalData.SelectedSession ) - 1;
            }
            else
            {
                _previousIndex = 0;
            }

            if( _previousIndex < 0 )
            {
                _previousIndex = _lastIndex;
            }

            AppGlobalData.SelectedSession = AppGlobalData.Sessions[ _previousIndex ];
        }

        /// <summary>
        /// Deletes the current session.
        /// </summary>
        [ RelayCommand ]
        public void DeleteCurrentSession( )
        {
            if( AppGlobalData.SelectedSession != null )
            {
                DeleteSession( AppGlobalData.SelectedSession );
            }
        }

        /// <summary>
        /// Switches the page to current session.
        /// </summary>
        [ RelayCommand ]
        public void SwitchPageToCurrentSession( )
        {
            if( AppGlobalData.SelectedSession != null )
            {
                ViewModel.CurrentChat = ChatPageService.GetPage( AppGlobalData.SelectedSession.Id );
            }
        }
    }
}