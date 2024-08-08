// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="AppWindow.xaml.cs" company="Terry D. Eppler">
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
//   AppWindow.xaml.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;
    using CommunityToolkit.Mvvm.Input;

    public partial class AppWindow : Window
    {
        public AppWindow( AppWindowModel viewModel,
            PageService pageService,
            NoteService noteService,
            LanguageService languageService,
            ColorModeService colorModeService )
        {
            ViewModel = viewModel;
            PageService = pageService;
            NoteService = noteService;
            LanguageService = languageService;
            ColorModeService = colorModeService;
            DataContext = this;
            InitializeComponent( );
        }

        public AppWindowModel ViewModel { get; }

        public PageService PageService { get; }

        public NoteService NoteService { get; }

        public LanguageService LanguageService { get; }

        public ColorModeService ColorModeService { get; }

        public void Navigate<TPage>( ) where TPage : class
        {
            appFrame.Navigate( PageService.GetPage<TPage>( ) );
        }

        protected override void OnSourceInitialized( EventArgs e )
        {
            base.OnSourceInitialized( e );
            EntryPoint.MainWindowHandle =
                new WindowInteropHelper( this ).Handle;

            LanguageService.Init( );
            ColorModeService.Init( );
        }
    }
}