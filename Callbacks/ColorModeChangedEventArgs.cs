// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 10-31-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        10-31-2024
// ******************************************************************************************
// <copyright file="ColorModeChangedEventArgs.cs" company="Terry D. Eppler">
//   An open source windows (wpf) application for interacting with Chat GPT that's developed
//   in C-Sharp under the MIT license
// 
//    Copyright ©  2020-2024 Terry D. Eppler
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
//   ColorModeChangedEventArgs.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.EventArgs" />
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public class ColorModeChangedEventArgs : EventArgs
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Booger.ColorModeChangedEventArgs" /> class.
        /// </summary>
        /// <param name="colorMode">The color mode.</param>
        /// <param name="actualColorMode">The actual color mode.</param>
        public ColorModeChangedEventArgs( ColorMode colorMode, ColorMode actualColorMode )
        {
            ColorMode = colorMode;
            ActualColorMode = actualColorMode;
        }

        /// <summary>
        /// Gets the color mode.
        /// </summary>
        /// <value>
        /// The color mode.
        /// </value>
        public ColorMode ColorMode { get; }

        /// <summary>
        /// Gets the actual color mode.
        /// </summary>
        /// <value>
        /// The actual color mode.
        /// </value>
        public ColorMode ActualColorMode { get; }
    }
}