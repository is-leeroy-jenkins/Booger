// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="MarkdownXaml.Styles.cs" company="Terry D. Eppler">
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
//   MarkdownXaml.Styles.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    /// <summary>
    /// 
    /// </summary>
    public enum StyleKey
    {
        /// <summary>
        /// The main foreground
        /// </summary>
        MainForeground,

        /// <summary>
        /// The main background
        /// </summary>
        MainBackground,

        /// <summary>
        /// The main border
        /// </summary>
        MainBorder,

        /// <summary>
        /// The code block background
        /// </summary>
        CodeBlockBackground,

        /// <summary>
        /// The code block foreground
        /// </summary>
        CodeBlockForeground,

        /// <summary>
        /// The code block border
        /// </summary>
        CodeBlockBorder,

        /// <summary>
        /// The code inline foreground
        /// </summary>
        CodeInlineForeground,

        /// <summary>
        /// The code inline background
        /// </summary>
        CodeInlineBackground,

        /// <summary>
        /// The code inline border
        /// </summary>
        CodeInlineBorder,

        /// <summary>
        /// The quote block foreground
        /// </summary>
        QuoteBlockForeground,

        /// <summary>
        /// The quote block background
        /// </summary>
        QuoteBlockBackground,

        /// <summary>
        /// The quote block border
        /// </summary>
        QuoteBlockBorder,

        /// <summary>
        /// The table foreground
        /// </summary>
        TableForeground,

        /// <summary>
        /// The table background
        /// </summary>
        TableBackground,

        /// <summary>
        /// The table stripe
        /// </summary>
        TableStripe,

        /// <summary>
        /// The table border
        /// </summary>
        TableBorder,

        /// <summary>
        /// The thematic break
        /// </summary>
        ThematicBreak,

        /// <summary>
        /// The mark
        /// </summary>
        Mark
    }
}