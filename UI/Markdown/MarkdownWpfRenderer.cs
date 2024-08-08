// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="MarkdownWpfRenderer.cs" company="Terry D. Eppler">
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
//   MarkdownWpfRenderer.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Reflection.Metadata;
    using System.Security.Policy;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using ColorCode;
    using ColorCode.Common;
    using ColorCode.Parsing;
    using ColorCode.Styling;
    using Markdig.Extensions.Tables;
    using Markdig.Extensions.TaskLists;
    using Markdig.Syntax;
    using Markdig.Syntax.Inlines;
    using WpfDocs = System.Windows.Documents;

    /// <summary>
    /// 
    /// </summary>
    public class MarkdownWpfRenderer
    {
        /// <summary>
        /// The heading1 size
        /// </summary>
        public double _heading1Size = 24;

        /// <summary>
        /// The heading2 size
        /// </summary>
        public double _heading2Size = 18;

        /// <summary>
        /// The heading3 size
        /// </summary>
        public double _heading3Size = 16;

        /// <summary>
        /// The heading4 size
        /// </summary>
        public double _heading4Size = 14;

        /// <summary>
        /// The normal size
        /// </summary>
        public double _normalSize = 12;

        /// <summary>
        /// Gets or sets the color mode.
        /// </summary>
        /// <value>
        /// The color mode.
        /// </value>
        public ColorMode ColorMode { get; set; } =
            App.GetService<ColorModeService>( ).CurrentActualMode;

        /// <summary>
        /// Occurs when [link navigate].
        /// </summary>
        public static event EventHandler<MarkdownLinkNavigateEventArgs> LinkNavigate;

        /// <summary>
        /// Renders the document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public FrameworkElement RenderDocument( MarkdownDocument document,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new FrameworkElement( );
            }

            if( document == null )
            {
                return new FrameworkElement( );
            }

            var _documentElement = new StackPanel( );
            foreach( var _renderedBlock in RenderBlocks( document, cancellationToken ) )
            {
                if( cancellationToken.IsCancellationRequested )
                {
                    break;
                }

                _documentElement.Children.Add( _renderedBlock );
            }

            return _documentElement;
        }

        /// <summary>
        /// Renders the document to.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="document">The document.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public void RenderDocumentTo( ContentControl target, MarkdownDocument document,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return;
            }

            if( document == null )
            {
                return;
            }

            var _documentElement = new StackPanel( );
            target.Content = _documentElement;
            foreach( var _renderedBlock in RenderBlocks( document, cancellationToken ) )
            {
                if( cancellationToken.IsCancellationRequested )
                {
                    break;
                }

                _documentElement.Children.Add( _renderedBlock );
            }
        }

        /// <summary>
        /// Renders the blocks.
        /// </summary>
        /// <param name="blocks">The blocks.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public List<FrameworkElement> RenderBlocks( IEnumerable<Block> blocks,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new List<FrameworkElement>( );
            }

            var _elements = new List<FrameworkElement>( );
            FrameworkElement _tailElement = null;
            foreach( var _block in blocks )
            {
                if( cancellationToken.IsCancellationRequested )
                {
                    break;
                }

                var _renderedBlock = RenderBlock( _block, cancellationToken );
                if( _renderedBlock != null )
                {
                    _elements.Add( _renderedBlock );
                    _tailElement = _renderedBlock;
                }
            }

            if( _tailElement != null )
            {
                _tailElement.Margin = _tailElement.Margin with
                {
                    Bottom = 0
                };
            }

            return _elements;
        }

        /// <summary>
        /// Renders the block.
        /// </summary>
        /// <param name="block">The block.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public FrameworkElement RenderBlock( Block block, CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new FrameworkElement( );
            }

            if( block is ParagraphBlock _paragraphBlock )
            {
                return RenderParagraphBlock( _paragraphBlock, cancellationToken );
            }
            else if( block is HeadingBlock _headingBlock )
            {
                return RenderHeadingBlock( _headingBlock, cancellationToken );
            }
            else if( block is QuoteBlock _quoteBlock )
            {
                return RenderQuoteBlock( _quoteBlock, cancellationToken );
            }
            else if( block is FencedCodeBlock _fencedCodeBlock )
            {
                return RenderFencedCodeBlock( _fencedCodeBlock, cancellationToken );
            }
            else if( block is CodeBlock _codeBlock )
            {
                return RenderCodeBlock( _codeBlock, cancellationToken );
            }
            else if( block is HtmlBlock _htmlBlock )
            {
                return RenderHtmlBlock( _htmlBlock, cancellationToken );
            }
            else if( block is ThematicBreakBlock _thematicBreakBlock )
            {
                return RenderThematicBreakBlock( _thematicBreakBlock, cancellationToken );
            }
            else if( block is ListBlock _listBlock )
            {
                return RenderListBlock( _listBlock, cancellationToken );
            }
            else if( block is Table _table )
            {
                return RenderTable( _table, cancellationToken );
            }
            else if( block is ContainerBlock _containerBlock )
            {
                return RenderContainerBlock( _containerBlock, cancellationToken );
            }
            else
            {
                return new TextBlock( );
            }
        }

        /// <summary>
        /// Renders the container block.
        /// </summary>
        /// <param name="containerBlock">The container block.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public FrameworkElement RenderContainerBlock( ContainerBlock containerBlock,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new FrameworkElement( );
            }

            var _documentElement = new StackPanel( )
            {
                Margin = new Thickness( 0, 0, 0, _normalSize )
            };

            foreach( var _renderedBlock in RenderBlocks( containerBlock, cancellationToken ) )
            {
                if( cancellationToken.IsCancellationRequested )
                {
                    return new FrameworkElement( );
                }

                _documentElement.Children.Add( _renderedBlock );
            }

            return _documentElement;
        }

        /// <summary>
        /// Renders the table.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public FrameworkElement RenderTable( Table table, CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new FrameworkElement( );
            }

            var _tableElement = new Border( )
            {
                BorderThickness = new Thickness( 0, 0, 1, 1 ),
                Margin = new Thickness( 0, 0, 0, _normalSize )
            };

            var _tableContentElement = new Grid( );
            _tableElement.Child = _tableContentElement;
            _tableElement
                .BindTableBackground( )
                .BindTableBorder( );

            foreach( var _col in table.ColumnDefinitions )
            {
                if( cancellationToken.IsCancellationRequested )
                {
                    return new FrameworkElement( );
                }

                _tableContentElement.ColumnDefinitions.Add( new ColumnDefinition( )
                {
                    Width = GridLength.Auto
                } );
            }

            var _rowIndex = 0;
            foreach( var _block in table )
            {
                if( cancellationToken.IsCancellationRequested )
                {
                    return new FrameworkElement( );
                }

                if( _block is not TableRow _row )
                {
                    continue;
                }

                _tableContentElement.RowDefinitions.Add( new RowDefinition( )
                {
                    Height = GridLength.Auto
                } );

                var _colIndex = 0;
                foreach( var _colBlock in _row )
                {
                    if( _colBlock is not TableCell _cell )
                    {
                        continue;
                    }

                    var _cellElement = new Border( )
                    {
                        BorderThickness = new Thickness( 1, 1, 0, 0 ),
                        Padding = new Thickness( _normalSize / 2, _normalSize / 4, _normalSize / 2,
                            _normalSize / 4 )
                    };

                    var _cellContentElement =
                        RenderBlock( _cell, cancellationToken );

                    _cellElement.Child = _cellContentElement;
                    _cellElement
                        .BindTableBorder( );

                    _cellContentElement.Margin = new Thickness( 0 );
                    if( _rowIndex % 2 == 1 )
                    {
                        _cellElement.BindTableStripe( );
                    }

                    Grid.SetRow( _cellElement, _rowIndex );
                    Grid.SetColumn( _cellElement, _colIndex );
                    _tableContentElement.Children.Add( _cellElement );
                    _colIndex++;
                }

                _rowIndex++;
            }

            return _tableElement;
        }

        /// <summary>
        /// Renders the list block.
        /// </summary>
        /// <param name="listBlock">The list block.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public FrameworkElement RenderListBlock( ListBlock listBlock,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new FrameworkElement( );
            }

            var _itemCount = listBlock.Count;
            Func<int, string> _markerTextGetter = listBlock.IsOrdered
                ? index => $"{index + 1}."
                : index => "-";

            var _listElement = new Border( )
            {
                Margin = new Thickness( _normalSize / 2, 0, 0, _normalSize )
            };

            var _listContentElement = new Grid( );
            _listElement.Child =
                _listContentElement;

            _listContentElement.ColumnDefinitions.Add( new ColumnDefinition( )
            {
                Width = GridLength.Auto
            } );

            _listContentElement.ColumnDefinitions.Add( new ColumnDefinition( ) );
            for( var _i = 0; _i < _itemCount; _i++ )
            {
                if( cancellationToken.IsCancellationRequested )
                {
                    return new FrameworkElement( );
                }

                _listContentElement.RowDefinitions.Add( new RowDefinition( )
                {
                    Height = GridLength.Auto
                } );
            }

            var _index = 0;
            FrameworkElement _lastRenderedItemBlock = null;
            foreach( var _itemBlock in listBlock )
            {
                if( cancellationToken.IsCancellationRequested )
                {
                    return new FrameworkElement( );
                }

                if( RenderBlock( _itemBlock, cancellationToken ) is FrameworkElement
                    _renderedItemBlock )
                {
                    _lastRenderedItemBlock = _renderedItemBlock;
                    _renderedItemBlock.Margin = _renderedItemBlock.Margin with
                    {
                        Bottom = _renderedItemBlock.Margin.Bottom / 4
                    };

                    var _marker = new TextBlock( );
                    Grid.SetRow( _marker, _index );
                    Grid.SetColumn( _marker, 0 );
                    _marker.Text = _markerTextGetter.Invoke( _index );
                    _marker.Margin = new Thickness( 0, 0, _normalSize / 2, 0 );
                    _marker.TextAlignment = TextAlignment.Right;
                    Grid.SetRow( _renderedItemBlock, _index );
                    Grid.SetColumn( _renderedItemBlock, 1 );
                    _listContentElement.Children.Add( _marker );
                    _listContentElement.Children.Add( _renderedItemBlock );
                    _index++;
                }
            }

            if( _lastRenderedItemBlock != null )
            {
                _lastRenderedItemBlock.Margin = _lastRenderedItemBlock.Margin with
                {
                    Bottom = 0
                };
            }

            return _listElement;
        }

        /// <summary>
        /// Renders the thematic break block.
        /// </summary>
        /// <param name="thematicBreakBlock">The thematic break block.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public FrameworkElement RenderThematicBreakBlock( ThematicBreakBlock thematicBreakBlock,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new FrameworkElement( );
            }

            var _thematicBreakElement = new Border( )
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 1,
                Margin = new Thickness( 0, 0, 0, _normalSize )
            };

            _thematicBreakElement
                .BindThematicBreak( );

            return _thematicBreakElement;
        }

        /// <summary>
        /// Renders the HTML block.
        /// </summary>
        /// <param name="htmlBlock">The HTML block.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public FrameworkElement RenderHtmlBlock( HtmlBlock htmlBlock,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new FrameworkElement( );
            }

            return new TextBlock( );
        }

        /// <summary>
        /// Renders the fenced code block.
        /// </summary>
        /// <param name="fencedCodeBlock">The fenced code block.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public FrameworkElement RenderFencedCodeBlock( FencedCodeBlock fencedCodeBlock,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new FrameworkElement( );
            }

            if( string.IsNullOrWhiteSpace( fencedCodeBlock.Info ) )
            {
                return RenderCodeBlock( fencedCodeBlock, cancellationToken );
            }

            var _codeElement = new Border( )
            {
                CornerRadius = new CornerRadius( 3 ),
                Margin = new Thickness( 0, 0, 0, _normalSize )
            };

            var _codeContentElement = new TextBlock( )
            {
                TextWrapping = TextWrapping.Wrap,
                Padding = new Thickness( _normalSize / 2 ),
                FontSize = _normalSize,
                FontFamily = GetCodeTextFontFamily( )
            };

            _codeElement.Child =
                _codeContentElement;

            _codeElement
                .BindCodeBlockBackground( )
                .BindCodeBlockBorder( );

            _codeContentElement
                .BindCodeBlockForeground( );

            if( fencedCodeBlock.Inline != null )
            {
                _codeContentElement.Inlines.AddRange( RenderInlines( fencedCodeBlock.Inline,
                    cancellationToken ) );
            }

            var _language = Languages.FindById( fencedCodeBlock.Info );
            var _styleDict = ColorMode switch
            {
                ColorMode.Light => StyleDictionary.DefaultLight,
                ColorMode.Dark => StyleDictionary.DefaultDark,
                _ => StyleDictionary.DefaultDark
            };

            var _writer = new WpfSyntaxHighLighting( StyleDictionary.DefaultDark );
            _writer.FormatTextBlock( fencedCodeBlock.Lines.ToString( ), _language,
                _codeContentElement );

            return _codeElement;
        }

        /// <summary>
        /// Renders the code block.
        /// </summary>
        /// <param name="codeBlock">The code block.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public FrameworkElement RenderCodeBlock( CodeBlock codeBlock,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new FrameworkElement( );
            }

            var _codeElement = new Border( )
            {
                CornerRadius = new CornerRadius( 3 ),
                Margin = new Thickness( 0, 0, 0, _normalSize )
            };

            var _codeContentElement = new TextBlock( )
            {
                TextWrapping = TextWrapping.Wrap,
                Padding = new Thickness( _normalSize / 2 ),
                FontSize = _normalSize,
                FontFamily = GetCodeTextFontFamily( )
            };

            _codeElement.Child =
                _codeContentElement;

            _codeElement
                .BindCodeBlockBackground( )
                .BindCodeBlockBorder( );

            _codeContentElement
                .BindCodeBlockForeground( );

            if( codeBlock.Inline != null )
            {
                _codeContentElement.Inlines.AddRange( RenderInlines( codeBlock.Inline,
                    cancellationToken ) );
            }

            _codeContentElement.Inlines.Add( new WpfDocs.Run( codeBlock.Lines.ToString( ) ) );
            return _codeElement;
        }

        /// <summary>
        /// Renders the quote block.
        /// </summary>
        /// <param name="quoteBlock">The quote block.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public FrameworkElement RenderQuoteBlock( QuoteBlock quoteBlock,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new FrameworkElement( );
            }

            var _quoteElement = new Border( )
            {
                BorderThickness = new Thickness( _normalSize / 3, 0, 0, 0 ),
                CornerRadius = new CornerRadius( _normalSize / 4 ),
                Padding = new Thickness( _normalSize / 2, 0, 0, 0 ),
                Margin = new Thickness( 0, 0, 0, _normalSize )
            };

            var _quoteContentPanel = new StackPanel( );
            _quoteElement.Child =
                _quoteContentPanel;

            _quoteElement
                .BindQuoteBlockBackground( )
                .BindQuoteBlockBorder( );

            foreach( var _renderedBlock in RenderBlocks( quoteBlock, cancellationToken ) )
            {
                _quoteContentPanel.Children.Add( _renderedBlock );
            }

            return _quoteElement;
        }

        /// <summary>
        /// Renders the heading block.
        /// </summary>
        /// <param name="headingBlock">The heading block.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public FrameworkElement RenderHeadingBlock( HeadingBlock headingBlock,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new FrameworkElement( );
            }

            var _fontSize = headingBlock.Level switch
            {
                1 => _heading1Size,
                2 => _heading2Size,
                3 => _heading3Size,
                4 => _heading4Size,
                _ => _normalSize
            };

            var _headingElement = new TextBlock( )
            {
                FontSize = _fontSize,
                FontWeight = FontWeights.Medium,
                Margin = new Thickness( 0, 0, 0, _normalSize )
            };

            _headingElement
                .BindMainForeground( )
                .BindMainBackground( );

            if( headingBlock.Inline != null )
            {
                _headingElement.Inlines.AddRange( RenderInlines( headingBlock.Inline,
                    cancellationToken ) );
            }

            return _headingElement;
        }

        /// <summary>
        /// Renders the paragraph block.
        /// </summary>
        /// <param name="paragraphBlock">The paragraph block.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public FrameworkElement RenderParagraphBlock( ParagraphBlock paragraphBlock,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new FrameworkElement( );
            }

            var _paragraphElement = new TextBlock( )
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness( 0, 0, 0, _normalSize ),
                FontSize = _normalSize
            };

            _paragraphElement
                .BindMainForeground( )
                .BindMainBackground( );

            if( paragraphBlock.Inline != null )
            {
                _paragraphElement.Inlines.AddRange( RenderInlines( paragraphBlock.Inline,
                    cancellationToken ) );
            }

            return _paragraphElement;
        }

        /// <summary>
        /// Renders the inlines.
        /// </summary>
        /// <param name="inlines">The inlines.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public List<WpfDocs.Inline> RenderInlines( IEnumerable<Inline> inlines,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new List<WpfDocs.Inline>( );
            }

            var _inlineElements = new List<WpfDocs.Inline>( );
            foreach( var _inline in inlines )
            {
                if( RenderInline( _inline, cancellationToken ) is WpfDocs.Inline _wpfInline )
                {
                    if( cancellationToken.IsCancellationRequested )
                    {
                        break;
                    }

                    _inlineElements.Add( _wpfInline );
                }
            }

            return _inlineElements;
        }

        /// <summary>
        /// Renders the inline.
        /// </summary>
        /// <param name="inline">The inline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public WpfDocs.Inline RenderInline( Inline inline, CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new WpfDocs.Run( );
            }

            if( inline is LiteralInline _literalInline )
            {
                return RenderLiteralInline( _literalInline, cancellationToken );
            }
            else if( inline is LinkInline _linkInline )
            {
                return RenderLinkInline( _linkInline, cancellationToken );
            }
            else if( inline is LineBreakInline _lineBreakInline )
            {
                return RenderLineBreakInline( _lineBreakInline, cancellationToken );
            }
            else if( inline is HtmlInline _htmlInline )
            {
                return RenderHtmlInline( _htmlInline, cancellationToken );
            }
            else if( inline is HtmlEntityInline _htmlEntityInline )
            {
                return RenderHtmlEntityInline( _htmlEntityInline, cancellationToken );
            }
            else if( inline is EmphasisInline _emphasisInline )
            {
                return RenderEmphasisInline( _emphasisInline, cancellationToken );
            }
            else if( inline is CodeInline _codeInline )
            {
                return RenderCodeInline( _codeInline, cancellationToken );
            }
            else if( inline is AutolinkInline _autolinkInline )
            {
                return RenderAutolinkInline( _autolinkInline, cancellationToken );
            }
            else if( inline is DelimiterInline _delimiterInline )
            {
                return RenderDelimiterInline( _delimiterInline, cancellationToken );
            }
            else if( inline is ContainerInline _containerInline )
            {
                return RenderContainerInline( _containerInline, cancellationToken );
            }
            else if( inline is TaskList _taskListInline )
            {
                return RenderTaskListInline( _taskListInline, cancellationToken );
            }
            else
            {
                return new WpfDocs.Run( );
            }
        }

        /// <summary>
        /// Renders the task list inline.
        /// </summary>
        /// <param name="taskListInline">The task list inline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public WpfDocs.Inline RenderTaskListInline( TaskList taskListInline,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new WpfDocs.Run( );
            }

            return new CheckBox( )
            {
                IsChecked = taskListInline.Checked,
                IsEnabled = false
            }.WrapWithContainer( );
        }

        /// <summary>
        /// Renders the autolink inline.
        /// </summary>
        /// <param name="autolinkInline">The autolink inline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public WpfDocs.Inline RenderAutolinkInline( AutolinkInline autolinkInline,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new WpfDocs.Run( );
            }

            return new WpfDocs.Run( autolinkInline.Url );
        }

        /// <summary>
        /// Renders the code inline.
        /// </summary>
        /// <param name="codeInline">The code inline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public WpfDocs.Inline RenderCodeInline( CodeInline codeInline,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new WpfDocs.Run( );
            }

            var _border = new Border( )
            {
                CornerRadius = new CornerRadius( 3 ),
                Padding = new Thickness( _normalSize / 6, 0, _normalSize / 6, 0 ),
                Margin = new Thickness( _normalSize / 6, 0, _normalSize / 6, 0 )
            };

            var _textBlock = new TextBlock( );
            _border.Child = _textBlock;
            _border
                .BindCodeInlineBackground( )
                .BindCodeInlineBorder( );

            _textBlock.Text = codeInline.Content;
            return _border.WrapWithContainer( );
        }

        /// <summary>
        /// Renders the container inline.
        /// </summary>
        /// <param name="containerInline">The container inline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public WpfDocs.Inline RenderContainerInline( ContainerInline containerInline,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new WpfDocs.Run( );
            }

            var _span = new WpfDocs.Span( );
            _span.Inlines.AddRange( RenderInlines( containerInline, cancellationToken ) );
            return _span;
        }

        /// <summary>
        /// Renders the emphasis inline.
        /// </summary>
        /// <param name="emphasisInline">The emphasis inline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public WpfDocs.Inline RenderEmphasisInline( EmphasisInline emphasisInline,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new WpfDocs.Run( );
            }

            var _span = new WpfDocs.Span( );
            switch( emphasisInline.DelimiterChar )
            {
                case '*' when emphasisInline.DelimiterCount == 2:// bold
                case '_' when emphasisInline.DelimiterCount == 2:// bold
                    _span.FontWeight = FontWeights.Bold;
                    break;
                case '*':// italic
                case '_':// italic
                    _span.FontStyle = FontStyles.Italic;
                    break;
                case '~':// 2x strike through, 1x subscript
                    if( emphasisInline.DelimiterCount == 2 )
                    {
                        _span.TextDecorations.Add( TextDecorations.Strikethrough );
                    }
                    else
                    {
                        WpfDocs.Typography.SetVariants( _span, FontVariants.Subscript );
                    }

                    break;
                case '^':// 1x superscript
                    WpfDocs.Typography.SetVariants( _span, FontVariants.Subscript );
                    break;
                case '+':// 2x underline
                    _span.TextDecorations.Add( TextDecorations.Underline );
                    break;
                case '=':// 2x Marked
                    _span.SetResourceReference( WpfDocs.TextElement.BackgroundProperty,
                        MarkdownResKey.Mark );

                    break;
            }

            _span.Inlines.AddRange( RenderInlines( emphasisInline, cancellationToken ) );
            return _span;
        }

        /// <summary>
        /// Renders the HTML entity inline.
        /// </summary>
        /// <param name="htmlEntityInline">The HTML entity inline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public WpfDocs.Inline RenderHtmlEntityInline( HtmlEntityInline htmlEntityInline,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new WpfDocs.Run( );
            }

            return new WpfDocs.Run( htmlEntityInline.Transcoded.ToString( ) );
        }

        /// <summary>
        /// Renders the HTML inline.
        /// </summary>
        /// <param name="htmlInline">The HTML inline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public WpfDocs.Inline RenderHtmlInline( HtmlInline htmlInline,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new WpfDocs.Run( );
            }

            return new WpfDocs.Run( );
        }

        /// <summary>
        /// Renders the line break inline.
        /// </summary>
        /// <param name="lineBreakInline">The line break inline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public WpfDocs.Inline RenderLineBreakInline( LineBreakInline lineBreakInline,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new WpfDocs.Run( );
            }

            return new WpfDocs.Run( "\n" );
        }

        /// <summary>
        /// Renders the delimiter inline.
        /// </summary>
        /// <param name="delimiterInline">The delimiter inline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public WpfDocs.Inline RenderDelimiterInline( DelimiterInline delimiterInline,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new WpfDocs.Run( );
            }

            return new WpfDocs.Run( delimiterInline.ToLiteral( ) );
        }

        /// <summary>
        /// Renders the link inline.
        /// </summary>
        /// <param name="linkInline">The link inline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public WpfDocs.Inline RenderLinkInline( LinkInline linkInline,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new WpfDocs.Run( );
            }

            Uri _uri = null;
            if( linkInline.Url != null
                && Uri.TryCreate( linkInline.Url, UriKind.RelativeOrAbsolute, out var uri ) )
            {
                _uri = uri;
            }

            if( linkInline.IsImage )
            {
                var _img = new Image( );
                _img.MaxWidth = 200;
                if( _uri != null )
                {
                    _img.Source = new BitmapImage( _uri );
                }

                return _img.WrapWithContainer( );
            }
            else
            {
                var _link = new WpfDocs.Hyperlink( );
                WpfDocs.Inline _linkContent = null;
                if( linkInline.Label != null )
                {
                    _linkContent = new WpfDocs.Run( linkInline.Label );
                }

                if( _linkContent != null )
                {
                    _link.Inlines.Add( _linkContent );
                }

                if( RenderContainerInline( linkInline, cancellationToken ) is WpfDocs.Inline
                    _extraInline )
                {
                    _link.Inlines.Add( _extraInline );
                }

                _link.Click += ( s, e ) =>
                {
                    MarkdownWpfRenderer.LinkNavigate?.Invoke( linkInline,
                        new MarkdownLinkNavigateEventArgs( linkInline.Url ) );
                };

                return _link;
            }
        }

        /// <summary>
        /// Renders the literal inline.
        /// </summary>
        /// <param name="literalInline">The literal inline.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public WpfDocs.Inline RenderLiteralInline( LiteralInline literalInline,
            CancellationToken cancellationToken )
        {
            if( cancellationToken.IsCancellationRequested )
            {
                return new WpfDocs.Run( );
            }

            return new WpfDocs.Run( literalInline.Content.ToString( ) );
        }

        /// <summary>
        /// Gets the normal text font family.
        /// </summary>
        /// <returns></returns>
        public FontFamily GetNormalTextFontFamily( )
        {
            return new FontFamily(
                "-apple-system,BlinkMacSystemFont,Segoe UI Adjusted,Segoe UI,Liberation Sans,sans-serif" );
        }

        /// <summary>
        /// Gets the code text font family.
        /// </summary>
        /// <returns></returns>
        public FontFamily GetCodeTextFontFamily( )
        {
            return new FontFamily(
                "ui-monospace,Cascadia Code,Segoe UI Mono,Liberation Mono,Menlo,Monaco,Consolas,monospace" );
        }
    }
}