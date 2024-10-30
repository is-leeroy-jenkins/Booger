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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Threading;
    using CommunityToolkit.Mvvm.Input;

    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for ChatPage.xaml
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    public partial class ChatPage : Page
    {
        /// <summary>
        /// The current session model
        /// </summary>
        private ChatSessionModel _currentSessionModel;

        /// <summary>
        /// The view model
        /// </summary>
        private protected ChatPageModel _viewModel;

        /// <summary>
        /// The application global data
        /// </summary>
        private protected AppGlobalData _appGlobalData;

        /// <summary>
        /// The chat service
        /// </summary>
        private protected ChatService _chatService;

        /// <summary>
        /// The chat storage service
        /// </summary>
        private protected ChatStorageService _chatStorageService;

        /// <summary>
        /// The note service
        /// </summary>
        private protected NoteService _noteService;

        /// <summary>
        /// The configuration service
        /// </summary>
        private protected ConfigurationService _configurationService;

        /// <summary>
        /// The title generation service
        /// </summary>
        private protected TitleGenerationService _titleGenerationService;

        /// <summary>
        /// The automatic scroll to end
        /// </summary>
        private protected bool _autoScrollToEnd;

        /// <summary>
        /// The session identifier
        /// </summary>
        private protected Guid _sessionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatPage"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="appGlobalData">The application global data.</param>
        /// <param name="noteService">The note service.</param>
        /// <param name="chatService">The chat service.</param>
        /// <param name="chatStorageService">The chat storage service.</param>
        /// <param name="configurationService">The configuration service.</param>
        /// <param name="smoothScrollingService">The smooth scrolling service.</param>
        /// <param name="titleGenerationService">The title generation service.</param>
        public ChatPage( ChatPageModel viewModel, AppGlobalData appGlobalData,
            NoteService noteService, ChatService chatService, ChatStorageService chatStorageService,
            ConfigurationService configurationService, SmoothScrollingService smoothScrollingService,
            TitleGenerationService titleGenerationService )
        {
            _viewModel = viewModel;
            _appGlobalData = appGlobalData;
            _noteService = noteService;
            _chatService = chatService;
            _chatStorageService = chatStorageService;
            _configurationService = configurationService;
            _titleGenerationService = titleGenerationService;
            DataContext = this;
            InitializeComponent( );
            MessageScrollViewer.PreviewMouseWheel += CloseAutoScrollWhileMouseWheel;
            MessageScrollViewer.ScrollChanged += MessageScrolled;
            smoothScrollingService.Register( MessageScrollViewer );
        }

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public ChatPageModel ViewModel
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
        /// Gets or sets the application global data.
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
        /// Gets or sets the chat service.
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
        /// Gets or sets the chat storage service.
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
        /// Gets or sets the note service.
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
        /// Gets or sets the configuration service.
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
        /// Gets or sets the title generation service.
        /// </summary>
        /// <value>
        /// The title generation service.
        /// </value>
        public TitleGenerationService TitleGenerationService
        {
            get
            {
                return _titleGenerationService;
            }
            set
            {
                _titleGenerationService = value;
            }
        }

        /// <summary>
        /// Gets or sets the session identifier.
        /// </summary>
        /// <value>
        /// The session identifier.
        /// </value>
        public Guid SessionId
        {
            get
            {
                return _sessionId;
            }
            set
            {
                _sessionId = value;
            }
        }

        /// <summary>
        /// Gets the current session model.
        /// </summary>
        /// <value>
        /// The current session model.
        /// </value>
        public ChatSessionModel CurrentSessionModel
        {
            get
            {
                return _currentSessionModel ??=
                    _appGlobalData.Sessions.FirstOrDefault( session => session.Id == _sessionId );
            }
        }

        /// <summary>
        /// Initializes the session.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public void InitSession( Guid sessionId )
        {
            _sessionId = sessionId;
            _viewModel.Messages.Clear( );
            foreach( var _msg in _chatStorageService.GetLastMessages( _sessionId, 10 ) )
            {
                _viewModel.Messages.Add( new ChatMessageModel( _msg ) );
            }
        }

        /// <summary>
        /// Chats the asynchronous.
        /// </summary>
        [RelayCommand ]
        public async Task ChatAsync( )
        {
            if( string.IsNullOrWhiteSpace( _viewModel.InputBoxText ) )
            {
                _ = _noteService.ShowAndWaitAsync( "Empty message", 1500 );
                return;
            }

            if( string.IsNullOrWhiteSpace( _configurationService.Configuration.ApiKey ) )
            {
                var _msg = "You can't use OpenChat now, because you haven't set your api key yet";
                await _noteService.ShowAndWaitAsync( _msg, 3000 );
                return;
            }

            if( MessageScrollViewer.IsAtEnd( ) )
            {
                _autoScrollToEnd = true;
            }

            var _input = _viewModel.InputBoxText.Trim( );
            _viewModel.InputBoxText = string.Empty;
            var _requestMessageModel = new ChatMessageModel( "user", _input );
            var _responseMessageModel = new ChatMessageModel( "assistant", string.Empty );
            var _responseAdded = false;
            _viewModel.Messages.Add( _requestMessageModel );
            try
            {
                var _dialogue =
                    await _chatService.ChatAsync( _sessionId, _input, content =>
                    {
                        _responseMessageModel.Content = content;
                        if( !_responseAdded )
                        {
                            _responseAdded = true;
                            Dispatcher.Invoke( ( ) =>
                            {
                                _viewModel.Messages.Add( _responseMessageModel );
                            } );
                        }
                    } );

                _requestMessageModel.Storage = _dialogue.Ask;
                _responseMessageModel.Storage = _dialogue.Answer;
                if( _currentSessionModel is ChatSessionModel _currentSession
                    && string.IsNullOrEmpty( _currentSession.Name ) )
                {
                    var _title = await _titleGenerationService.GenerateAsync( new[ ]
                    {
                        _requestMessageModel.Content,
                        _responseMessageModel.Content
                    } );

                    _currentSession.Name = _title;
                }
            }
            catch( TaskCanceledException )
            {
                Rollback( _requestMessageModel, _responseMessageModel, _input );
            }
            catch( Exception ex )
            { 
                var _ = _noteService.ShowAndWaitAsync( $"{ex.GetType( ).Name}: {ex.Message}", 3000 );
                Rollback( _requestMessageModel, _responseMessageModel, _input );
            }

            void Rollback( ChatMessageModel request, ChatMessageModel response, string input )
            {
                _viewModel.Messages.Remove( request );
                _viewModel.Messages.Remove( response );
                if( string.IsNullOrWhiteSpace( _viewModel.InputBoxText ) )
                {
                    _viewModel.InputBoxText = _input;
                }
                else
                {
                    _viewModel.InputBoxText = $"{_input} {_viewModel.InputBoxText}";
                }
            }
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        [RelayCommand ]
        public void Cancel( )
        {
            _chatService.Cancel( );
        }

        /// <summary>
        /// Chats the or cancel.
        /// </summary>
        [RelayCommand ]
        public void ChatOrCancel( )
        {
            if( ChatCommand.IsRunning )
            {
                _chatService.Cancel( );
            }
            else
            {
                ChatCommand.Execute( null );
            }
        }

        /// <summary>
        /// Copies the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        [RelayCommand ]
        public void Copy( string text )
        {
            Clipboard.SetText( text );
        }

        /// <summary>
        /// Closes the automatic scroll while mouse wheel.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseWheelEventArgs"/> instance containing the event data.</param>
        private void CloseAutoScrollWhileMouseWheel( object sender, MouseWheelEventArgs e )
        {
            _autoScrollToEnd = false;
        }

        /// <summary>
        /// Messages the scrolled.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ScrollChangedEventArgs"/> instance containing the event data.</param>
        private void MessageScrolled( object sender, ScrollChangedEventArgs e )
        {
            if( e.OriginalSource != MessageScrollViewer )
            {
                return;
            }

            if( MessageScrollViewer.IsAtEnd( ) )
            {
                _autoScrollToEnd = true;
            }

            if( e.VerticalChange != 0
                && Messages.IsLoaded
                && IsLoaded
                && MessageScrollViewer.IsAtTop( 10 )
                && _viewModel.Messages.FirstOrDefault( )?.Storage?.Timestamp is DateTime _timestamp )
            {
                foreach( var _msg in _chatStorageService.GetLastMessagesBefore( _sessionId, 10,
                    _timestamp ) )
                {
                    _viewModel.Messages.Insert( 0, new ChatMessageModel( _msg ) );
                }

                var _distanceFromEnd = MessageScrollViewer.ScrollableHeight
                    - MessageScrollViewer.VerticalOffset;

                Dispatcher.BeginInvoke( DispatcherPriority.Loaded,
                    new Action<ScrollChangedEventArgs>( e =>
                    {
                        var _sv = (ScrollViewer)e.Source;
                        _sv.ScrollToVerticalOffset( _sv.ScrollableHeight - _distanceFromEnd );
                    } ), e );

                e.Handled = true;
            }
        }

        /// <summary>
        /// Scrolls to end while receiving.
        /// </summary>
        [RelayCommand ]
        public void ScrollToEndWhileReceiving( )
        {
            if( ChatCommand.IsRunning && _autoScrollToEnd )
            {
                MessageScrollViewer.ScrollToEnd( );
            }
        }
    }
}