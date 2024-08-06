// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-05-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-05-2024
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
    using System.Linq;
    using System.Windows;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;

    public partial class HistoryViewModel : ObservableObject
    {
        private IHistoryRepo _historyRepo;

        public HistoryViewModel( IHistoryRepo historyRepo )
        {
            _historyRepo = historyRepo;
            ChatList = new ObservableCollection<Chat>( _historyRepo.LoadChatList( ) );
            RegisterAddChatMessage( );
        }

        public string DBConfigInfo
        {
            get
            {
                return _historyRepo.DBConfigInfo;
            }
        }

        public ObservableCollection<Chat> ChatList { get; }

        [ ObservableProperty ]
        private Chat? _selectedChat;

        [ ObservableProperty ]
        private string _statusMessage = "List of history chats";

        private void RegisterAddChatMessage( )
        {
            WeakReferenceMessenger.Default.Register<AddChatMessage>( this, ( recipient, message ) =>
            {
                // Received from LiveChatViewModel to HistoryViewModel
                // WeakReferenceMessenger.Default.Send(new AddChatMessage(chat));
                if( message != null )
                {
                    var chat = message.Chat;
                    var existingChat = ChatList.FirstOrDefault( x => x.Id == chat.Id );
                    if( existingChat != null )
                    {
                        ChatList.Remove( existingChat );
                    }

                    ChatList.Add( chat );
                }
            } );
        }

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

        [ RelayCommand ]
        public void CopyMessage( Message message )
        {
            if( SelectedChat != null )
            {
                var meIndex = SelectedChat.MessageList.IndexOf( message ) - 1;
                if( meIndex >= 0 )
                {
                    Clipboard.SetText(
                        $"Me: {SelectedChat.MessageList[ meIndex ].Text}\n\n{message.Text}" );

                    StatusMessage = "Both Me and Bot messages copied to clipboard";
                }
            }
        }

        private bool ConfirmDelete( Chat chat )
        {
            return MessageBox.Show(
                    $"Are you sure you want to delete '{chat.Name}' (PK: {chat.Id})",
                    "Confirm Delete",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No )
                == MessageBoxResult.Yes;
        }
    }
}