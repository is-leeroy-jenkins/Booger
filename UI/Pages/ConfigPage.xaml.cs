// ******************************************************************************************
//     Assembly:                Booger
//     Author:                  Terry D. Eppler
//     Created:                 08-08-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        08-08-2024
// ******************************************************************************************
// <copyright file="ConfigPage.xaml.cs" company="Terry D. Eppler">
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
//   ConfigPage.xaml.cs
// </summary>
// ******************************************************************************************

namespace Booger
{
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using CommunityToolkit.Mvvm.Input;

    /// <summary>
    /// Interaction logic for ConfigPage.xaml
    /// </summary>
    public partial class ConfigPage : Page
    {
        public ConfigPage( MainWindow appWindow,
            ConfigPageModel viewModel,
            PageService pageService,
            NoteService noteService,
            LanguageService languageService,
            ColorModeService colorModeService,
            ConfigurationService configurationService,
            SmoothScrollingService smoothScrollingService )
        {
            AppWindow = appWindow;
            ViewModel = viewModel;
            PageService = pageService;
            NoteService = noteService;
            LanguageService = languageService;
            ColorModeService = colorModeService;
            ConfigurationService = configurationService;
            DataContext = this;
            LoadSystemMessagesCore( );
            InitializeComponent( );
            smoothScrollingService.Register( configurationScrollViewer );
        }

        public MainWindow AppWindow { get; }

        public ConfigPageModel ViewModel { get; }

        public PageService PageService { get; }

        public NoteService NoteService { get; }

        public LanguageService LanguageService { get; }

        public ColorModeService ColorModeService { get; }

        public ConfigurationService ConfigurationService { get; }

        private void LoadSystemMessagesCore( )
        {
            ViewModel.SystemMessages.Clear( );
            foreach( var _msg in ConfigurationService.Configuration.SystemMessages )
            {
                ViewModel.SystemMessages.Add( new ValueWrapper<string>( _msg ) );
            }
        }

        private void ApplySystemMessagesCore( )
        {
            ConfigurationService.Configuration.SystemMessages = ViewModel.SystemMessages
                .Select( wraper => wraper.Value )
                .ToArray( );
        }

        [ RelayCommand ]
        public void GoToMainPage( )
        {
            AppWindow.Navigate<MainPage>( );
        }

        [ RelayCommand ]
        public void AboutOpenChat( )
        {
            MessageBox.Show( Application.Current.MainWindow,
                $"""
                 {nameof( Booger )}, by SlimeNull v{Assembly.GetEntryAssembly( )?.GetName( )?.Version}

                 A simple chat client based on OpenAI Chat completion API.

                 Repository: https://github.com/SlimeNull/{nameof( Booger )}
                 """,
                $"About {nameof( Booger )}", MessageBoxButton.OK, MessageBoxImage.Information );
        }

        [ RelayCommand ]
        public Task LoadSystemMessages( )
        {
            LoadSystemMessagesCore( );
            return NoteService.ShowAndWaitAsync( "System messages loaded", 1500 );
        }

        [ RelayCommand ]
        public Task ApplySystemMessages( )
        {
            ApplySystemMessagesCore( );
            return NoteService.ShowAndWaitAsync( "System messages applied", 1500 );
        }

        [ RelayCommand ]
        public void AddSystemMessage( )
        {
            ViewModel.SystemMessages.Add( new ValueWrapper<string>( "New system message" ) );
        }

        [ RelayCommand ]
        public void RemoveSystemMessage( )
        {
            if( ViewModel.SystemMessages.Count > 0 )
            {
                ViewModel.SystemMessages.RemoveAt( ViewModel.SystemMessages.Count - 1 );
            }
        }

        [ RelayCommand ]
        public Task SaveConfiguration( )
        {
            ConfigurationService.Configuration.Language =
                LanguageService.CurrentLanguage.ToString( );

            ConfigurationService.Configuration.ColorMode =
                ColorModeService.CurrentMode;

            ConfigurationService.Save( );
            return NoteService.ShowAndWaitAsync( "Configuration saved", 2000 );
        }
    }
}