namespace Booger
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;
    using CommunityToolkit.Mvvm.Input;

    public partial class AppWindow : Window
    {
        public AppWindow(
            AppWindowModel viewModel,
            PageService pageService,
            NoteService noteService,
            LanguageService languageService,
            ColorModeService colorModeService)
        {
            ViewModel = viewModel;
            PageService = pageService;
            NoteService = noteService;
            LanguageService = languageService;
            ColorModeService = colorModeService;
            DataContext = this;

            InitializeComponent();
        }

        public AppWindowModel ViewModel { get; }
        public PageService PageService { get; }
        public NoteService NoteService { get; }
        public LanguageService LanguageService { get; }
        public ColorModeService ColorModeService { get; }


        public void Navigate<TPage>() where TPage : class
        {
            appFrame.Navigate(
                PageService.GetPage<TPage>());
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            EntryPoint.MainWindowHandle = 
                new WindowInteropHelper(this).Handle;

            LanguageService.Init( );
            ColorModeService.Init( );
        }
    }
}
