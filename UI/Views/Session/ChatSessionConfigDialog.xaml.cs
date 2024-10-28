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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using CommunityToolkit.Mvvm.Input;

    /// <inheritdoc />
    /// <summary>
    /// Interaction logic for ChatSessionConfigDialog.xaml
    /// </summary>
    public partial class ChatSessionConfigDialog : Window
    {
        public ChatSessionConfigDialog( ChatSessionModel session )
        {
            Session = session;
            DataContext = this;
            NoteService =
                App.GetService<NoteService>( );

            InitializeComponent( );
            if( !session.EnableChatContext.HasValue )
            {
                //EnableChatContextComboBox.SelectedIndex = 0;
            }
            else if( session.EnableChatContext.Value )
            {
                //EnableChatContextComboBox.SelectedIndex = 1;
            }
            else
            {
                //EnableChatContextComboBox.SelectedIndex = 2;
            }
        }

        public ChatSessionModel Session { get; }

        public NoteService NoteService { get; }

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
            Session.SystemMessages.Add( new ValueWrapper<string>( "New system message" ) );
        }

        [ RelayCommand ]
        public void RemoveSystemMessage( )
        {
            if( Session.SystemMessages.Count > 0 )
            {
                Session.SystemMessages.RemoveAt( Session.SystemMessages.Count - 1 );
            }
        }

        [ RelayCommand ]
        public void Accept( )
        {
            DialogResult = true;
            Close( );
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var _combo = sender as MetroComboBox;
            if(_combo.SelectedItem is not ComboBoxItem _item)
            {
                return;
            }

            if(_item.Tag is bool _value)
            {
                Session.EnableChatContext = _value;
            }
            else
            {
                Session.EnableChatContext = null;
            }
        }
    }
}