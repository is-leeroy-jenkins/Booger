// ******************************************************************************************
//     Assembly:                Booger GPT
//     Author:                  Terry D. Eppler
//     Created:                 05-24-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        05-24-2024
// ******************************************************************************************
// <copyright file="ExtensionHelper.cs" company="Terry D. Eppler">
//    This is a Federal Budget, Finance, and Accounting application
//    for the US Environmental Protection Agency (US EPA).
//    Copyright ©  2024  Terry Eppler
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
//    You can contact me at:   terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   ExtensionHelper.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    [ SuppressMessage( "ReSharper", "MemberCanBeInternal" ) ]
    [ SuppressMessage( "ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "CompareNonConstrainedGenericWithNull" ) ]
    public static class Extensions
    {
        /// <summary>
        /// Determines whether this instance is blank.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if the specified string is blank; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBlank( this string str )
        {
            return string.IsNullOrEmpty( str );
        }

        /// <summary>
        /// Determines whether [is not blank].
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>
        ///   <c>true</c> if [is not blank] [the specified string]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotBlank( this string str )
        {
            return !IsBlank( str );
        }

        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>
        ///   <c>true</c> if the specified c is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty( this ICollection c )
        {
            return ( c == null ) || ( c.Count == 0 );
        }

        /// <summary>
        /// Determines whether [is not empty].
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns>
        ///   <c>true</c> if [is not empty] [the specified c]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotEmpty( this ICollection c )
        {
            return !IsEmpty( c );
        }

        /// <summary>
        /// Ins the specified test values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="testValues">The test values.</param>
        /// <returns></returns>
        public static bool In<T>( this T item, params T[ ] testValues )
        {
            if( item != null )
            {
                foreach( var _testValue in testValues )
                {
                    if( _testValue != null
                       && _testValue.Equals( item ) )
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Nots the in.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="testValues">The test values.</param>
        /// <returns></returns>
        public static bool NotIn<T>( this T item, params T[ ] testValues )
        {
            return !In( item, testValues );
        }
    }
}