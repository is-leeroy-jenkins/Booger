// ******************************************************************************************
//     Assembly:              Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-04-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-04-2024
// ******************************************************************************************
// <copyright file="HistoryMessage.cs" company="Terry D. Eppler">
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
//   HistoryMessage.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    // DB counterpart (table) of class Message    
    /// <summary>
    /// 
    /// </summary>
    public class HistoryMessage
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        public string Sender { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; } = string.Empty;

        // The pair auto-defines FK HistoryChatId in HistoryChat table
        /// <summary>
        /// Gets or sets the history chat identifier.
        /// </summary>
        /// <value>
        /// The history chat identifier.
        /// </value>
        public int HistoryChatId { get; set; }// FK, not strictly required

        /// <summary>
        /// Gets or sets the history chat.
        /// </summary>
        /// <value>
        /// The history chat.
        /// </value>
        public HistoryChat HistoryChat { get; set; } = null!;
    }
}