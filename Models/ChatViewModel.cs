// ******************************************************************************************
//     Assembly:                Booger GPT
//     Author:                  Terry D. Eppler
//     Created:                 05-24-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        05-24-2024
// ******************************************************************************************
// <copyright file="ChatViewModel.cs" company="Terry D. Eppler">
//    This is a Federal Budget, Finance, and Accounting application
//    for the US Environmental Protection Agency (US EPA).
//    Copyright ©  2024  Terry Eppler
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
//    You can contact me at:   terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   ChatViewModel.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Speech.Synthesis;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.ComponentModel;
    using Whetstone.ChatGPT;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:CommunityToolkit.Mvvm.ComponentModel.ObservableObject" />
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public partial class ChatViewModel : ObservableObject
    {
        /// <summary>
        /// The chat GPT service
        /// </summary>
        private readonly GptService _gptService;

        /// <summary>
        /// The chat history
        /// </summary>
        private readonly ChatHistory _chatHistory;

        /// <summary>
        /// The chat input list
        /// </summary>
        private readonly List<string> _chatInputList;

        /// <summary>
        /// The chat input list index
        /// </summary>
        private int _chatInputListIndex;

        /// <summary>
        /// Gets the application title.
        /// </summary>
        /// <value>
        /// The application title.
        /// </value>
        public string AppTitle { get; }

        /// <summary>
        /// Gets or sets the update UI action.
        /// </summary>
        /// <value>
        /// The update UI action.
        /// </value>
        public Action<UpdateUIEnum> UpdateUIAction { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is command not busy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is command not busy;
        /// otherwise, <c>false</c>.
        /// </value>
        public bool IsCommandNotBusy
        {
            get
            {
                return !IsCommandBusy;
            }
        }

        /// <summary>
        /// The is command busy
        /// </summary>
        [ ObservableProperty ]
        private bool _isCommandBusy;

        /// <summary>
        /// The is send command busy
        /// </summary>
        [ ObservableProperty ]
        private bool _isSendCommandBusy;

        /// <summary>
        /// Wrap _chatHistory.ChatList
        /// </summary>
        /// <value>
        /// The chat list.
        /// </value>
        public ObservableCollection<Chat> ChatList { get; }

        /// <summary>
        /// The selected chat
        /// </summary>
        [ ObservableProperty ]
        private Chat _selectedChat;

        /// <summary>
        /// The chat input
        /// </summary>
        [ ObservableProperty ]
        private string _chatInput;

        /// <summary>
        /// The chat result
        /// </summary>
        [ ObservableProperty ]
        private string _chatResult = string.Empty;

        /// <summary>
        /// The selected message
        /// </summary>
        [ ObservableProperty ]
        private Message _selectedMessage;

        /// <summary>
        /// The image pane visibility
        /// </summary>
        [ ObservableProperty ]
        private Visibility _imagePaneVisibility = Visibility.Collapsed;

        /// <summary>
        /// The image input
        /// </summary>
        [ ObservableProperty ]
        private string _imageInput = "A tennis court";

        /// <summary>
        /// The result image
        /// </summary>
        [ ObservableProperty ]
        public byte[ ] _resultImage;

        /// <summary>
        /// The is streaming mode
        /// </summary>
        [ ObservableProperty ]
        private bool _isStreamingMode = true;

        /// <summary>
        /// Gets the language list.
        /// </summary>
        /// <value>
        /// The language list.
        /// </value>
        public string[ ] LangList { get; } =
        {
            "English",
            "Chinese",
            "Hindi",
            "Spanish"
        };

        /// <summary>
        /// The selected language
        /// </summary>
        [ ObservableProperty ]
        public string _selectedLanguage;

        /// <summary>
        /// The is female voice
        /// </summary>
        [ ObservableProperty ]
        private bool _isFemaleVoice = true;

        /// <summary>
        /// The status message
        /// </summary>
        [ ObservableProperty ]
        private string _statusMessage =
            "Ctrl+Enter for input of multiple lines. Enter-Key to send." 
            + " Ctrl+UpArrow | DownArrow to navigate previous input lines";

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatViewModel"/> class.
        /// </summary>
        /// <param name="gptService">The chat GPT service.</param>
        public ChatViewModel( GptService gptService )
        {
            _gptService = gptService;
            _chatHistory = new ChatHistory( );
            _selectedChat = _chatHistory.AddNewChat( );
            ChatList = new ObservableCollection<Chat>( _chatHistory.ChatList );
            _chatInputList = new List<string>( );
            _chatInputListIndex = -1;
            _chatInput = "What's the latest as of 2024 on ML.NET?";
            _selectedLanguage = LangList[ 3 ];
            var _appVer = Assembly.GetExecutingAssembly( ).GetName( ).Version!;
            var _dotnetVer = Environment.Version;
            AppTitle =
                $"C# WPF ChatGPT v{_appVer.Major}.{_appVer.Minor} (.NET {_dotnetVer.Major}.{_dotnetVer.Minor}.{_dotnetVer.Build} runtime) by Peter Sun";
#if DEBUG
            AppTitle += " - DEBUG";
#endif
        }

        /// <summary>
        /// Also RelayCommand from AppBar
        /// </summary>
        [ RelayCommand ]
        public void NewChat( )
        {
            try
            {
                if( !AddNewChatIfNotExists( ) )
                {
                    StatusMessage = "'New Chat' already exists";
                    return;
                }

                UpdateUIAction?.Invoke( UpdateUIEnum.SetFocusToChatInput );
                StatusMessage = "'New Chat' has been added and selected";
            }
            catch( Exception _ex )
            {
                StatusMessage = _ex.Message;
            }
        }

        /// <summary>
        /// Adds the new chat if not exists.
        /// </summary>
        /// <returns></returns>
        private bool AddNewChatIfNotExists( )
        {
            if( _chatHistory.NewChatExists )
            {
                return false;
            }

            var _newChat = _chatHistory.AddNewChat( );
            ChatList.Add( _newChat );
            SelectedChat = _newChat;
            return true;
        }

        /// <summary>
        /// Up/Previous or down/next chat input in the chat input list
        /// </summary>
        /// <param name="isUp">if set to <c>true</c> [is up].</param>
        public void PrevNextChatInput( bool isUp )
        {
            if( _chatInputList.IsNotEmpty( ) )
            {
                if( ChatInput.IsBlank( ) )
                {
                    ChatInput = _chatInputList[ ^1 ];
                    _chatInputListIndex = _chatInputList.Count - 1;
                }
                else
                {
                    if( isUp )
                    {
                        if( _chatInputListIndex <= 0 )
                        {
                            _chatInputListIndex = _chatInputList.Count - 1;
                        }
                        else
                        {
                            _chatInputListIndex--;
                        }
                    }
                    else
                    {
                        if( _chatInputListIndex >= _chatInputList.Count - 1 )
                        {
                            _chatInputListIndex = 0;
                        }
                        else
                        {
                            _chatInputListIndex++;
                        }
                    }
                }

                // Bind ChatInput
                if( !ChatInput.Equals( _chatInputList[ _chatInputListIndex ] ) )
                {
                    ChatInput = _chatInputList[ _chatInputListIndex ];
                }
            }
        }

        /// <summary>
        /// Copies the chat prompt.
        /// </summary>
        /// <param name="chat">The chat.</param>
        public void CopyChatPrompt( Chat chat )
        {
            try
            {
                Clipboard.SetText( chat.Name );
                StatusMessage = "Chat prompt copied to clipboard";
            }
            catch( Exception _ex )
            {
                StatusMessage = _ex.Message;
            }
        }

        /// <summary>
        /// Deletes the chat.
        /// </summary>
        /// <param name="chat">The chat.</param>
        public void DeleteChat( Chat chat )
        {
            try
            {
                // Works for deleting last one too
                _chatHistory.DeleteChat( chat.Name );
                ChatList.Remove( chat );
                if( _chatHistory.ChatList.IsEmpty( ) )
                {
                    var _newChat = _chatHistory.AddNewChat( );
                    ChatList.Add( _newChat );
                }

                SelectedChat = ChatList[ 0 ];
                StatusMessage = "Deleted the chat and selected the first chat in the list";
            }
            catch( Exception _ex )
            {
                StatusMessage = _ex.Message;
            }
        }

        /// <summary>
        /// Both XAML (important DataContext in DataContext.CopyMessageCommand) binding and menu item
        /// </summary>
        /// <param name="message">The message.</param>
        [ RelayCommand ]
        public void CopyMessage( Message message )
        {
            var _meIndex = SelectedChat.MessageList.IndexOf( message ) - 1;
            if( _meIndex >= 0 )
            {
                Clipboard.SetText(
                    $"Me: {SelectedChat.MessageList[ _meIndex ].Text}\n\n{message.Text}" );

                StatusMessage = "Both Me and Bot messages copied to clipboard";
            }
            else
            {
                Clipboard.SetText( $"Me: {message.Text}" );
                StatusMessage = "Me message copied to clipboard";
            }
        }

        /// <summary>
        /// Deletes the message.
        /// </summary>
        /// <param name="message">The message.</param>
        [ RelayCommand ]
        public void DeleteMessage( Message message )
        {
            try
            {
                SelectedChat.MessageList.Remove( message );
                StatusMessage = "Message deleted";
            }
            catch( Exception _ex )
            {
                StatusMessage = _ex.Message;
            }
        }

        /// <summary>
        /// Sends this instance.
        /// </summary>
        [ RelayCommand ]
        private async Task Send( )
        {
            if( IsCommandBusy )
            {
                return;
            }

            if( !ValidateInput( ChatInput, out var _prompt ) )
            {
                return;
            }

            try
            {
                SetCommandBusy( true, true );
                var _previousPrompts = BuildPreviousPrompts( );
                if( !string.IsNullOrEmpty( _previousPrompts ) )
                {
                    await Send( $"{_previousPrompts}\nMe: {_prompt}", _prompt );
                }
                else
                {
                    await Send( _prompt, _prompt );
                }

                PostProcessOnSend( _prompt );
                StatusMessage = "Ready";
            }
            catch( Exception _ex )
            {
                StatusMessage = _ex.Message;
            }

            SetCommandBusy( false, true );
            UpdateUIAction?.Invoke( UpdateUIEnum.SetFocusToChatInput );
            UpdateUIAction?.Invoke( UpdateUIEnum.MessageListViewScrollToBottom );
        }

        /// <summary>
        /// Explains this instance.
        /// </summary>
        [ RelayCommand ]
        private async Task Explain( )
        {
            await ExecutePost( "Explain" );
        }

        /// <summary>
        /// Translates to.
        /// </summary>
        [ RelayCommand ]
        private async Task TranslateTo( )
        {
            await ExecutePost( $"Translate to {SelectedLanguage}" );
        }

        /// <summary>
        /// Executes the post.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        private async Task ExecutePost( string prefix )
        {
            if( IsCommandBusy )
            {
                return;
            }

            if( !ValidateInput( ChatInput, out var _prompt ) )
            {
                return;
            }

            try
            {
                SetCommandBusy( true );
                AddNewChatIfNotExists( );
                _prompt = $"{prefix} '{_prompt}'";
                await Send( _prompt, _prompt );
                PostProcessOnSend( _prompt );
                SelectedChat.IsSend = false;
                StatusMessage = "Ready";
            }
            catch( Exception _ex )
            {
                StatusMessage = _ex.Message;
            }

            SetCommandBusy( false );
            UpdateUIAction?.Invoke( UpdateUIEnum.SetFocusToChatInput );
            UpdateUIAction?.Invoke( UpdateUIEnum.MessageListViewScrollToBottom );
        }

        /// <summary>
        /// Speaks this instance.
        /// </summary>
        [ RelayCommand ]
        private void Speak( )
        {
            try
            {
                SetCommandBusy( true );
                var _synthesizer = new SpeechSynthesizer( )
                {
                    Volume = 100,// 0...100
                    Rate = -2    // -10...10                    
                };

                _synthesizer.SelectVoiceByHints( IsFemaleVoice
                    ? VoiceGender.Female
                    : VoiceGender.Male, VoiceAge.Adult );

                _synthesizer.SpeakAsync( ChatInput );
                _synthesizer.Speak( ChatInput );
                StatusMessage = "Done";
            }
            catch( Exception _ex )
            {
                StatusMessage = _ex.Message;
            }

            SetCommandBusy( false );
            UpdateUIAction?.Invoke( UpdateUIEnum.SetFocusToChatInput );
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        private bool ValidateInput( string input, out string prompt )
        {
            prompt = input.Trim( );
            if( prompt.Length < 2 )
            {
                StatusMessage = "Prompt must be at least 2 characters";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Build 'context' for ChatGPT
        /// </summary>
        /// <returns></returns>
        private string BuildPreviousPrompts( )
        {
            var _previousPrompts = string.Empty;
            if( !SelectedChat.IsSend )
            {
                AddNewChatIfNotExists( );
            }
            else if( SelectedChat.MessageList.IsNotEmpty( ) )
            {
                foreach( var _message in SelectedChat.MessageList )
                {
                    _previousPrompts += $"{_message.Sender}: {_message.Text}";
                }
            }

            return _previousPrompts;
        }

        /// <summary>
        /// Sends the specified prompt.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <param name="promptDisplay">The prompt display.</param>
        private async Task Send( string prompt, string promptDisplay )
        {
            var _newMessage = new Message( "Me", promptDisplay, false );
            SelectedChat.AddMessage( _newMessage );
            StatusMessage = "Talking to ChatGPT API...please wait";
            if( IsStreamingMode )
            {
                await SendStreamingMode( prompt );
            }
            else
            {
                var _result = await DoSend( prompt );
                SelectedChat.AddMessage( "Bot", _result.Replace( "Bot: ", string.Empty ) );
            }

            ChatInput = string.Empty;
        }

        /// <summary>
        /// Does the send.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        private async Task<string> DoSend( string prompt )
        {
            // GPT-3.5
            var _completionResponse = await _gptService.CreateChatCompletionAsync( prompt );
            var _message = _completionResponse?.GetMessage( );
            return _message?.Content ?? string.Empty;
        }

        /// <summary>
        /// Sends the streaming mode.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        private async Task SendStreamingMode( string prompt )
        {
            var _message = SelectedChat.AddMessage( "Bot", string.Empty );
            await foreach( var _response in
                          _gptService.StreamChatCompletionAsync( prompt )
                              .ConfigureAwait( false ) )
            {
                if( _response is not null )
                {
                    var _responseText = _response.GetCompletionText( );
                    _message.Text = _message.Text + _responseText;
                }
            }
        }

        /// <summary>
        /// Posts the process on send.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        private void PostProcessOnSend( string prompt )
        {
            if( _chatHistory.IsNewChat( SelectedChat.Name ) )
            {
                _chatHistory.RenameNewChat( prompt );
            }

            if( !_chatInputList.Any( x => x.Equals( prompt ) ) )
            {
                _chatInputList.Add( prompt );
                _chatInputListIndex = _chatInputList.Count - 1;
            }

            if( !_chatInputList.Any( x => x.Equals( prompt ) ) )
            {
                _chatInputList.Add( prompt );
                _chatInputListIndex = _chatInputList.Count - 1;
            }
        }

        /// <summary>
        /// Expands the or collapse image pane.
        /// </summary>
        [ RelayCommand ]
        private void ExpandOrCollapseImagePane( )
        {
            ImagePaneVisibility = ( ImagePaneVisibility == Visibility.Visible )
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        /// <summary>
        /// Creates the image.
        /// </summary>
        [ RelayCommand ]
        private async Task CreateImage( )
        {
            if( !ValidateInput( ImageInput, out var _prompt ) )
            {
                return;
            }

            try
            {
                SetCommandBusy( true );
                StatusMessage = "Creating an image...please wait";
                ResultImage = await _gptService.CreateImageAsync( _prompt );
                StatusMessage = $"Processed image request for '{_prompt}'";
            }
            catch( Exception _ex )
            {
                StatusMessage = _ex.Message;
            }

            SetCommandBusy( false );
        }

        /// <summary>
        /// ESC key maps to ClearChatInputCommand
        /// </summary>
        [ RelayCommand ]
        private void ClearChatInput( )
        {
            ChatInput = string.Empty;
        }

        /// <summary>
        /// Clears the image input.
        /// </summary>
        [ RelayCommand ]
        private void ClearImageInput( )
        {
            ImageInput = string.Empty;
        }

        /// <summary>
        /// Sets the command busy.
        /// </summary>
        /// <param name="isCommandBusy">if set to <c>true</c> [is command busy].</param>
        /// <param name="isSendCommand">if set to <c>true</c> [is send command].</param>
        private void SetCommandBusy( bool isCommandBusy, bool isSendCommand = false )
        {
            IsCommandBusy = isCommandBusy;
            OnPropertyChanged( nameof( IsCommandNotBusy ) );
            if( isSendCommand )
            {
                IsSendCommandBusy = isCommandBusy;
            }
            else
            {
                if( IsCommandBusy )
                {
                    Mouse.OverrideCursor = Cursors.Wait;
                }
                else
                {
                    Mouse.OverrideCursor = null;
                }
            }
        }

        /// <summary>
        /// partial method (CommunityToolkit MVVM)
        /// </summary>
        /// <param name="value">The value.</param>
        partial void OnSelectedChatChanged( Chat value )
        {
            if( value != null )
            {
                UpdateUIAction?.Invoke( UpdateUIEnum.SetupMessageListViewScrollViewer );
            }
        }
    }
}