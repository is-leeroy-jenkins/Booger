// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="MarkdownWpfRendererExtensions.cs" company="Terry D. Eppler">
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
//   MarkdownWpfRendererExtensions.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System.Windows;
    using System.Windows.Controls;
    using WpfDocs = System.Windows.Documents;

    public static class MarkdownWpfRendererExtensions
    {
        public static TextBlock BindMainForeground( this TextBlock element )
        {
            element.SetResourceReference( TextBlock.ForegroundProperty,
                MarkdownResKey.MainForeground );

            return element;
        }

        public static TextBlock BindMainBackground( this TextBlock element )
        {
            element.SetResourceReference( TextBlock.BackgroundProperty,
                MarkdownResKey.MainBackground );

            return element;
        }

        public static TextBlock BindCodeBlockForeground( this TextBlock element )
        {
            element.SetResourceReference( TextBlock.ForegroundProperty,
                MarkdownResKey.CodeBlockForeground );

            return element;
        }

        public static Border BindMainBackground( this Border element )
        {
            element.SetResourceReference( Border.BackgroundProperty,
                MarkdownResKey.MainBackground );

            return element;
        }

        public static Border BindQuoteBlockBackground( this Border element )
        {
            element.SetResourceReference( Border.BackgroundProperty,
                MarkdownResKey.QuoteBlockBackground );

            return element;
        }

        public static Border BindCodeBlockBackground( this Border element )
        {
            element.SetResourceReference( Border.BackgroundProperty,
                MarkdownResKey.CodeBlockBackground );

            return element;
        }

        public static Border BindCodeBlockBorder( this Border element )
        {
            element.SetResourceReference( Border.BorderBrushProperty,
                MarkdownResKey.CodeBlockBorder );

            return element;
        }

        public static Border BindCodeInlineBackground( this Border element )
        {
            element.SetResourceReference( Border.BackgroundProperty,
                MarkdownResKey.CodeInlineBackground );

            return element;
        }

        public static Border BindCodeInlineBorder( this Border element )
        {
            element.SetResourceReference( Border.BorderBrushProperty,
                MarkdownResKey.CodeInlineBorder );

            return element;
        }

        public static Border BindMainBorder( this Border element )
        {
            element.SetResourceReference( Border.BorderBrushProperty, MarkdownResKey.MainBorder );
            return element;
        }

        public static Border BindQuoteBlockBorder( this Border element )
        {
            element.SetResourceReference( Border.BorderBrushProperty,
                MarkdownResKey.QuoteBlockBorder );

            return element;
        }

        public static TextBlock BindTableForeground( this TextBlock element )
        {
            element.SetResourceReference( TextBlock.ForegroundProperty,
                MarkdownResKey.TableForeground );

            return element;
        }

        public static TextBlock BindTableBackground( this TextBlock element )
        {
            element.SetResourceReference( TextBlock.BackgroundProperty,
                MarkdownResKey.TableBackground );

            return element;
        }

        public static Border BindTableBackground( this Border element )
        {
            element.SetResourceReference( Border.BackgroundProperty,
                MarkdownResKey.TableBackground );

            return element;
        }

        public static Border BindTableStripe( this Border element )
        {
            element.SetResourceReference( Border.BackgroundProperty, MarkdownResKey.TableStripe );
            return element;
        }

        public static Border BindTableBorder( this Border element )
        {
            element.SetResourceReference( Border.BorderBrushProperty, MarkdownResKey.TableBorder );
            return element;
        }

        public static Border BindThematicBreak( this Border element )
        {
            element.SetResourceReference( Border.BackgroundProperty, MarkdownResKey.ThematicBreak );
            return element;
        }

        public static WpfDocs.Inline WrapWithContainer( this UIElement element )
        {
            return new WpfDocs.Span( )
            {
                BaselineAlignment = BaselineAlignment.Center,
                Inlines =
                {
                    new WpfDocs.InlineUIContainer( )
                    {
                        Child = element
                    }
                }
            };
        }
    }
}