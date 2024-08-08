// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="ChatPage.xaml.cs" company="Terry D. Eppler">
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
//   ChatPage.xaml.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        public ChatPage( ChatPageModel viewModel,
            AppGlobalData appGlobalData,
            NoteService noteService,
            ChatService chatService,
            ChatStorageService chatStorageService,
            ConfigurationService configurationService,
            SmoothScrollingService smoothScrollingService,
            TitleGenerationService titleGenerationService )
        {
            ViewModel = viewModel;
            AppGlobalData = appGlobalData;
            NoteService = noteService;
            ChatService = chatService;
            ChatStorageService = chatStorageService;
            ConfigurationService = configurationService;
            TitleGenerationService = titleGenerationService;
            DataContext = this;
            InitializeComponent( );
            messagesScrollViewer.PreviewMouseWheel += CloseAutoScrollWhileMouseWheel;
            messagesScrollViewer.ScrollChanged += MessageScrolled;
            smoothScrollingService.Register( messagesScrollViewer );
        }

        private ChatSessionModel _currentSessionModel;

        public ChatPageModel ViewModel { get; }

        public AppGlobalData AppGlobalData { get; }

        public ChatService ChatService { get; }

        public ChatStorageService ChatStorageService { get; }

        public NoteService NoteService { get; }

        public ConfigurationService ConfigurationService { get; }

        public TitleGenerationService TitleGenerationService { get; }

        public Guid SessionId { get; private set; }

        public ChatSessionModel CurrentSessionModel
        {
            get
            {
                return _currentSessionModel ??=
                    AppGlobalData.Sessions.FirstOrDefault( session => session.Id == SessionId );
            }
        }

        public void InitSession( Guid sessionId )
        {
            SessionId = sessionId;
            ViewModel.Messages.Clear( );
            foreach( var _msg in ChatStorageService.GetLastMessages( SessionId, 10 ) )
            {
                ViewModel.Messages.Add( new ChatMessageModel( _msg ) );
            }
        }

        [ RelayCommand ]
        public async Task ChatAsync( )
        {
            if( string.IsNullOrWhiteSpace( ViewModel.InputBoxText ) )
            {
                _ = NoteService.ShowAndWaitAsync( "Empty message", 1500 );
                return;
            }

            if( string.IsNullOrWhiteSpace( ConfigurationService.Configuration.ApiKey ) )
            {
                await NoteService.ShowAndWaitAsync(
                    "You can't use OpenChat now, because you haven't set your api key yet", 3000 );

                return;
            }

            // 发个消息, 将自动滚动打开, 如果已经在底部, 则将自动滚动打开
            if( messagesScrollViewer.IsAtEnd( ) )
            {
                _autoScrollToEnd = true;
            }

            var _input = ViewModel.InputBoxText.Trim( );
            ViewModel.InputBoxText = string.Empty;
            var _requestMessageModel = new ChatMessageModel( "user", _input );
            var _responseMessageModel = new ChatMessageModel( "assistant", string.Empty );
            var _responseAdded = false;
            ViewModel.Messages.Add( _requestMessageModel );
            try
            {
                var _dialogue =
                    await ChatService.ChatAsync( SessionId, _input, content =>
                    {
                        _responseMessageModel.Content = content;
                        if( !_responseAdded )
                        {
                            _responseAdded = true;
                            Dispatcher.Invoke( ( ) =>
                            {
                                ViewModel.Messages.Add( _responseMessageModel );
                            } );
                        }
                    } );

                _requestMessageModel.Storage = _dialogue.Ask;
                _responseMessageModel.Storage = _dialogue.Answer;
                if( CurrentSessionModel is ChatSessionModel _currentSessionModel
                    && string.IsNullOrEmpty( _currentSessionModel.Name ) )
                {
                    var _title = await TitleGenerationService.GenerateAsync( new[ ]
                    {
                        _requestMessageModel.Content,
                        _responseMessageModel.Content
                    } );

                    _currentSessionModel.Name = _title;
                }
            }
            catch( TaskCanceledException )
            {
                Rollback( _requestMessageModel, _responseMessageModel, _input );
            }
            catch( Exception ex )
            {
                _ = NoteService.ShowAndWaitAsync( $"{ex.GetType( ).Name}: {ex.Message}", 3000 );
                Rollback( _requestMessageModel, _responseMessageModel, _input );
            }

            void Rollback( ChatMessageModel requestMessageModel,
                ChatMessageModel responseMessageModel,
                string originInput )
            {
                ViewModel.Messages.Remove( requestMessageModel );
                ViewModel.Messages.Remove( responseMessageModel );
                if( string.IsNullOrWhiteSpace( ViewModel.InputBoxText ) )
                {
                    ViewModel.InputBoxText = _input;
                }
                else
                {
                    ViewModel.InputBoxText = $"{_input} {ViewModel.InputBoxText}";
                }
            }
        }

        [ RelayCommand ]
        public void Cancel( )
        {
            ChatService.Cancel( );
        }

        [ RelayCommand ]
        public void ChatOrCancel( )
        {
            if( ChatCommand.IsRunning )
            {
                ChatService.Cancel( );
            }
            else
            {
                ChatCommand.Execute( null );
            }
        }

        [ RelayCommand ]
        public void Copy( string text )
        {
            Clipboard.SetText( text );
        }

        private bool _autoScrollToEnd;

        private void CloseAutoScrollWhileMouseWheel( object sender, MouseWheelEventArgs e )
        {
            _autoScrollToEnd = false;
        }

        private void MessageScrolled( object sender, ScrollChangedEventArgs e )
        {
            if( e.OriginalSource != messagesScrollViewer )
            {
                return;
            }

            if( messagesScrollViewer.IsAtEnd( ) )
            {
                _autoScrollToEnd = true;
            }

            if( e.VerticalChange != 0
                && messages.IsLoaded
                && IsLoaded
                && messagesScrollViewer.IsAtTop( 10 )
                && ViewModel.Messages.FirstOrDefault( )?.Storage?.Timestamp is DateTime _timestamp )
            {
                foreach( var _msg in ChatStorageService.GetLastMessagesBefore( SessionId, 10,
                    _timestamp ) )
                {
                    ViewModel.Messages.Insert( 0, new ChatMessageModel( _msg ) );
                }

                var _distanceFromEnd = messagesScrollViewer.ScrollableHeight
                    - messagesScrollViewer.VerticalOffset;

                Dispatcher.BeginInvoke( DispatcherPriority.Loaded,
                    new Action<ScrollChangedEventArgs>( e =>
                    {
                        var _sv = (ScrollViewer)e.Source;
                        _sv.ScrollToVerticalOffset( _sv.ScrollableHeight - _distanceFromEnd );
                    } ), e );

                e.Handled = true;
            }
        }

        [ RelayCommand ]
        public void ScrollToEndWhileReceiving( )
        {
            if( ChatCommand.IsRunning && _autoScrollToEnd )
            {
                messagesScrollViewer.ScrollToEnd( );
            }
        }
    }
}