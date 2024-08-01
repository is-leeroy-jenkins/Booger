// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-01-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-01-2024
// ******************************************************************************************
// <copyright file="MessageDialog.xaml.cs" company="Terry D. Eppler">
//     Booger is a quick & dirty application in C sharp for interacting with the OpenAI GPT API.
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
//   MessageDialog.xaml.cs
// </summary>
// ******************************************************************************************

namespace Badjao
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    public class MessageDialog : Window
    {
        /// <summary>
        /// The locked object
        /// </summary>
        private object _path;

        /// <summary>
        /// The busy
        /// </summary>
        private bool _busy;

        /// <summary>
        /// The status update
        /// </summary>
        private Action _statusUpdate;

        /// <summary>
        /// 
        /// </summary>
        private protected int _time;

        /// <summary>
        /// 
        /// </summary>
        private protected int _seconds;

        /// <summary>
        /// The title
        /// </summary>
        private protected string _titleText;

        /// <summary>
        /// The message
        /// </summary>
        private protected string _messageText;

        /// <summary>
        /// Gets the title text.
        /// </summary>
        /// <value>
        /// The title text.
        /// </value>
        public string TitleText
        {
            get
            {
                return _titleText;
            }
            private protected set
            {
                _titleText = value;
            }
        }

        /// <summary>
        /// Gets the message text.
        /// </summary>
        /// <value>
        /// The message text.
        /// </value>
        public string MessageText
        {
            get
            {
                return _messageText;
            }
            private protected set
            {
                _messageText = value;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Badger.MessageDialog" /> class.
        /// </summary>
        public MessageDialog( )
            : base( )
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Badger.MessageDialog" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageDialog( string message )
            : this( )
        {
        }
    }
}