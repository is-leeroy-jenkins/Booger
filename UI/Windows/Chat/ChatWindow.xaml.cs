// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-01-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-01-2024
// ******************************************************************************************
// <copyright file="ChatWindow.xaml.cs" company="Terry D. Eppler">
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
//   ChatWindow.xaml.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System.Diagnostics.CodeAnalysis;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Collections.Specialized;
    using System.Threading;
    using ModernWpf.Controls;
    using ToastNotifications;
    using ToastNotifications.Lifetime;
    using ToastNotifications.Messages;
    using ToastNotifications.Position;

    /// <inheritdoc />
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "SuggestBaseTypeForParameter" ) ]
    [ SuppressMessage( "ReSharper", "UsePatternMatching" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "ConvertIfStatementToSwitchStatement" ) ]
    public partial class ChatWindow : Window
    {
        /// <summary>
        /// The new chat
        /// </summary>
        private const string NEW_CHAT = "New";

        /// <summary>
        /// The copy chat prompt
        /// </summary>
        private const string COPY_CHAT_PROMPT = "Copy";

        /// <summary>
        /// The delete chat
        /// </summary>
        private const string DELETE_CHAT = "Delete";

        /// <summary>
        /// The copy message
        /// </summary>
        private const string COPY_MESSAGE = "Copy";

        /// <summary>
        /// The delete message
        /// </summary>
        private const string DELETE_MESSAGE = "Delete";

        /// <summary>
        /// The chat view model
        /// </summary>
        private readonly ChatViewModel _viewModel;

        /// <summary>
        /// The chat ListView scroll viewer
        /// </summary>
        private ScrollViewer _chatViewer;

        /// <summary>
        /// The chat context menu
        /// </summary>
        private readonly ContextMenu _chatMenu;

        /// <summary>
        /// The message ListView scroll viewer
        /// </summary>
        private ScrollViewer _messageViewer;

        /// <summary>
        /// The message context menu
        /// </summary>
        private readonly ContextMenu _contextMenu;

        /// <summary>
        /// The update status
        /// </summary>
        private protected Action _statusUpdate;

        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <summary>
        /// The time
        /// </summary>
        private protected int _time;

        /// <summary>
        /// The timer
        /// </summary>
        private protected Timer _timer;

        /// <summary>
        /// The timer
        /// </summary>
        private protected TimerCallback _timerCallback;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:CSharpWpfChatGPT.MainWindow" /> class.
        /// </summary>
        /// <param name="viewModel">
        /// The chat view model.
        /// </param>
        public ChatWindow( ChatViewModel viewModel )
        {
            InitializeComponent( );

            // Window Properties
            ResizeMode = _theme.SizeMode;
            FontFamily = _theme.FontFamily;
            FontSize = _theme.FontSize;
            Padding = _theme.Padding;
            BorderThickness = _theme.BorderThickness;
            WindowStartupLocation = _theme.StartLocation;
            Background = _theme.BackColor;
            Foreground = _theme.ForeColor;
            BorderBrush = _theme.BorderColor;
            DataContext = _viewModel = viewModel;
            _viewModel.UpdateUIAction = UpdateUserInterface;
            Loaded += OnMainWindowLoaded;
            PreviewKeyDown += OnPreviewKeyDown;
            ChatListView.PreviewMouseRightButtonUp += OnChatListMouseRightButtonUp;
            _chatMenu = new ContextMenu( );
            _chatMenu.AddHandler( MenuItem.ClickEvent, new RoutedEventHandler( OnChatMenuClick ) );
            MessageListView.PreviewMouseRightButtonUp += OnMessageListMouseRightButtonUp;
            _contextMenu = new ContextMenu( );
            _contextMenu.AddHandler( MenuItem.ClickEvent,
                new RoutedEventHandler( OnMessageMenuClick ) );
        }

        /// <summary>
        /// Update UI from ChatViewModel
        /// </summary>
        /// <param name="updateUIEnum">The update UI enum.</param>
        private void UpdateUserInterface( UpdateUIEnum updateUIEnum )
        {
            switch( updateUIEnum )
            {
                case UpdateUIEnum.SetFocusToChatInput:
                    ChatInputTextBox.Focus( );

                    break;
                case UpdateUIEnum.SetupMessageListViewScrollViewer:
                    SetupMessageListViewScrollViewer( );

                    break;
                case UpdateUIEnum.MessageListViewScrollToBottom:
                    _messageViewer?.ScrollToBottom( );

                    break;
            }
        }

        /// <summary>
        /// Setups the chat ListView scroll viewer.
        /// </summary>
        private void SetupChatListViewScrollViewer( )
        {
            _chatViewer = GetScrollViewer( ChatListView );
            var _notifyCollectionChanged = ChatListView.ItemsSource as INotifyCollectionChanged;
            if( _notifyCollectionChanged != null )
            {
                _notifyCollectionChanged.CollectionChanged += ( sender, e ) =>
                {
                    _chatViewer?.ScrollToBottom( );
                };
            }
        }

        /// <summary>
        /// Setups the message ListView scroll viewer.
        /// </summary>
        private void SetupMessageListViewScrollViewer( )
        {
            var _notifyCollectionChanged = MessageListView.ItemsSource as INotifyCollectionChanged;
            if( _notifyCollectionChanged != null )
            {
                _notifyCollectionChanged.CollectionChanged += ( sender, e ) =>
                {
                    _messageViewer?.ScrollToBottom( );
                };
            }
        }

        /// <summary>
        /// Shows the chat context menu.
        /// </summary>
        /// <param name="chat">
        /// The chat.
        /// </param>
        public void ShowChatContextMenu( Chat chat )
        {
            _chatMenu.Tag = chat;
            _chatMenu.Items.Clear( );
            _chatMenu.Items.Add( new MenuItem
            {
                Header = "Chat",
                IsHitTestVisible = false,
                FontSize = 20,
                FontWeight = FontWeights.SemiBold
            } );

            _chatMenu.Items.Add( new Separator( ) );
            _chatMenu.Items.Add( new MenuItem
            {
                Header = ChatWindow.NEW_CHAT,
                FontSize = 20,
                Icon = new FontIcon
                {
                    Glyph = "\uE8E5"
                }
            } );

            _chatMenu.Items.Add( new Separator( ) );
            _chatMenu.Items.Add( new MenuItem
            {
                Header = ChatWindow.COPY_CHAT_PROMPT,
                FontSize = 20,
                Icon = new FontIcon
                {
                    Glyph = "\uE16F"
                }
            } );

            _chatMenu.Items.Add( new Separator( ) );
            _chatMenu.Items.Add( new MenuItem
            {
                Header = ChatWindow.DELETE_CHAT,
                FontSize = 20,
                Icon = new FontIcon
                {
                    Glyph = "\uE107"
                }
            } );

            _chatMenu.IsOpen = true;
        }

        /// <summary>
        /// Shows the message context menu.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void ShowMessageContextMenu( Message message )
        {
            _contextMenu.Tag = message;
            _contextMenu.Items.Clear( );
            _contextMenu.Items.Add( new MenuItem
            {
                Header = "Message",
                IsHitTestVisible = false,
                FontSize = 20,
                FontWeight = FontWeights.SemiBold
            } );

            _contextMenu.Items.Add( new Separator( ) );
            _contextMenu.Items.Add( new MenuItem
            {
                Header = ChatWindow.COPY_MESSAGE,
                FontSize = 20,
                Icon = new FontIcon
                {
                    Glyph = "\uE16F"
                }
            } );

            _contextMenu.Items.Add( new Separator( ) );
            _contextMenu.Items.Add( new MenuItem
            {
                Header = ChatWindow.DELETE_MESSAGE,
                FontSize = 20,
                Icon = new FontIcon
                {
                    Glyph = "\uE107"
                }
            } );

            _contextMenu.IsOpen = true;
        }

        /// <summary>
        /// Gets the scroll viewer.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <returns>
        /// ScrollViewer
        /// </returns>
        private protected ScrollViewer GetScrollViewer( UIElement element )
        {
            ScrollViewer _viewer = null;
            if( element != null )
            {
                try
                {
                    for( var _i = 0;
                        _i < VisualTreeHelper.GetChildrenCount( element ) && _viewer == null; _i++ )
                    {
                        if( VisualTreeHelper.GetChild( element, _i ) is ScrollViewer )
                        {
                            _viewer = (ScrollViewer)VisualTreeHelper.GetChild( element, _i );
                        }
                        else
                        {
                            _viewer = 
                                GetScrollViewer( VisualTreeHelper.GetChild( element, _i ) as UIElement );
                        }
                    }
                }
                catch( Exception e )
                {
                    Fail( e );

                    return default( ScrollViewer );
                }
            }

            return _viewer;
        }


        /// <summary>
        /// Creates the notifier.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Handles the PreviewMouseRightButtonUp event of the ChatListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/>
        /// instance containing the event data.</param>
        private void OnChatListMouseRightButtonUp( object sender, MouseButtonEventArgs e )
        {
            if( e.Device.Target is Grid
                || e.Device.Target is TextBox
                || e.Device.Target is TextBlock )
            {
                var _chat = ( e.Device.Target as FrameworkElement )?.DataContext as Chat;
                if( _chat != null )
                {
                    try
                    {
                        ShowChatContextMenu( _chat );
                        e.Handled = true;
                    }
                    catch( Exception ex )
                    {
                        Fail( ex );
                    }
                }
            }
        }

        /// <summary>
        /// Handles the PreviewMouseRightButtonUp event of the MessageListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/>
        /// instance containing the event data.</param>
        private void OnMessageListMouseRightButtonUp( object sender, MouseButtonEventArgs e )
        {
            var _target = e.Device.Target?.ToString( );
            if( e.Device.Target is Grid
                || _target == "System.Windows.Controls.TextBoxView"
                || e.Device.Target is TextBlock )
            {
                var _message = ( e.Device.Target as FrameworkElement )?.DataContext as Message;
                if( _message != null )
                {
                    try
                    {
                        ShowMessageContextMenu( _message );
                        e.Handled = true;
                    }
                    catch( Exception ex )
                    {
                        Fail( ex );
                    }
                }
            }
        }

        /// <summary>
        /// Chats the menu on click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnChatMenuClick( object sender, RoutedEventArgs args )
        {
            var _mi = args.Source as MenuItem;
            var _chat = _chatMenu.Tag as Chat;
            if( _mi != null
                && _chat != null )
            {
                try
                {
                    switch( _mi.Header as string )
                    {
                        case ChatWindow.NEW_CHAT:
                            _viewModel.NewChat( );

                            break;
                        case ChatWindow.COPY_CHAT_PROMPT:
                            _viewModel.CopyChatPrompt( _chat );

                            break;
                        case ChatWindow.DELETE_CHAT:
                            _viewModel.DeleteChat( _chat );

                            break;
                    }
                }
                catch( Exception e )
                {
                    Fail( e );
                }
            }
        }

        /// <summary>
        /// Messages the menu on click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnMessageMenuClick( object sender, RoutedEventArgs args )
        {
            var _mi = args.Source as MenuItem;
            var _message = _contextMenu.Tag as Message;
            if( _mi != null
                && _message != null )
            {
                try
                {
                    switch( _mi.Header as string )
                    {
                        case ChatWindow.COPY_MESSAGE:
                            _viewModel.CopyMessage( _message );

                            break;
                        case ChatWindow.DELETE_MESSAGE:
                            _viewModel.DeleteMessage( _message );

                            break;
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
            }
        }

        /// <summary>
        /// Handles the Loaded event of the MainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnMainWindowLoaded( object sender, RoutedEventArgs e )
        {
            try
            {
                SetupChatListViewScrollViewer( );
                _messageViewer = GetScrollViewer( MessageListView );
                SetupMessageListViewScrollViewer( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of the MainWindow control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/>
        /// instance containing the event data.</param>
        private void OnPreviewKeyDown( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Enter
                && Keyboard.Modifiers == ModifierKeys.Control )
            {
                try
                {
                    var _mainWindow = sender as ChatWindow;
                    if( _mainWindow != null )
                    {
                        var _textBox = _mainWindow.ChatInputTextBox;
                        var _caretIndex = _textBox.CaretIndex;
                        _textBox.Text = _textBox.Text.Insert( _caretIndex, Environment.NewLine );
                        _textBox.CaretIndex = _caretIndex + Environment.NewLine.Length;
                        e.Handled = true;
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
                }
            }
            else if( ( e.Key == Key.Up || e.Key == Key.Down )
                && ( e.KeyboardDevice.Modifiers & ModifierKeys.Control ) != 0 )
            {
                try
                {
                    var _inputTextBox = Keyboard.FocusedElement as TextBox;
                    if( _inputTextBox?.Name == "ChatInputTextBox" )
                    {
                        _viewModel.PrevNextChatInput( e.Key == Key.Up );
                    }
                }
                catch( Exception ex )
                {
                    Fail( ex );
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
                    Owner = this,
                    Topmost = true
                };

                _calculator.ShowDialog( );
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
                    Owner = this,
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
                    Owner = this,
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