// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="ChatPageModel.cs" company="Terry D. Eppler">
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
//   ChatPageModel.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="CommunityToolkit.Mvvm.ComponentModel.ObservableObject" />
    public partial class ChatPageModel : ObservableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatPageModel"/> class.
        /// </summary>
        /// <param name="chatStorageService">The chat storage service.</param>
        public ChatPageModel( ChatStorageService chatStorageService )
        {
            _chatStorageService = chatStorageService;
            Messages.CollectionChanged += ( s, e ) =>
            {
                OnPropertyChanged( nameof( ChatPageModel.LastMessage ) );
            };
        }

        /// <summary>
        /// The input box text
        /// </summary>
        [ ObservableProperty ]
        private string _inputBoxText = string.Empty;

        /// <summary>
        /// The chat storage service
        /// </summary>
        private readonly ChatStorageService _chatStorageService;

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public ObservableCollection<ChatMessageModel> Messages { get; } =
            new ObservableCollection<ChatMessageModel>( );

        /// <summary>
        /// Gets the last message.
        /// </summary>
        /// <value>
        /// The last message.
        /// </value>
        public ChatMessageModel LastMessage
        {
            get
            {
                return Messages.Count > 0
                    ? Messages.Last( )
                    : null;
            }
        }

        /// <summary>
        /// Deletes the message.
        /// </summary>
        /// <param name="messageModel">The message model.</param>
        [ RelayCommand ]
        public void DeleteMessage( ChatMessageModel messageModel )
        {
            Messages.Remove( messageModel );
            if( messageModel.Storage != null )
            {
                _chatStorageService.DeleteMessage( messageModel.Storage );
            }
        }

        /// <summary>
        /// Deletes the messages above.
        /// </summary>
        /// <param name="messageModel">The message model.</param>
        [ RelayCommand ]
        public void DeleteMessagesAbove( ChatMessageModel messageModel )
        {
            while( true )
            {
                var _index = Messages.IndexOf( messageModel );
                if( _index <= 0 )
                {
                    break;
                }

                Messages.RemoveAt( 0 );
            }

            if( messageModel.Storage != null )
            {
                _chatStorageService.DeleteMessagesBefore( messageModel.Storage.SessionId,
                    messageModel.Storage.Timestamp );
            }
        }

        /// <summary>
        /// Deletes the messages below.
        /// </summary>
        /// <param name="messageModel">The message model.</param>
        [ RelayCommand ]
        public void DeleteMessagesBelow( ChatMessageModel messageModel )
        {
            while( true )
            {
                var _index = Messages.IndexOf( messageModel );
                if( _index == -1
                    || _index == Messages.Count - 1 )
                {
                    break;
                }

                Messages.RemoveAt( _index + 1 );
            }

            if( messageModel.Storage != null )
            {
                _chatStorageService.DeleteMessagesAfter( messageModel.Storage.SessionId,
                    messageModel.Storage.Timestamp );
            }
        }
    }
}