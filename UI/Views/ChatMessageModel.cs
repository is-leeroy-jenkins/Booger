// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="ChatMessageModel.cs" company="Terry D. Eppler">
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
//   ChatMessageModel.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="CommunityToolkit.Mvvm.ComponentModel.ObservableObject" />
    public partial class ChatMessageModel : ObservableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageModel"/> class.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="content">The content.</param>
        public ChatMessageModel( string role, string content )
        {
            _role = role;
            _content = content;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageModel"/> class.
        /// </summary>
        /// <param name="storage">The storage.</param>
        public ChatMessageModel( ChatMessage storage )
        {
            Storage = storage;
            _role = storage.Role;
            _content = storage.Content;
        }

        /// <summary>
        /// Gets or sets the storage.
        /// </summary>
        /// <value>
        /// The storage.
        /// </value>
        public ChatMessage Storage { get; set; }

        /// <summary>
        /// The role
        /// </summary>
        [ObservableProperty ]
        private string _role = "user";

        /// <summary>
        /// The content
        /// </summary>
        [ObservableProperty ]
        [ NotifyPropertyChangedFor( nameof( SingleLineContent ) ) ]
        private string _content = string.Empty;

        /// <summary>
        /// Gets the content of the single line.
        /// </summary>
        /// <value>
        /// The content of the single line.
        /// </value>
        public string SingleLineContent
        {
            get
            {
                return ( Content ?? string.Empty ).Replace( '\n', ' ' ).Replace( '\r', ' ' );
            }
        }

        /// <summary>
        /// The is editing
        /// </summary>
        [ObservableProperty ]
        [ NotifyPropertyChangedFor( nameof( IsReadOnly ) ) ]
        private bool _isEditing;

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get
            {
                return !IsEditing;
            }
        }

        /// <summary>
        /// Gets the chat storage service.
        /// </summary>
        /// <value>
        /// The chat storage service.
        /// </value>
        private static ChatStorageService ChatStorageService { get; } =
            App.GetService<ChatStorageService>( );

        // 用于将数据同步到数据存储
        /// <summary>
        /// Raises the <see cref="E:CommunityToolkit.Mvvm.ComponentModel.ObservableObject.PropertyChanged" /> event.
        /// </summary>
        /// <param name="e">The input <see cref="T:System.ComponentModel.PropertyChangedEventArgs" /> instance.</param>
        protected override void OnPropertyChanged( PropertyChangedEventArgs e )
        {
            base.OnPropertyChanged( e );

            // 如果有后备存储, 则使用存储服务保存
            if( Storage != null )
            {
                Storage = Storage with
                {
                    Role = Role,
                    Content = Content
                };

                ChatStorageService.SaveMessage( Storage );
            }
        }

        #region 布局用的一些属性
        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName
        {
            get
            {
                return string.Equals( Role, "user", StringComparison.CurrentCultureIgnoreCase )
                    ? "Me"
                    : "Bot";
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is me.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is me; otherwise, <c>false</c>.
        /// </value>
        public bool IsMe
        {
            get
            {
                return "Me".Equals( DisplayName, StringComparison.CurrentCultureIgnoreCase );
            }
        }

        /// <summary>
        /// Gets the self alignment.
        /// </summary>
        /// <value>
        /// The self alignment.
        /// </value>
        public HorizontalAlignment SelfAlignment
        {
            get
            {
                return IsMe
                    ? HorizontalAlignment.Right
                    : HorizontalAlignment.Left;
            }
        }

        /// <summary>
        /// Gets the self cornor radius.
        /// </summary>
        /// <value>
        /// The self cornor radius.
        /// </value>
        public CornerRadius SelfCornorRadius
        {
            get
            {
                return IsMe
                    ? new CornerRadius( 5, 0, 5, 5 )
                    : new CornerRadius( 0, 5, 5, 5 );
            }
        }
        #endregion

        #region 页面用的一些指令
        /// <summary>
        /// Copies this instance.
        /// </summary>
        [RelayCommand ]
        public void Copy( )
        {
            Clipboard.SetText( Content );
        }

        /// <summary>
        /// Starts the edit.
        /// </summary>
        [RelayCommand ]
        public void StartEdit( )
        {
            IsEditing = true;
        }

        /// <summary>
        /// Ends the edit.
        /// </summary>
        [RelayCommand ]
        public void EndEdit( )
        {
            IsEditing = false;
        }
        #endregion
    }
}