// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="MainPage.xaml.cs" company="Terry D. Eppler">
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
//   MainPage.xaml.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using CommunityToolkit.Mvvm.Input;

    public partial class MainPage : Page
    {
        public MainPage( MainWindow appWindow,
            MainPageModel viewModel,
            AppGlobalData appGlobalData,
            PageService pageService,
            NoteService noteService,
            ChatService chatService,
            ChatPageService chatPageService,
            ChatStorageService chatStorageService,
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

        public MainWindow AppWindow { get; }

        public MainPageModel ViewModel { get; }

        public AppGlobalData AppGlobalData { get; }

        public PageService PageService { get; }

        public NoteService NoteService { get; }

        public ChatService ChatService { get; }

        public ChatPageService ChatPageService { get; }

        public ChatStorageService ChatStorageService { get; }

        public ConfigurationService ConfigurationService { get; }

        [ RelayCommand ]
        public void GoToConfigPage( )
        {
            AppWindow.Navigate<ConfigPage>( );
        }

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

        [ RelayCommand ]
        public void NewSession( )
        {
            var _session = ChatSession.Create( );
            var _sessionModel = new ChatSessionModel( _session );
            ChatStorageService.SaveSession( _session );
            AppGlobalData.Sessions.Add( _sessionModel );
            AppGlobalData.SelectedSession = _sessionModel;
        }

        [ RelayCommand ]
        public void DeleteSession( ChatSessionModel session )
        {
            if( AppGlobalData.Sessions.Count == 1 )
            {
                NoteService.Show( "You can't delete the last session.", 1500 );
                return;
            }

            var _index =
                AppGlobalData.Sessions.IndexOf( session );

            var _newIndex =
                Math.Max( 0, _index - 1 );

            ChatPageService.RemovePage( session.Id );
            ChatStorageService.DeleteSession( session.Id );
            AppGlobalData.Sessions.Remove( session );
            AppGlobalData.SelectedSession = AppGlobalData.Sessions[ _newIndex ];
        }

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
            AppGlobalData.SelectedSession =
                AppGlobalData.Sessions[ _nextIndex ];
        }

        [ RelayCommand ]
        public void SwitchToPreviousSession( )
        {
            int _previousIndex;
            var _lastIndex = AppGlobalData.Sessions.Count - 1;
            if( AppGlobalData.SelectedSession != null )
            {
                _previousIndex = AppGlobalData.Sessions.IndexOf( AppGlobalData.SelectedSession ) - 1;
            }
            else
            {
                _previousIndex = 0;
            }

            _previousIndex = Math.Clamp( _previousIndex, 0, _lastIndex );
            AppGlobalData.SelectedSession =
                AppGlobalData.Sessions[ _previousIndex ];
        }

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

            AppGlobalData.SelectedSession =
                AppGlobalData.Sessions[ _nextIndex ];
        }

        [ RelayCommand ]
        public void CycleSwitchToPreviousSession( )
        {
            int _previousIndex;
            var _lastIndex = AppGlobalData.Sessions.Count - 1;
            if( AppGlobalData.SelectedSession != null )
            {
                _previousIndex = AppGlobalData.Sessions.IndexOf( AppGlobalData.SelectedSession ) - 1;
            }
            else
            {
                _previousIndex = 0;
            }

            if( _previousIndex < 0 )
            {
                _previousIndex = _lastIndex;
            }

            AppGlobalData.SelectedSession =
                AppGlobalData.Sessions[ _previousIndex ];
        }

        [ RelayCommand ]
        public void DeleteCurrentSession( )
        {
            if( AppGlobalData.SelectedSession != null )
            {
                DeleteSession( AppGlobalData.SelectedSession );
            }
        }

        [ RelayCommand ]
        public void SwitchPageToCurrentSession( )
        {
            if( AppGlobalData.SelectedSession != null )
            {
                ViewModel.CurrentChat =
                    ChatPageService.GetPage( AppGlobalData.SelectedSession.Id );
            }
        }
    }
}