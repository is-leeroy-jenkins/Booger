// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="UiUtils.cs" company="Terry D. Eppler">
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
//   UiUtils.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// 
    /// </summary>
    public static class UiUtils
    {
        /// <summary>
        /// Gets the corner radius.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        [ AttachedPropertyBrowsableForType( typeof( FrameworkElement ) ) ]
        public static CornerRadius Get_cornerRadius( DependencyObject obj )
        {
            return (CornerRadius)obj.GetValue( _cornerRadiusProperty );
        }

        /// <summary>
        /// Sets the corner radius.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="value">The value.</param>
        public static void Set_cornerRadius( DependencyObject obj, CornerRadius value )
        {
            obj.SetValue( _cornerRadiusProperty, value );
        }

        /// <summary>
        /// The corner radius property
        /// </summary>
        public static readonly DependencyProperty _cornerRadiusProperty =
            DependencyProperty.RegisterAttached( "_cornerRadius", typeof( CornerRadius ),
                typeof( UiUtils ),
                new PropertyMetadata( new CornerRadius( ), UiUtils.CornerRadiusChanged ) );

        /// <summary>
        /// Corners the radius changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/>
        /// instance containing the event data.</param>
        private static void CornerRadiusChanged( DependencyObject d,
            DependencyPropertyChangedEventArgs e )
        {
            if( d is not FrameworkElement _ele )
            {
                return;
            }

            void ApplyBorder( FrameworkElement ele )
            {
                if( UiUtils.GetBorderFromControl( ele ) is not Border _border )
                {
                    return;
                }

                _border.CornerRadius = (CornerRadius)e.NewValue;
            }

            void LoadedOnce( object sender, RoutedEventArgs _e )
            {
                ApplyBorder( _ele );
                _ele.Loaded -= LoadedOnce;
            }

            if( _ele.IsLoaded )
            {
                ApplyBorder( _ele );
            }
            else
            {
                _ele.Loaded += LoadedOnce;
            }
        }

        /// <summary>
        /// Gets the border from control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        private static Border GetBorderFromControl( FrameworkElement control )
        {
            if( control is Border _border )
            {
                return _border;
            }

            var _childrenCount = VisualTreeHelper.GetChildrenCount( control );
            for( var _i = 0; _i < _childrenCount; _i++ )
            {
                var _child = VisualTreeHelper.GetChild( control, _i );
                if( _child is not FrameworkElement _childElement )
                {
                    continue;
                }

                if( _child is Border _borderChild )
                {
                    return _borderChild;
                }

                if( UiUtils.GetBorderFromControl( _childElement ) is Border _childBorderChild )
                {
                    return _childBorderChild;
                }
            }

            return null;
        }
    }
}