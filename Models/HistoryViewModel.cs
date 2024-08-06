// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-06-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-06-2024
// ******************************************************************************************
// <copyright file="HistoryViewModel.cs" company="Terry D. Eppler">
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
//   HistoryViewModel.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:CommunityToolkit.Mvvm.ComponentModel.ObservableObject" />
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    public partial class HistoryViewModel : ObservableObject
    {
        /// <summary>
        /// The history repo
        /// </summary>
        private IHistoryRepo _historyRepo;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="HistoryViewModel"/> class.
        /// </summary>
        /// <param name="historyRepo">The history repo.</param>
        [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
        public HistoryViewModel( IHistoryRepo historyRepo )
        {
            _historyRepo = historyRepo;
            ChatList = new ObservableCollection<Chat>( _historyRepo.LoadChatList( ) );
            RegisterAddChatMessage( );
        }

        /// <summary>
        /// Gets the database configuration information.
        /// </summary>
        /// <value>
        /// The database configuration information.
        /// </value>
        public string DBConfigInfo
        {
            get
            {
                return _historyRepo.DBConfigInfo;
            }
        }

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
        /// The status message
        /// </summary>
        [ ObservableProperty ]
        private string _statusMessage = "List of history chats";

        /// <summary>
        /// Registers the add chat message.
        /// </summary>
        private void RegisterAddChatMessage( )
        {
            WeakReferenceMessenger.Default.Register<AddChatMessage>( this, ( recipient, message ) =>
            {
                // Received from LiveChatViewModel to HistoryViewModel
                // WeakReferenceMessenger.Default.Send(new AddChatMessage(chat));
                if( message != null )
                {
                    var _chat = message.Chat;
                    var _existingChat = ChatList.FirstOrDefault( x => x.Id == _chat.Id );
                    if( _existingChat != null )
                    {
                        ChatList.Remove( _existingChat );
                    }

                    ChatList.Add( _chat );
                }
            } );
        }

        /// <summary>
        /// Deletes the history chat.
        /// </summary>
        /// <param name="chat">The chat.</param>
        [ RelayCommand ]
        private void DeleteHistoryChat( Chat chat )
        {
            try
            {
                if( !ConfirmDelete( chat ) )
                {
                    return;
                }

                ChatList.Remove( chat );
                _historyRepo.DeleteChat( chat );
                StatusMessage = $"'{chat.Name}' (PK: {chat.Id}) deleted";
            }
            catch( Exception ex )
            {
                StatusMessage = ex.Message;
            }
        }

        /// <summary>
        /// Copies the message.
        /// </summary>
        /// <param name="message">The message.</param>
        [ RelayCommand ]
        public void CopyMessage( Message message )
        {
            if( SelectedChat != null )
            {
                var _meIndex = SelectedChat.MessageList.IndexOf( message ) - 1;
                if( _meIndex >= 0 )
                {
                    var _prefix = $"{SelectedChat.MessageList[ _meIndex ].Text}\n\n{message.Text}";
                    var _text = "Me: " + _prefix;
                    Clipboard.SetText( _text );
                    StatusMessage = "Both Me and Bot messages copied to clipboard";
                }
            }
        }

        /// <summary>
        /// Confirms the delete.
        /// </summary>
        /// <param name="chat">The chat.</param>
        /// <returns></returns>
        private bool ConfirmDelete( Chat chat )
        {
            return MessageBox.Show(
                    $"Are you sure you want to delete '{chat.Name}' (PK: {chat.Id})",
                    "Confirm Delete",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No )
                == MessageBoxResult.Yes;
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