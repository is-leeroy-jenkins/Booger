﻿// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-05-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-05-2024
// ******************************************************************************************
// <copyright file="Provider.cs" company="Terry D. Eppler">
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
//   Provider.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public enum Provider
    {
        /// <summary>
        /// The SQLite data provider
        /// </summary>
        SQLite = 0,

        /// <summary>
        /// The excel
        /// </summary>
        Excel = 1,

        /// <summary>
        /// The SQL server data provider
        /// </summary>
        SqlServer = 2,

        /// <summary>
        /// The SQL Compact data provider
        /// </summary>
        SqlCe = 3,

        /// <summary>
        /// The MS Access data provider
        /// </summary>
        Access = 4,

        /// <summary>
        /// The OLE DB data provider
        /// </summary>
        OleDb = 5,

        /// <summary>
        /// The CSV data provider
        /// </summary>
        CSV = 6,

        /// <summary>
        /// The TXT data provider
        /// </summary>
        Text = 7
    }
}