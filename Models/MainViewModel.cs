// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-05-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-05-2024
// ******************************************************************************************
// <copyright file="MainViewModel.cs" company="Terry D. Eppler">
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
//   MainViewModel.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Reflection;

    public class MainViewModel
    {
        public MainViewModel( IHistoryRepo historyRepo, WhetstoneChatGPTService chatGPTService )
        {
            HistoryViewModel = new HistoryViewModel( historyRepo );
            LiveChatViewModel = new LiveChatViewModel( historyRepo, chatGPTService );

            // <Version>1.3</Version> in .csproj
            var appVer = Assembly.GetExecutingAssembly( ).GetName( ).Version!;
            var dotnetVer = Environment.Version;
            AppTitle =
                $"C# WPF ChatGPT v{appVer.Major}.{appVer.Minor} (.NET {dotnetVer.Major}.{dotnetVer.Minor}.{dotnetVer.Build} runtime) by Peter Sun";
#if DEBUG
            AppTitle += " - DEBUG";
#endif
        }

        public string AppTitle { get; }

        // Bind to LiveChat tab item
        public LiveChatViewModel LiveChatViewModel { get; }

        // Bind to History tab item
        public HistoryViewModel HistoryViewModel { get; }
    }
}