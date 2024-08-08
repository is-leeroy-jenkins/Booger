// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="SyntaxHighLightingWriter.cs" company="Terry D. Eppler">
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
//   SyntaxHighLightingWriter.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Media;
    using ColorCode;
    using ColorCode.Common;
    using ColorCode.Parsing;
    using ColorCode.Styling;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ColorCode.CodeColorizerBase" />
    [ SuppressMessage( "ReSharper", "ArrangeModifiersOrder" ) ]
    public class WpfSyntaxHighLighting : CodeColorizerBase
    {
        /// <summary>
        /// The brush converter
        /// </summary>
        private readonly static BrushConverter _brushConverter = new BrushConverter( );

        /// <summary>
        /// Gets or sets the inline collection.
        /// </summary>
        /// <value>
        /// The inline collection.
        /// </value>
        private InlineCollection InlineCollection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfSyntaxHighLighting"/> class.
        /// </summary>
        /// <param name="Styles">The styles.</param>
        /// <param name="languageParser">The language parser.</param>
        public WpfSyntaxHighLighting( StyleDictionary Styles = null,
            ILanguageParser languageParser = null )
            : base( Styles, languageParser )
        {
        }

        /// <summary>
        /// Formats the text block.
        /// </summary>
        /// <param name="sourceCode">The source code.</param>
        /// <param name="language">The language.</param>
        /// <param name="textBlock">The text block.</param>
        public void FormatTextBlock( string sourceCode, ILanguage language, TextBlock textBlock )
        {
            FormatInlines( sourceCode, language, textBlock.Inlines );
        }

        /// <summary>
        /// Formats the inlines.
        /// </summary>
        /// <param name="sourceCode">The source code.</param>
        /// <param name="language">The language.</param>
        /// <param name="inlines">The inlines.</param>
        public void FormatInlines( string sourceCode, ILanguage language, InlineCollection inlines )
        {
            InlineCollection = inlines;
            if( language != null )
            {
                languageParser.Parse( sourceCode, language, Write );
            }
            else
            {
                CreateSpan( sourceCode, null );
            }
        }

        /// <summary>
        /// Writes the specified parsed source code.
        /// </summary>
        /// <param name="parsedSourceCode">The parsed source code.</param>
        /// <param name="scopes">The scopes.</param>
        protected override void Write( string parsedSourceCode, IList<Scope> scopes )
        {
            var _styleInsertions = new List<TextInsertion>( );
            foreach( var _scope in scopes )
            {
                GetStyleInsertionsForCapturedStyle( _scope, _styleInsertions );
            }

            _styleInsertions.SortStable( ( x, y ) => x.Index.CompareTo( y.Index ) );
            var _offset = 0;
            Scope _previousScope = null;
            foreach( var _styleinsertion in _styleInsertions )
            {
                var _text = parsedSourceCode.Substring( _offset, _styleinsertion.Index - _offset );
                CreateSpan( _text, _previousScope );
                if( !string.IsNullOrWhiteSpace( _styleinsertion.Text ) )
                {
                    CreateSpan( _text, _previousScope );
                }

                _offset = _styleinsertion.Index;
                _previousScope = _styleinsertion.Scope;
            }

            var _remaining = parsedSourceCode.Substring( _offset );

            // Ensures that those loose carriages don't run away!
            if( _remaining != "\r" )
            {
                CreateSpan( _remaining, null );
            }
        }

        /// <summary>
        /// Creates the span.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="scope">The scope.</param>
        private void CreateSpan( string text, Scope scope )
        {
            var _span = new Span( );
            var _run = new Run
            {
                Text = text
            };

            // Styles and writes the text to the span.
            if( scope != null )
            {
                StyleRun( _run, scope );
            }

            _span.Inlines.Add( _run );
            InlineCollection?.Add( _span );
        }

        /// <summary>
        /// Styles the run.
        /// </summary>
        /// <param name="run">The run.</param>
        /// <param name="scope">The scope.</param>
        private void StyleRun( Run run, Scope scope )
        {
            string _foreground = null;
            string _background = null;
            var _italic = false;
            var _bold = false;
            if( Styles.Contains( scope.Name ) )
            {
                var _style = Styles[ scope.Name ];
                _foreground = _style.Foreground;
                _background = _style.Background;
                _italic = _style.Italic;
                _bold = _style.Bold;
            }

            if( !string.IsNullOrWhiteSpace( _foreground ) )
            {
                try
                {
                    run.Foreground =
                        WpfSyntaxHighLighting._brushConverter.ConvertFromString( _foreground ) as
                            Brush;
                }
                catch { }
            }

            //Background isn't supported, but a workaround could be created.
            if( _italic )
            {
                run.FontStyle = FontStyles.Italic;
            }

            if( _bold )
            {
                run.FontWeight = FontWeights.Bold;
            }
        }

        /// <summary>
        /// Gets the style insertions for captured style.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="styleInsertions">The style insertions.</param>
        private void GetStyleInsertionsForCapturedStyle( Scope scope,
            ICollection<TextInsertion> styleInsertions )
        {
            styleInsertions.Add( new TextInsertion
            {
                Index = scope.Index,
                Scope = scope
            } );

            foreach( var _childScope in scope.Children )
            {
                GetStyleInsertionsForCapturedStyle( _childScope, styleInsertions );
            }

            styleInsertions.Add( new TextInsertion
            {
                Index = scope.Index + scope.Length
            } );
        }
    }
}