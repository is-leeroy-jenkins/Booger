// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-06-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-06-2024
// ******************************************************************************************
// <copyright file="LiveChatUserControl.xaml.cs" company="Terry D. Eppler">
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
//   LiveChatUserControl.xaml.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using ModernWpf.Controls;
    using Syncfusion.SfSkinManager;
    using ToastNotifications;
    using ToastNotifications.Lifetime;
    using ToastNotifications.Messages;
    using ToastNotifications.Position;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.Controls.UserControl" />
    /// <seealso cref="T:System.Windows.Markup.IComponentConnector" />
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "UsePatternMatching" ) ]
    [ SuppressMessage( "ReSharper", "UnusedVariable" ) ]
    [ SuppressMessage( "ReSharper", "SuggestBaseTypeForParameter" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    public partial class LiveChatUserControl : UserControl
    {
        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <summary>
        /// The copy message
        /// </summary>
        private const string _copyMessage = "Copy";

        /// <summary>
        /// The already loaded
        /// </summary>
        private bool _alreadyLoaded;

        /// <summary>
        /// The chat ListView scroll viewer
        /// </summary>
        private ScrollViewer _chatScrollViewer;

        /// <summary>
        /// The message ListView scroll viewer
        /// </summary>
        private ScrollViewer _messageScrollViewer;

        /// <summary>
        /// The message context menu
        /// </summary>
        private ContextMenu _messageContextMenu;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Booger.LiveChatUserControl" /> class.
        /// </summary>
        public LiveChatUserControl( )
        {
            // Theme Properties
            SfSkinManager.SetTheme( this, new Theme( "FluentDark", App.Controls ) );

            // Control Properties
            InitializeComponent( );
            Background = _theme.BackColor;
            Foreground = _theme.ForeColor;
            BorderBrush = _theme.BackColor;
            Width = 1500;
            MinWidth = 1500;
            Height = 800;
            MinHeight = 800;
            PreviewKeyDown += OnPreviewKeyDown;
            MessageListView.PreviewMouseRightButtonUp += OnPreviewMouseRightButtonUp;
            _messageContextMenu = new ContextMenu( );
            _messageContextMenu.AddHandler( MenuItem.ClickEvent,
                new RoutedEventHandler( OnMessageMenuClick ) );

            // Control Events
            Loaded += OnLoaded;
        }

        /// <summary>
        /// Gets or sets the live chat view model.
        /// </summary>
        /// <value>
        /// The live chat view model.
        /// </value>
        public LiveChatViewModel LiveChatViewModel { get; set; }

        /// <summary>
        /// Creates the notifier.
        /// </summary>
        /// <returns>
        /// </returns>
        private Notifier CreateNotifier( )
        {
            try
            {
                var _position = new PrimaryScreenPositionProvider( Corner.BottomRight, 10, 10 );
                var _lifeTime = new TimeAndCountBasedLifetimeSupervisor( TimeSpan.FromSeconds( 5 ),
                    MaximumNotificationCount.UnlimitedNotifications( ) );

                return new Notifier( _config =>
                {
                    _config.LifetimeSupervisor = _lifeTime;
                    _config.PositionProvider = _position;
                    _config.Dispatcher = Application.Current.Dispatcher;
                } );
            }
            catch( Exception ex )
            {
                Fail( ex );
                return default( Notifier );
            }
        }

        /// <summary>
        /// Sends the notification.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        private void SendNotification( string message )
        {
            try
            {
                ThrowIf.Null( message, nameof( message ) );
                var _notification = CreateNotifier( );
                _notification.ShowInformation( message );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        // Update UI from ChatViewModel
        /// <summary>
        /// Updates the UI.
        /// </summary>
        /// <param name="updateUIEnum">
        /// The update UI enum.
        /// </param>
        private void UpdateUI( UpdateUIEnum updateUIEnum )
        {
            switch( updateUIEnum )
            {
                case UpdateUIEnum.SetFocusToChatInput:
                {
                    ChatInputTextBox.Focus( );
                    break;
                }
                case UpdateUIEnum.SetupMessageListViewScrollViewer:
                {
                    SetupMessageListViewScrollViewer( );
                    break;
                }
                case UpdateUIEnum.MessageListViewScrollToBottom:
                {
                    _messageScrollViewer?.ScrollToBottom( );
                    break;
                }
                default:
                {
                    ChatInputTextBox.Focus( );
                    break;
                }
            }
        }

        /// <summary>
        /// Setups the chat ListView scroll viewer.
        /// </summary>
        private void SetupChatListViewScrollViewer( )
        {
            // Get the ScrollViewer from the ListView.             
            _chatScrollViewer = GetScrollViewer( ChatListView );

            // Based on: https://stackoverflow.com/a/1426312	
            var _collectionChanged = ChatListView.ItemsSource as INotifyCollectionChanged;
            if( _collectionChanged != null )
            {
                _collectionChanged.CollectionChanged += ( sender, e ) =>
                {
                    _chatScrollViewer?.ScrollToBottom( );
                };
            }
        }

        // Needs to re-setup because MessageListView.ItemsSource resets with SelectedChat.MessageList
        // Note: technically there could be a leak without doing 'CollectionChanged -='
        /// <summary>
        /// Setups the message ListView scroll viewer.
        /// </summary>
        private void SetupMessageListViewScrollViewer( )
        {
            var _collectionChanged = MessageListView.ItemsSource as INotifyCollectionChanged;
            if( _collectionChanged != null )
            {
                _collectionChanged.CollectionChanged += ( sender, e ) =>
                {
                    _messageScrollViewer?.ScrollToBottom( );
                };
            }
        }

        // From: https://stackoverflow.com/a/41136328
        // This is part of implementing the "automatically scroll to the bottom" functionality.
        /// <summary>
        /// Gets the scroll viewer.
        /// </summary>
        /// <param name="element">The UIElement.</param>
        /// <returns>
        /// ScrollViewer
        /// </returns>
        private ScrollViewer GetScrollViewer( UIElement element )
        {
            ScrollViewer _viewer = null;
            if( element != null )
            {
                for( var _i = 0;
                    _i < VisualTreeHelper.GetChildrenCount( element ) && _viewer == null;
                    _i++ )
                {
                    _viewer = VisualTreeHelper.GetChild( element, _i ) is ScrollViewer
                        ? (ScrollViewer)VisualTreeHelper.GetChild( element, _i )
                        : GetScrollViewer( VisualTreeHelper.GetChild( element, _i ) as UIElement );
                }
            }

            return _viewer;
        }

        /// <summary>
        /// Shows the message context menu.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowMessageContextMenu( Message message )
        {
            _messageContextMenu.Tag = message;
            _messageContextMenu.Items.Clear( );

            // Message header
            _messageContextMenu.Items.Add( new MenuItem
            {
                Header = "Message",
                IsHitTestVisible = false,
                FontSize = 12,
                FontWeight = FontWeights.SemiBold
            } );

            _messageContextMenu.Items.Add( new Separator( ) );

            // Copy to clipboard
            _messageContextMenu.Items.Add( new MenuItem
            {
                Header = _copyMessage,
                FontSize = 12,
                Icon = new FontIcon
                {
                    Glyph = "\uE16F"
                }
            } );

            _messageContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Handles the Loaded event of the LiveChatUserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs" />
        /// instance containing the event data.</param>
        private void OnLoaded( object sender, RoutedEventArgs e )
        {
            if( !_alreadyLoaded )
            {
                _alreadyLoaded = true;
                LiveChatViewModel = (LiveChatViewModel)DataContext;
                LiveChatViewModel.UpdateUIAction = UpdateUI;
                SetupChatListViewScrollViewer( );
                _messageScrollViewer = GetScrollViewer( MessageListView );
                SetupMessageListViewScrollViewer( );
            }
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of the LiveChatUserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs" />
        /// instance containing the event data.</param>
        private void OnPreviewKeyDown( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Enter
                && Keyboard.Modifiers == ModifierKeys.Control )
            {
                // Ctrl+Enter for input of multiple lines
                var _liveChatUserControl = sender as LiveChatUserControl;
                if( _liveChatUserControl != null )
                {
                    // ChatGPT mostly answered this!
                    var _textBox = _liveChatUserControl.ChatInputTextBox;
                    e.Handled = true;
                }
            }
            else if( ( e.Key == Key.Up || e.Key == Key.Down )
                && ( e.KeyboardDevice.Modifiers & ModifierKeys.Control ) != 0 )
            {
                // Use CTRL+Up/Down to allow Up/Down alone for multiple lines in ChatInputTextBox
                var _inputTextBox = Keyboard.FocusedElement as TextBox;
                if( _inputTextBox?.Name == "ChatInputTextBox" )
                {
                    LiveChatViewModel?.PrevNextChatInput( e.Key == Key.Up );
                }
            }
        }

        /// <summary>
        /// Handles the PreviewMouseRightButtonUp event of the MessageListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs" />
        /// instance containing the event data.</param>
        private void OnPreviewMouseRightButtonUp( object sender, MouseButtonEventArgs e )
        {
            // Note: target could be System.Windows.Controls.TextBoxView (in .NET 6)
            //          but it's internal (seen in debugger) and not accessible, so use string
            var _target = e.Device.Target?.ToString( );

            // Hit test for image, text, blank space below text (Border)
            if( e.Device.Target is Grid
                || _target == "System.Windows.Controls.TextBoxView"
                || e.Device.Target is TextBlock )
            {
                var _message = ( e.Device.Target as FrameworkElement )?.DataContext as Message;
                if( _message != null )
                {
                    ShowMessageContextMenu( _message );
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Messages the menu on click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoutedEventArgs" />
        /// instance containing the event data.</param>
        private void OnMessageMenuClick( object sender, RoutedEventArgs args )
        {
            var _menuItem = args.Source as MenuItem;
            var _message = _messageContextMenu.Tag as Message;
            if( _menuItem != null
                && _message != null
                && LiveChatViewModel != null )
            {
                switch( _menuItem.Header as string )
                {
                    case LiveChatUserControl._copyMessage:
                        LiveChatViewModel.CopyMessage( _message );
                        break;
                }
            }
        }

        /// <summary>
        /// Called when [calculator menu option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnCalculatorMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _calculator = new CalculatorWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Topmost = true
                };

                _calculator.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [file menu option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnFileMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _fileBrowser = new FileBrowser
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Topmost = true
                };

                _fileBrowser.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [folder menu option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnFolderMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _fileBrowser = new FileBrowser
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Topmost = true
                };

                _fileBrowser.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [control panel option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnControlPanelOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WinMinion.LaunchControlPanel( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [task manager option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnTaskManagerOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WinMinion.LaunchTaskManager( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [close option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnCloseOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                Application.Current.Shutdown( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [chrome option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// containing the event data.</param>
        private void OnChromeOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WebMinion.RunChrome( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [edge option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private void OnEdgeOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WebMinion.RunEdge( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [firefox option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// containing the event data.</param>
        private void OnFirefoxOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WebMinion.RunFirefox( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
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