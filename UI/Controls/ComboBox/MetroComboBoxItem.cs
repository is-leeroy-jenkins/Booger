﻿// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-04-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-04-2024
// ******************************************************************************************
// <copyright file="MetroComboBoxItem.cs" company="Terry D. Eppler">
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
//   MetroComboBoxItem.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using Syncfusion.Windows.Tools.Controls;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Syncfusion.Windows.Tools.Controls.ComboBoxItemAdv" />
    [ SuppressMessage( "ReSharper", "UnusedType.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "InconsistentNaming" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    public class MetroComboBoxItem : ComboBoxItemAdv
    {
        /// <summary>
        /// The theme
        /// </summary>
        private protected readonly DarkMode _theme = new DarkMode( );

        /// <summary>
        /// Gets or sets an arbitrary object value that can be used
        /// to store custom information about this element.
        /// </summary>
        public new object Tag { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Booger.MetroComboBoxItem" /> class.
        /// </summary>
        public MetroComboBoxItem( )
            : base( )
        {
            // Control Properties
            SetResourceReference( MetroComboBoxItem.StyleProperty, typeof( ComboBoxItemAdv ) );
            Height = 22;
            Padding = new Thickness( 10, 1, 1, 1 );
            Background = _theme.ControlColor;
            BorderBrush = _theme.ControlColor;
            Foreground = _theme.ForeColor;

            // Event Wiring
            MouseEnter += OnItemMouseEnter;
            MouseLeave += OnItemMouseLeave;
        }

        /// <summary>
        /// Called when [item mouse enter].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnItemMouseEnter( object sender, EventArgs e )
        {
            try
            {
                if( sender is MetroComboBoxItem _item )
                {
                    _item.Foreground = _theme.WhiteColor;
                    _item.Background = _theme.SteelBlueColor;
                    _item.BorderBrush = _theme.SteelBlueColor;
                }
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [item mouse leave].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/>
        /// instance containing the event data.</param>
        private protected void OnItemMouseLeave( object sender, EventArgs e )
        {
            try
            {
                if( sender is MetroComboBoxItem _item )
                {
                    _item.Foreground = _theme.ForeColor;
                    _item.Background = _theme.ControlColor;
                    _item.BorderBrush = _theme.ControlColor;
                }
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