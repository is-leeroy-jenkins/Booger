namespace Booger
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.Window" />
    /// <seealso cref="T:System.Windows.Markup.IComponentConnector" />
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public partial class ChatWindow : Window
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Booger.MainWindow" /> class.
        /// </summary>
        /// <param name="mainViewModel">The main view model.</param>
        public ChatWindow( ChatViewModel mainViewModel )
        {
            InitializeComponent( );
            DataContext = mainViewModel;            
        }
    }
}
