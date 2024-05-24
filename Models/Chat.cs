// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 05-24-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        05-24-2024
// ******************************************************************************************
// <copyright file="Chat.cs" company="Terry D. Eppler">
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
//   Chat.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using CommunityToolkit.Mvvm.ComponentModel;

    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public partial class Chat : ObservableObject
    {
        /// <summary>
        /// The name
        /// </summary>
        [ObservableProperty]
        private string _name = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is send.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is send; otherwise, <c>false</c>.
        /// </value>
        public bool IsSend { get; set; } = true;

        /// <summary>
        /// Gets the message list.
        /// </summary>
        /// <value>
        /// The message list.
        /// </value>
        public ObservableCollection<Message> MessageList { get; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Chat"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public Chat( string name )
        {
            Name = name;
            MessageList = new ObservableCollection<Message>( );
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public Message AddMessage( string sender, string text )
        {
            var message = new Message( sender, text, true );
            AddMessage( message );
            return message;
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void AddMessage( Message message )
        {
            MessageList.Add( message );
        }
    }
}