// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="ChatSessionModel.cs" company="Terry D. Eppler">
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
//   ChatSessionModel.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="CommunityToolkit.Mvvm.ComponentModel.ObservableObject" />
    public partial class ChatSessionModel : ObservableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatSessionModel"/> class.
        /// </summary>
        /// <param name="storage">The storage.</param>
        public ChatSessionModel( ChatSession storage )
        {
            Storage = storage;
            SetupStorage( storage );
        }

        /// <summary>
        /// Setups the storage.
        /// </summary>
        /// <param name="storage">The storage.</param>
        private void SetupStorage( ChatSession storage )
        {
#pragma warning disable MVVMTK0034
            _id = storage.Id;
            _name = storage.Name;
            _enableChatContext = storage.EnableChatContext;
            _systemMessages = storage.SystemMessages.WrapCollection( );
#pragma warning restore MVVMTK0034
        }

        /// <summary>
        /// Gets or sets the storage.
        /// </summary>
        /// <value>
        /// The storage.
        /// </value>
        public ChatSession Storage
        {
            get
            {
                return _storage;
            }
            set
            {
                _storage = value;
                if( value != null )
                {
                    SetupStorage( value );
                }
            }
        }

        /// <summary>
        /// The identifier
        /// </summary>
        [ ObservableProperty ]
        private Guid _id;

        /// <summary>
        /// The name
        /// </summary>
        [ ObservableProperty ]
        private string _name = string.Empty;

        /// <summary>
        /// The enable chat context
        /// </summary>
        [ ObservableProperty ]
        private bool? _enableChatContext;

        /// <summary>
        /// The system messages
        /// </summary>
        [ ObservableProperty ]
        private ObservableCollection<ValueWrapper<string>> _systemMessages
            = new ObservableCollection<ValueWrapper<string>>( );

        /// <summary>
        /// The is editing
        /// </summary>
        [ ObservableProperty ]
        [ NotifyPropertyChangedFor( nameof( IsReadOnly ) ) ]
        private bool _isEditing;

        /// <summary>
        /// The storage
        /// </summary>
        private ChatSession _storage;

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
        /// Gets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public ChatPage Page
        {
            get
            {
                return ChatPageService.GetPage( Id );
            }
        }

        /// <summary>
        /// Gets the page model.
        /// </summary>
        /// <value>
        /// The page model.
        /// </value>
        public ChatPageModel PageModel
        {
            get
            {
                return Page.ViewModel;
            }
        }

        /// <summary>
        /// Gets the chat page service.
        /// </summary>
        /// <value>
        /// The chat page service.
        /// </value>
        private static ChatPageService ChatPageService { get; } =
            App.GetService<ChatPageService>( );

        /// <summary>
        /// Gets the chat storage service.
        /// </summary>
        /// <value>
        /// The chat storage service.
        /// </value>
        private static ChatStorageService ChatStorageService { get; } =
            App.GetService<ChatStorageService>( );

        /// <summary>
        /// Raises the <see cref="E:CommunityToolkit.Mvvm.ComponentModel.ObservableObject.PropertyChanged" /> event.
        /// </summary>
        /// <param name="e">The input <see cref="T:System.ComponentModel.PropertyChangedEventArgs" /> instance.</param>
        protected override void OnPropertyChanged( PropertyChangedEventArgs e )
        {
            base.OnPropertyChanged( e );
            SyncStorage( );
        }

        /// <summary>
        /// Starts the edit.
        /// </summary>
        [ RelayCommand ]
        public void StartEdit( )
        {
            IsEditing = true;
        }

        /// <summary>
        /// Ends the edit.
        /// </summary>
        [ RelayCommand ]
        public void EndEdit( )
        {
            IsEditing = false;
        }

        /// <summary>
        /// Configurations this instance.
        /// </summary>
        [ RelayCommand ]
        public void Config( )
        {
            var _dialog =
                new ChatSessionConfigDialog( this );

            if( Application.Current.MainWindow is Window _window )
            {
                _dialog.Owner = _window;
            }

            if( _dialog.ShowDialog( ) ?? false )
            {
                SyncStorage( );
            }
        }

        /// <summary>
        /// Synchronizes the storage.
        /// </summary>
        [ RelayCommand ]
        public void SyncStorage( )
        {
            if( Storage != null )
            {
                Storage = Storage with
                {
                    Name = Name,
                    EnableChatContext = EnableChatContext,
                    SystemMessages = SystemMessages.UnwrapToArray( )
                };

                ChatStorageService.SaveSession( Storage );
            }
        }
    }
}