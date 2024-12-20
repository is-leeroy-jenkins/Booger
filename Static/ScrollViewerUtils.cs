﻿// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="ScrollViewerUtils.cs" company="Terry D. Eppler">
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
//   ScrollViewerUtils.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Windows.Controls;

    /// <summary>
    /// 
    /// </summary>
    public static class ScrollViewerUtils
    {
        /// <summary>
        /// Determines whether [is at top] [the specified threshold].
        /// </summary>
        /// <param name="scrollViewer">The scroll viewer.</param>
        /// <param name="threshold">The threshold.</param>
        /// <returns>
        ///   <c>true</c> if [is at top] [the specified threshold]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAtTop( this ScrollViewer scrollViewer, int threshold = 5 )
        {
            if( scrollViewer.VerticalOffset <= threshold )
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether [is at end] [the specified threshold].
        /// </summary>
        /// <param name="scrollViewer">The scroll viewer.</param>
        /// <param name="threshold">The threshold.</param>
        /// <returns>
        ///   <c>true</c> if [is at end] [the specified threshold]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAtEnd( this ScrollViewer scrollViewer, int threshold = 5 )
        {
            if( scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight )
            {
                return true;
            }

            if( scrollViewer.VerticalOffset + threshold >= scrollViewer.ScrollableHeight )
            {
                return true;
            }

            return false;
        }
    }
}