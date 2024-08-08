// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="MarkdownViewer.cs" company="Terry D. Eppler">
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
//   MarkdownViewer.cs
// </summary>
// ******************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Markdig;
using Markdig.Syntax;
using Booger;

namespace Booger
{
    using Booger;
    using static MarkdownViewer;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Controls.Control" />
    public class MarkdownViewer : Control
    {
        /// <summary>
        /// Initializes the <see cref="MarkdownViewer"/> class.
        /// </summary>
        static MarkdownViewer( )
        {
            DefaultStyleKeyProperty.OverrideMetadata( typeof( MarkdownViewer ),
                new FrameworkPropertyMetadata( typeof( MarkdownViewer ) ) );
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string _content
        {
            get { return (string)GetValue( _contentProperty ); }
            set { SetValue( _contentProperty, value ); }
        }

        /// <summary>
        /// Gets the content of the rendered.
        /// </summary>
        /// <value>
        /// The content of the rendered.
        /// </value>
        public FrameworkElement _renderedContent
        {
            get { return (FrameworkElement)GetValue( _renderedContentProperty ); }
            private set { SetValue( _renderedContentProperty, value ); }
        }

        /// <summary>
        /// The content property
        /// </summary>
        public static readonly DependencyProperty _contentProperty =
            DependencyProperty.Register( nameof( _content ), typeof( string ),
                typeof( MarkdownViewer ),
                new PropertyMetadata( string.Empty, MarkdownViewer.ContentChangedCallback ) );

        /// <summary>
        /// The rendered content property
        /// </summary>
        public static readonly DependencyProperty _renderedContentProperty =
            DependencyProperty.Register( nameof( _renderedContent ),
                typeof( FrameworkElement ), typeof( MarkdownViewer ),
                new PropertyMetadata( null ) );

        /// <summary>
        /// Renders the process asynchronous.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task RenderProcessAsync( CancellationToken cancellationToken )
        {
            try
            {
                if( cancellationToken.IsCancellationRequested )
                {
                    return;
                }

                var content = _content;
                var _doc =
                    await Task.Run( ( ) =>
                    {
                        var _doc = Markdown.Parse( _content,
                            new MarkdownPipelineBuilder( )
                                .UseEmphasisExtras( )
                                .UseGridTables( )
                                .UsePipeTables( )
                                .UseTaskLists( )
                                .UseAutoLinks( )
                                .Build( ) );

                        return _doc;
                    } );

                var _renderer =
                    App.GetService<MarkdownWpfRenderer>( );

                var _contentControl =
                    new ContentControl( );

                _renderedContent = _contentControl;
                _renderer.RenderDocumentTo( _contentControl, _doc, cancellationToken );
            }
            catch { }
        }

        /// <summary>
        /// The render process task
        /// </summary>
        private Task _renderProcessTask;

        /// <summary>
        /// The render process cancellation
        /// </summary>
        private CancellationTokenSource _renderProcessCancellation;

        /// <summary>
        /// Contents the changed callback.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void ContentChangedCallback( DependencyObject d,
            DependencyPropertyChangedEventArgs e )
        {
            if( d is not MarkdownViewer _markdownViewer )
            {
                return;
            }

            if( _markdownViewer
                ._renderProcessCancellation is CancellationTokenSource _cancellation )
            {
                _cancellation.Cancel( );
            }

            _cancellation =
                _markdownViewer._renderProcessCancellation =
                    new CancellationTokenSource( );

            _markdownViewer._renderProcessTask =
                _markdownViewer.RenderProcessAsync( _cancellation.Token );
        }
    }
}