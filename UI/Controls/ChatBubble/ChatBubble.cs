// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="ChatBubble.cs" company="Terry D. Eppler">
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
//   ChatBubble.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:OpenGptChat.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:OpenGptChat.Controls;assembly=OpenGptChat.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ChatBubble/>
    ///
    /// </summary>
    public class ChatBubble : Control
    {
        static ChatBubble( )
        {
            ChatBubble.DefaultStyleKeyProperty.OverrideMetadata( typeof( ChatBubble ),
                new FrameworkPropertyMetadata( typeof( ChatBubble ) ) );
        }

        public string _username
        {
            get { return (string)GetValue( ChatBubble._usernameProperty ); }
            set { SetValue( ChatBubble._usernameProperty, value ); }
        }

        public string _content
        {
            get { return (string)GetValue( ChatBubble._contentProperty ); }
            set { SetValue( ChatBubble._contentProperty, value ); }
        }

        public Brush _headerForeground
        {
            get { return (Brush)GetValue( ChatBubble._headerForegroundProperty ); }
            set { SetValue( ChatBubble._headerForegroundProperty, value ); }
        }

        public Brush _headerBackground
        {
            get { return (Brush)GetValue( ChatBubble._headerBackgroundProperty ); }
            set { SetValue( ChatBubble._headerBackgroundProperty, value ); }
        }

        public Brush _contentForeground
        {
            get { return (Brush)GetValue( ChatBubble._contentForegroundProperty ); }
            set { SetValue( ChatBubble._contentForegroundProperty, value ); }
        }

        public Brush _contentBackground
        {
            get { return (Brush)GetValue( ChatBubble._contentBackgroundProperty ); }
            set { SetValue( ChatBubble._contentBackgroundProperty, value ); }
        }

        public CornerRadius _cornerRadius
        {
            get { return (CornerRadius)GetValue( ChatBubble._cornerRadiusProperty ); }
            set { SetValue( ChatBubble._cornerRadiusProperty, value ); }
        }

        public bool _isReadonly
        {
            get { return (bool)GetValue( ChatBubble._isReadonlyProperty ); }
            set { SetValue( ChatBubble._isReadonlyProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for Username.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _usernameProperty =
            DependencyProperty.Register( nameof( ChatBubble._username ), typeof( string ),
                typeof( ChatBubble ), new PropertyMetadata( string.Empty ) );

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _contentProperty =
            DependencyProperty.Register( nameof( ChatBubble._content ), typeof( string ),
                typeof( ChatBubble ), new PropertyMetadata( string.Empty ) );

        // Using a DependencyProperty as the backing store for HeaderForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _headerForegroundProperty =
            DependencyProperty.Register( nameof( ChatBubble._headerForeground ), typeof( Brush ),
                typeof( ChatBubble ), new PropertyMetadata( Brushes.Gray ) );

        // Using a DependencyProperty as the backing store for HeaderBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _headerBackgroundProperty =
            DependencyProperty.Register( nameof( ChatBubble._headerBackground ), typeof( Brush ),
                typeof( ChatBubble ), new PropertyMetadata( Brushes.Transparent ) );

        // Using a DependencyProperty as the backing store for ContentForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _contentForegroundProperty =
            DependencyProperty.Register( nameof( ChatBubble._contentForeground ), typeof( Brush ),
                typeof( ChatBubble ), new PropertyMetadata( Brushes.Black ) );

        // Using a DependencyProperty as the backing store for ContentBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _contentBackgroundProperty =
            DependencyProperty.Register( nameof( ChatBubble._contentBackground ), typeof( Brush ),
                typeof( ChatBubble ), new PropertyMetadata( Brushes.White ) );

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _cornerRadiusProperty =
            DependencyProperty.Register( nameof( ChatBubble._cornerRadius ), typeof( CornerRadius ),
                typeof( ChatBubble ), new PropertyMetadata( new CornerRadius( 0 ) ) );

        // Using a DependencyProperty as the backing store for IsReadonly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty _isReadonlyProperty =
            DependencyProperty.Register( nameof( ChatBubble._isReadonly ), typeof( bool ),
                typeof( ChatBubble ), new PropertyMetadata( true ) );
    }
}