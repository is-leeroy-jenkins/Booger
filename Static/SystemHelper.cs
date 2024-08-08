// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="SystemHelper.cs" company="Terry D. Eppler">
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
//   SystemHelper.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Windows;
    using Microsoft.Win32;

    /// <summary>
    /// 
    /// </summary>
    public static class SystemHelper
    {
        /// <summary>
        /// Determines whether [is dark theme depend on color].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is dark theme depend on color]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsDarkThemeDependOnColor( )
        {
            var _themeColor = SystemParameters.WindowGlassColor;
            var _brightness =
                ColorHelper.GetBrightness( _themeColor.R, _themeColor.G, _themeColor.B );

            return _brightness < 0.3;
        }

        /// <summary>
        /// Determines whether [is dark theme depend on registry].
        /// </summary>
        /// <returns></returns>
        private static bool? IsDarkThemeDependOnRegistry( )
        {
            const string REGISTRY_KEY_PATH =
                @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

            const string REGISTRY_VALUE_NAME = "AppsUseLightTheme";
            var _registryValueObject = Registry.CurrentUser.OpenSubKey( REGISTRY_KEY_PATH )
                ?.GetValue( REGISTRY_VALUE_NAME );

            if( _registryValueObject is null )
            {
                return false;
            }

            return ( _registryValueObject == null );
        }

        /// <summary>
        /// Determines whether [is dark theme].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is dark theme]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDarkTheme( )
        {
            return IsDarkThemeDependOnRegistry( )
                ?? IsDarkThemeDependOnColor( );
        }
    }
}