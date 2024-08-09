// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="FocusUtils.cs" company="Terry D. Eppler">
//     Booger is a quick & dirty WPF application that interacts with OpenAI GPT-3.5 Turbo API
//     based on NET6 and written in C-Sharp.
// 
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
//   FocusUtils.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Input;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.FrameworkElement" />
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "ArrangeStaticMemberQualifier" ) ]
    public class FocusUtils : FrameworkElement
    {
        /// <summary>
        /// Gets the is automatic logic focus.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static bool Get_isAutoLogicFocus( DependencyObject obj )
        {
            return (bool)obj.GetValue( _isAutoLogicFocusProperty );
        }

        /// <summary>
        /// Sets the is automatic logic focus.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void Set_isAutoLogicFocus( DependencyObject obj, bool value )
        {
            obj.SetValue( _isAutoLogicFocusProperty, value );
        }

        /// <summary>
        /// Gets the is automatic keyboard focus.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static bool Get_isAutoKeyboardFocus( DependencyObject obj )
        {
            return (bool)obj.GetValue( _isAutoKeyboardFocusProperty );
        }

        /// <summary>
        /// Sets the is automatic keyboard focus.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public static void Set_isAutoKeyboardFocus( DependencyObject obj, bool value )
        {
            obj.SetValue( _isAutoKeyboardFocusProperty, value );
        }

        /// <summary>
        /// The is automatic logic focus property
        /// </summary>
        public static readonly DependencyProperty _isAutoLogicFocusProperty =
            DependencyProperty.RegisterAttached( "_isAutoLogicFocus", typeof( bool ),
                typeof( FocusUtils ),
                new PropertyMetadata( false, PropertyIsAutoLogicFocusChanged ) );

        /// <summary>
        /// The is automatic keyboard focus property
        /// </summary>
        public static readonly DependencyProperty _isAutoKeyboardFocusProperty =
            DependencyProperty.RegisterAttached( "_isAutoKeyboardFocus", typeof( bool ),
                typeof( FocusUtils ),
                new PropertyMetadata( false, PropertyIsAutoKeyboardFocusChanged ) );

        /// <summary>
        /// Properties the is automatic logic focus changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        /// <exception cref="System.InvalidOperationException">
        /// You can only attach this property to FrameworkElement.
        /// </exception>
        private static void PropertyIsAutoLogicFocusChanged( DependencyObject d,
            DependencyPropertyChangedEventArgs e )
        {
            if( d is not FrameworkElement _element )
            {
                var _message = "You can only attach this property to FrameworkElement.";
                throw new InvalidOperationException( _message );
            }

            RoutedEventHandler _loaded = ( s, e ) => _element.Focus( );
            if( e.NewValue is bool _b && _b )
            {
                _element.Loaded += _loaded;
            }
            else
            {
                _element.Loaded -= _loaded;
            }
        }

        /// <summary>
        /// Properties the is automatic keyboard focus changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.
        /// </param>
        /// <exception cref="System.InvalidOperationException">
        /// You can only attach this property to FrameworkElement.
        /// </exception>
        private static void PropertyIsAutoKeyboardFocusChanged( DependencyObject d,
            DependencyPropertyChangedEventArgs e )
        {
            if( d is not FrameworkElement _element )
            {
                var _message = "You can only attach this property to FrameworkElement.";
                throw new InvalidOperationException( _message );
            }

            RoutedEventHandler _loaded = ( s, e ) => Keyboard.Focus( _element );
            if( e.NewValue is bool _b && _b )
            {
                _element.Loaded += _loaded;
            }
            else
            {
                _element.Loaded -= _loaded;
            }
        }
    }
}