// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="ChatSessionConfigDialog.xaml.cs" company="Terry D. Eppler">
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
//   ChatSessionConfigDialog.xaml.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using CommunityToolkit.Mvvm.Input;

    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for ChatSessionConfigDialog.xaml
    /// </summary>
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    public partial class ChatSessionConfigDialog : Window
    {
        private protected ChatSessionModel _session;

        private protected NoteService _noteService;

        public ChatSessionConfigDialog( ChatSessionModel session )
        {
            _session = session;
            DataContext = this;
            _noteService = App.GetService<NoteService>( );
            InitializeComponent( );
            if( !session.EnableChatContext.HasValue )
            {
                EnableChatContextComboBox.SelectedIndex = 0;
            }
            else if( session.EnableChatContext.Value )
            {
                EnableChatContextComboBox.SelectedIndex = 1;
            }
            else
            {
                EnableChatContextComboBox.SelectedIndex = 2;
            }
        }

        public ChatSessionModel Session
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
            }
        }

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

        public ObservableCollection<bool?> _enableChatContextValues =
            new ObservableCollection<bool?>( )
            {
                null,
                true,
                false
            };

        [ RelayCommand ]
        public void AddSystemMessage( )
        {
            _session.SystemMessages.Add( new ValueWrapper<string>( "New System Message" ) );
        }

        [ RelayCommand ]
        public void RemoveSystemMessage( )
        {
            if( _session.SystemMessages.Count > 0 )
            {
                _session.SystemMessages.RemoveAt( _session.SystemMessages.Count - 1 );
            }
        }

        [ RelayCommand ]
        public void Accept( )
        {
            DialogResult = true;
            Close( );
        }

        private void OnComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var _combo = sender as MetroComboBox;
            if( _combo?.SelectedItem is not ComboBoxItem _item )
            {
                return;
            }

            if( _item.Tag is bool _value ) 
            {
                _session.EnableChatContext = _value;
            }
            else
            {
                _session.EnableChatContext = null;
            }
        }
    }
}