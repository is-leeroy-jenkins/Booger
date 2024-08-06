
namespace Booger
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Collections.Specialized;
    using ModernWpf.Controls;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Controls.UserControl" />
    /// <seealso cref="System.Windows.Markup.IComponentConnector" />
    public partial class LiveChatUserControl : UserControl
    {
        /// <summary>
        /// The copy message
        /// </summary>
        private const string _CopyMessage = "Copy";

        /// <summary>
        /// The already loaded
        /// </summary>
        private bool _alreadyLoaded;
        /// <summary>
        /// The chat ListView scroll viewer
        /// </summary>
        private ScrollViewer? _chatListViewScrollViewer;
        /// <summary>
        /// The message ListView scroll viewer
        /// </summary>
        private ScrollViewer? _messageListViewScrollViewer;
        /// <summary>
        /// The message context menu
        /// </summary>
        private ContextMenu _messageContextMenu;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveChatUserControl"/> class.
        /// </summary>
        public LiveChatUserControl()
        {
            InitializeComponent();

            Loaded += LiveChatUserControl_Loaded;
            PreviewKeyDown += LiveChatUserControl_PreviewKeyDown;                        
            MessageListView.PreviewMouseRightButtonUp += MessageListView_PreviewMouseRightButtonUp;
            _messageContextMenu = new ContextMenu();
            _messageContextMenu.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(MessageMenuOnClick));
        }

        /// <summary>
        /// Gets or sets the live chat view model.
        /// </summary>
        /// <value>
        /// The live chat view model.
        /// </value>
        public LiveChatViewModel? LiveChatViewModel { get; set; }

        // Update UI from ChatViewModel
        /// <summary>
        /// Updates the UI.
        /// </summary>
        /// <param name="updateUIEnum">The update UI enum.</param>
        private void UpdateUI(UpdateUIEnum updateUIEnum)
        {
            switch (updateUIEnum)
            {
                case UpdateUIEnum.SetFocusToChatInput:
                    ChatInputTextBox.Focus();
                    break;
                case UpdateUIEnum.SetupMessageListViewScrollViewer:
                    SetupMessageListViewScrollViewer();
                    break;
                case UpdateUIEnum.MessageListViewScrollToBottom:
                    _messageListViewScrollViewer?.ScrollToBottom();
                    break;
            }
        }

        /// <summary>
        /// Handles the Loaded event of the LiveChatUserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void LiveChatUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_alreadyLoaded)
            {
                _alreadyLoaded = true;

                LiveChatViewModel = (LiveChatViewModel)DataContext;
                LiveChatViewModel.UpdateUIAction = UpdateUI;
                SetupChatListViewScrollViewer();
                _messageListViewScrollViewer = GetScrollViewer(MessageListView);
                SetupMessageListViewScrollViewer();
            }
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of the LiveChatUserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void LiveChatUserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Control)
            {
                // Ctrl+Enter for input of multiple lines
                var liveChatUserControl = sender as LiveChatUserControl;
                if (liveChatUserControl != null)
                {
                    // ChatGPT mostly answered this!
                    TextBox textBox = liveChatUserControl.ChatInputTextBox;
                    int caretIndex = textBox.CaretIndex;
                    textBox.Text = textBox.Text.Insert(caretIndex, Environment.NewLine);
                    textBox.CaretIndex = caretIndex + Environment.NewLine.Length;
                    e.Handled = true;
                }
            }
            else if ((e.Key == Key.Up || e.Key == Key.Down) && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0)
            {
                // Use CTRL+Up/Down to allow Up/Down alone for multiple lines in ChatInputTextBox
                TextBox? inputTextBox = Keyboard.FocusedElement as TextBox;
                if (inputTextBox?.Name == "ChatInputTextBox")
                {
                    LiveChatViewModel?.PrevNextChatInput(isUp: e.Key == Key.Up);
                }
            }
        }

        /// <summary>
        /// Setups the chat ListView scroll viewer.
        /// </summary>
        private void SetupChatListViewScrollViewer()
        {
            // Get the ScrollViewer from the ListView. We'll need that in order to reliably
            // implement "automatically scroll to the bottom when new items are added" functionality.            
            _chatListViewScrollViewer = GetScrollViewer(ChatListView);

            // Based on: https://stackoverflow.com/a/1426312	
            INotifyCollectionChanged? notifyCollectionChanged = ChatListView.ItemsSource as INotifyCollectionChanged;
            if (notifyCollectionChanged != null)
            {
                notifyCollectionChanged.CollectionChanged += (sender, e) =>
                {
                    _chatListViewScrollViewer?.ScrollToBottom();
                };
            }
        }

        // Needs to re-setup because MessageListView.ItemsSource resets with SelectedChat.MessageList
        // Note: technically there could be a leak without doing 'CollectionChanged -='
        /// <summary>
        /// Setups the message ListView scroll viewer.
        /// </summary>
        private void SetupMessageListViewScrollViewer()
        {
            INotifyCollectionChanged? notifyCollectionChanged = MessageListView.ItemsSource as INotifyCollectionChanged;
            if (notifyCollectionChanged != null)
            {
                notifyCollectionChanged.CollectionChanged += (sender, e) =>
                {
                    _messageListViewScrollViewer?.ScrollToBottom();
                };
            }
        }

        /// <summary>
        /// Handles the PreviewMouseRightButtonUp event of the MessageListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/> instance containing the event data.</param>
        private void MessageListView_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Note: target could be System.Windows.Controls.TextBoxView (in .NET 6)
            //          but it's internal (seen in debugger) and not accessible, so use string
            string? target = e.Device.Target?.ToString();

            // Hit test for image, text, blank space below text (Border)
            if (e.Device.Target is Grid ||
                target == "System.Windows.Controls.TextBoxView" ||
                e.Device.Target is TextBlock)
            {
                Message? message = (e.Device.Target as FrameworkElement)?.DataContext as Message;
                if (message != null)
                {
                    ShowMessageContextMenu(message);
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Shows the message context menu.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowMessageContextMenu(Message message)
        {
            _messageContextMenu.Tag = message;
            _messageContextMenu.Items.Clear();

            // Message header
            _messageContextMenu.Items.Add(new MenuItem
            {
                Header = "Message",
                IsHitTestVisible = false,
                FontSize = 20,
                FontWeight = FontWeights.SemiBold
            }); ;
            _messageContextMenu.Items.Add(new Separator());
            // Copy to clipboard
            _messageContextMenu.Items.Add(new MenuItem
            {
                Header = _CopyMessage,
                FontSize = 20,
                Icon = new FontIcon { Glyph = "\uE16F" }
            });            
            _messageContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Messages the menu on click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MessageMenuOnClick(object sender, RoutedEventArgs args)
        {
            MenuItem? mi = args.Source as MenuItem;
            Message? message = _messageContextMenu.Tag as Message;
            if (mi != null && message != null && LiveChatViewModel != null)
            {
                switch (mi.Header as string)
                {
                    case _CopyMessage: LiveChatViewModel.CopyMessage(message); break;                    
                    default: break;
                }
            }
        }

        // From: https://stackoverflow.com/a/41136328
        // This is part of implementing the "automatically scroll to the bottom" functionality.
        /// <summary>
        /// Gets the scroll viewer.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private ScrollViewer? GetScrollViewer(UIElement? element)
        {
            ScrollViewer? scrollViewer = null;
            if (element != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element) && scrollViewer == null; i++)
                {
                    if (VisualTreeHelper.GetChild(element, i) is ScrollViewer)
                    {
                        scrollViewer = (ScrollViewer)(VisualTreeHelper.GetChild(element, i));
                    }
                    else
                    {
                        scrollViewer = GetScrollViewer(VisualTreeHelper.GetChild(element, i) as UIElement);
                    }
                }
            }

            return scrollViewer;
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
