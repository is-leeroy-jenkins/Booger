// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-06-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-06-2024
// ******************************************************************************************
// <copyright file="LiveChatViewModel.cs" company="Terry D. Eppler">
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
//   LiveChatViewModel.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Speech.Synthesis;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Whetstone.ChatGPT;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:CommunityToolkit.Mvvm.ComponentModel.ObservableObject" />
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public partial class LiveChatViewModel : ObservableObject
    {
        /// <summary>
        /// The history repo
        /// </summary>
        private IHistoryRepo _historyRepo;

        /// <summary>
        /// The chat GPT service
        /// </summary>
        private ChatGptService _chatGptService;

        /// <summary>
        /// The live chat manager
        /// </summary>
        private LiveChatManager _liveChatManager = new LiveChatManager( );

        /// <summary>
        /// The chat input list
        /// </summary>
        private List<string> _chatInputList = new List<string>( );

        /// <summary>
        /// The chat input list index
        /// </summary>
        private int _chatInputListIndex = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveChatViewModel"/> class.
        /// </summary>
        /// <param name="historyRepo">The history repo.</param>
        /// <param name="chatGPTService">The chat GPT service.</param>
        public LiveChatViewModel( IHistoryRepo historyRepo, ChatGptService chatGPTService )
        {
            _historyRepo = historyRepo;
            _chatGptService = chatGPTService;
            SelectedChat = _liveChatManager.AddNewChat( );
            ChatList = new ObservableCollection<Chat>( _liveChatManager.ChatList );
            ChatInput = "Please list top 5 ChatGPT prompts";

            // Uncomment this to insert testing data
            // DevDebugInitialize();
        }

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
        ///   <c>true</c> if this instance is command not busy; otherwise, <c>false</c>.
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
        /// Gets the chat list.
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
        /// The add to history button enabled
        /// </summary>
        [ ObservableProperty ]
        public bool _addToHistoryButtonEnabled;

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
        public string _selectedLang = "Spanish";

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

        // Also RelayCommand from AppBar
        /// <summary>
        /// Creates new chat.
        /// </summary>
        [ RelayCommand ]
        private void NewChat( )
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
            catch( Exception ex )
            {
                StatusMessage = ex.Message;
            }
        }

        /// <summary>
        /// Devs the debug initialize.
        /// </summary>
        private void DevDebugInitialize( )
        {
            var _prompt = "TestPrompt1";
            var _promptDisplay = _prompt;
            var _newMessage = new Message( "Me", _promptDisplay );
            SelectedChat.AddMessage( _newMessage );
            var _result = "TestPrompt1 result";
            SelectedChat.AddMessage( "Bot", _result.Replace( "Bot: ", string.Empty ) );
            PostProcessOnSend( _prompt );
            var _newChat = _liveChatManager.AddNewChat( );
            ChatList.Add( _newChat );
            SelectedChat = _newChat;
            _prompt = "TestPrompt2";
            _promptDisplay = _prompt;
            _newMessage = new Message( "Me", _promptDisplay );
            SelectedChat.AddMessage( _newMessage );
            _result = "TestPrompt2 result";
            SelectedChat.AddMessage( "Bot", _result.Replace( "Bot: ", string.Empty ) );
            PostProcessOnSend( _prompt );
        }

        /// <summary>
        /// Adds the new chat if not exists.
        /// </summary>
        /// <returns></returns>
        private bool AddNewChatIfNotExists( )
        {
            if( _liveChatManager.NewChatExists )
            {
                return false;
            }

            // Note: 'New Chat' will be renamed after it's used
            var _newChat = _liveChatManager.AddNewChat( );
            ChatList.Add( _newChat );
            SelectedChat = _newChat;
            return true;
        }

        // Up/Previous or down/next chat input in the chat input list
        /// <summary>
        /// Previouses the next chat input.
        /// </summary>
        /// <param name="isUp">if set to <c>true</c> [is up].</param>
        public void PrevNextChatInput( bool isUp )
        {
            if( _chatInputList.IsNotEmpty( ) )
            {
                if( ChatInput.IsBlank( ) )
                {
                    // Pick the just used last one
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
        /// Adds to history.
        /// </summary>
        /// <param name="chat">The chat.</param>
        [ RelayCommand ]
        private void AddToHistory( Chat chat )
        {
            _historyRepo.AddChat( chat );
            FixupChatId( chat );

            // Add chat to HistoryViewModel
            WeakReferenceMessenger.Default.Send( new AddChatMessage( chat ) );
            StatusMessage = $"'{chat.Name}' (PK: {chat.Id}) added to History tab";
        }

        // If DB is configured, chat.Id will be PK (i.e. DB insert already called)    
        /// <summary>
        /// Fixups the chat identifier.
        /// </summary>
        /// <param name="chat">The chat.</param>
        private void FixupChatId( Chat chat )
        {
            if( chat.Id == 0 )
            {
                // DB not configured, assign a max + 1
                chat.Id = ChatList.Count == 0
                    ? 1
                    : ChatList.Max( x => x.Id ) + 1;
            }
        }

        // Both XAML (important DataContext in DataContext.CopyMessageCommand) binding and menu item
        /// <summary>
        /// Copies the message.
        /// </summary>
        /// <param name="message">The message.</param>
        [ RelayCommand ]
        public void CopyMessage( Message message )
        {
            var _meIndex = SelectedChat.MessageList.IndexOf( message ) - 1;
            if( _meIndex >= 0 )
            {
                var _text = $"{SelectedChat.MessageList[ _meIndex ].Text}\n\n{message.Text}";
                var _prefix = "Me: ";
                var _selection = _prefix + _text;
                Clipboard.SetText( _selection );
                StatusMessage = "Both Me and Bot messages copied to clipboard";
            }
            else
            {
                Clipboard.SetText( $"Me: {message.Text}" );
                StatusMessage = "Me message copied to clipboard";
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
            catch( Exception ex )
            {
                StatusMessage = ex.Message;
            }

            SetCommandBusy( false, true );

            // Always set focus to ChatInput after Send()
            UpdateUIAction?.Invoke( UpdateUIEnum.SetFocusToChatInput );

            // Always ScrollToBottom
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
            await ExecutePost( $"Translate to {SelectedLang}" );
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

                // 'Explain' or 'Translate to' always uses a new chat
                AddNewChatIfNotExists( );
                _prompt = $"{prefix} '{_prompt}'";
                await Send( _prompt, _prompt );
                PostProcessOnSend( _prompt );

                // Ensure this is marked for logic in BuildPreviousPrompts()
                SelectedChat.IsSend = false;
                StatusMessage = "Ready";
            }
            catch( Exception ex )
            {
                StatusMessage = ex.Message;
            }

            SetCommandBusy( false );

            // Always set focus to ChatInput after Send()
            UpdateUIAction?.Invoke( UpdateUIEnum.SetFocusToChatInput );

            // Always ScrollToBottom
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

                // Note: need to have voices installedL: Win-Key + I, Time & language -> Speech
                var _synthesizer = new SpeechSynthesizer( )
                {
                    Volume = 100,// 0...100
                    Rate = -2    // -10...10                    
                };

                _synthesizer.SelectVoiceByHints( IsFemaleVoice
                    ? VoiceGender.Female
                    : VoiceGender.Male, VoiceAge.Adult );

                // Asynchronous / Synchronous
                _synthesizer.SpeakAsync( ChatInput );

                //synthesizer.Speak(ChatInput);
                StatusMessage = "Done";
            }
            catch( Exception ex )
            {
                StatusMessage = ex.Message;
            }

            SetCommandBusy( false );

            // Always set focus to ChatInput
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

        // Build 'context' for ChatGPT
        /// <summary>
        /// Builds the previous prompts.
        /// </summary>
        /// <returns></returns>
        private string BuildPreviousPrompts( )
        {
            var _previousPrompts = string.Empty;
            if( !SelectedChat.IsSend )
            {
                // We are on 'Explain' or 'Translate to', so auto-create a new chat
                AddNewChatIfNotExists( );
            }
            else if( SelectedChat.MessageList.IsNotEmpty( ) )
            {
                // Continue with previous chat by sending previousPrompts
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
            var _newMessage = new Message( "Me", promptDisplay );
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

            // Clear the ChatInput field
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
            var _completionResponse = await _chatGptService.CreateChatCompletionAsync( prompt );
            var _message = _completionResponse?.GetMessage( );
            return _message?.Content ?? string.Empty;
        }

        /// <summary>
        /// Sends the streaming mode.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        private async Task SendStreamingMode( string prompt )
        {
            // Append with message.Text below
            var _message = SelectedChat.AddMessage( "Bot", string.Empty );

            // GPT-3.5
            await foreach( var _response in
                _chatGptService.StreamChatCompletionAsync( prompt ).ConfigureAwait( false ) )
            {
                if( _response is not null )
                {
                    var _responseText = _response.GetCompletionText( );
                    _message.Text += _responseText ?? string.Empty;
                }
            }
        }

        /// <summary>
        /// Posts the process on send.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        private void PostProcessOnSend( string prompt )
        {
            // Handle new chat
            if( _liveChatManager.IsNewChat( SelectedChat.Name ) )
            {
                // After this call, SelectedChat.Name updated on the left panel because SelectedChat is/wraps the new chat                
                _liveChatManager.RenameNewChat( prompt );
                UpdateAddToHistoryButton( SelectedChat );
            }

            // Handle chat input list
            if( !_chatInputList.Any( x => x.Equals( prompt ) ) )
            {
                _chatInputList.Add( prompt );
                _chatInputListIndex = _chatInputList.Count - 1;
            }

            // Handle user input list
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
            ImagePaneVisibility = ImagePaneVisibility == Visibility.Visible
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

                // Will reject query of an image of a real person
                ResultImage = await _chatGptService.CreateImageAsync( _prompt );
                StatusMessage = $"Processed image request for '{_prompt}'";
            }
            catch( Exception ex )
            {
                StatusMessage = ex.Message;
            }

            SetCommandBusy( false );
        }

        // ESC key maps to ClearChatInputCommand
        /// <summary>
        /// Clears the chat input.
        /// </summary>
        [ RelayCommand ]
        private void ClearChatInput( )
        {
            ChatInput = string.Empty;
        }

        // ESC key maps to ClearImageInputCommand
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
            OnPropertyChanged( nameof( LiveChatViewModel.IsCommandNotBusy ) );
            if( !isSendCommand )
            {
                Mouse.OverrideCursor = IsCommandBusy
                    ? Cursors.Wait
                    : null;
            }
        }

        // partial method (CommunityToolkit MVVM)
        /// <summary>
        /// Called when [selected chat changed].
        /// </summary>
        /// <param name="value">The value.</param>
        partial void OnSelectedChatChanged( Chat value )
        {
            UpdateAddToHistoryButton( value );
            if( value != null )
            {
                // Re-setup on selected chat changed
                UpdateUIAction?.Invoke( UpdateUIEnum.SetupMessageListViewScrollViewer );
            }
        }

        /// <summary>
        /// Updates the add to history button.
        /// </summary>
        /// <param name="value">The value.</param>
        private void UpdateAddToHistoryButton( Chat value )
        {
            AddToHistoryButtonEnabled = value != null && !_liveChatManager.IsNewChat( value.Name );
        }
    }
}