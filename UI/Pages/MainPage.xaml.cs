

namespace Booger
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using CommunityToolkit.Mvvm.Input;

    public partial class MainPage : Page
    {
        public MainPage(
            AppWindow appWindow,
            MainPageModel viewModel,
            AppGlobalData appGlobalData,
            PageService pageService,
            NoteService noteService,
            ChatService chatService,
            ChatPageService chatPageService,
            ChatStorageService chatStorageService,
            ConfigurationService configurationService,
            SmoothScrollingService smoothScrollingService)
        {
            AppWindow = appWindow;
            ViewModel = viewModel;
            AppGlobalData = appGlobalData;
            PageService = pageService;
            NoteService = noteService;
            ChatService = chatService;
            ChatPageService = chatPageService;
            ChatStorageService = chatStorageService;
            ConfigurationService = configurationService;
            DataContext = this;

            foreach (var session in ChatStorageService.GetAllSessions())
                AppGlobalData.Sessions.Add(new ChatSessionModel(session));

            if (AppGlobalData.Sessions.Count == 0)
                NewSession();

            InitializeComponent();

            SwitchPageToCurrentSession();

            smoothScrollingService.Register(sessionsScrollViewer);
        }

        public AppWindow AppWindow { get; }
        public MainPageModel ViewModel { get; }
        public AppGlobalData AppGlobalData { get; }
        public PageService PageService { get; }
        public NoteService NoteService { get; }
        public ChatService ChatService { get; }
        public ChatPageService ChatPageService { get; }
        public ChatStorageService ChatStorageService { get; }
        public ConfigurationService ConfigurationService { get; }


        [RelayCommand]
        public void GoToConfigPage()
        {
            AppWindow.Navigate<ConfigPage>();
        }

        [RelayCommand]
        public async Task ResetChat()
        {
            if (AppGlobalData.SelectedSession != null)
            {
                Guid sessionId = AppGlobalData.SelectedSession.Id;

                ChatService.Cancel();
                ChatStorageService.ClearMessage(sessionId);
                ViewModel.CurrentChat?.ViewModel.Messages.Clear();

                await NoteService.ShowAndWaitAsync("Chat has been reset.", 1500);
            }
            else
            {
                await NoteService.ShowAndWaitAsync("You need to select a session.", 1500);
            }
        }

        [RelayCommand]
        public void NewSession()
        {
            ChatSession session = ChatSession.Create();
            ChatSessionModel sessionModel = new ChatSessionModel(session);

            ChatStorageService.SaveSession(session);
            AppGlobalData.Sessions.Add(sessionModel);

            AppGlobalData.SelectedSession = sessionModel;
        }

        [RelayCommand]
        public void DeleteSession(ChatSessionModel session)
        {
            if (AppGlobalData.Sessions.Count == 1)
            {
                NoteService.Show("You can't delete the last session.", 1500);
                return;
            }

            int index = 
                AppGlobalData.Sessions.IndexOf(session);
            int newIndex =
                Math.Max(0, index - 1);

            ChatPageService.RemovePage(session.Id);
            ChatStorageService.DeleteSession(session.Id);
            AppGlobalData.Sessions.Remove(session);

            AppGlobalData.SelectedSession = AppGlobalData.Sessions[newIndex];
        }

        [RelayCommand]
        public void SwitchToNextSession()
        {
            int nextIndex;
            int lastIndex = AppGlobalData.Sessions.Count - 1;

            if (AppGlobalData.SelectedSession != null)
                nextIndex = AppGlobalData.Sessions.IndexOf(AppGlobalData.SelectedSession) + 1;
            else
                nextIndex = 0;

            nextIndex = Math.Clamp(nextIndex, 0, lastIndex);

            AppGlobalData.SelectedSession = 
                AppGlobalData.Sessions[nextIndex];
        }

        [RelayCommand]
        public void SwitchToPreviousSession()
        {
            int previousIndex;
            int lastIndex = AppGlobalData.Sessions.Count - 1;

            if (AppGlobalData.SelectedSession != null)
                previousIndex = AppGlobalData.Sessions.IndexOf(AppGlobalData.SelectedSession) - 1;
            else
                previousIndex = 0;

            previousIndex = Math.Clamp(previousIndex, 0, lastIndex);

            AppGlobalData.SelectedSession =
                AppGlobalData.Sessions[previousIndex];
        }


        [RelayCommand]
        public void CycleSwitchToNextSession()
        {
            int nextIndex;
            int lastIndex = AppGlobalData.Sessions.Count - 1;

            if (AppGlobalData.SelectedSession != null)
                nextIndex = AppGlobalData.Sessions.IndexOf(AppGlobalData.SelectedSession) + 1;
            else
                nextIndex = 0;

            if (nextIndex > lastIndex)
                nextIndex = 0;

            AppGlobalData.SelectedSession =
                AppGlobalData.Sessions[nextIndex];
        }

        [RelayCommand]
        public void CycleSwitchToPreviousSession()
        {
            int previousIndex;
            int lastIndex = AppGlobalData.Sessions.Count - 1;

            if (AppGlobalData.SelectedSession != null)
                previousIndex = AppGlobalData.Sessions.IndexOf(AppGlobalData.SelectedSession) - 1;
            else
                previousIndex = 0;

            if (previousIndex < 0)
                previousIndex = lastIndex;

            AppGlobalData.SelectedSession =
                AppGlobalData.Sessions[previousIndex];
        }

        [RelayCommand]
        public void DeleteCurrentSession()
        {
            if (AppGlobalData.SelectedSession != null)
                DeleteSession(AppGlobalData.SelectedSession);
        }


        [RelayCommand]
        public void SwitchPageToCurrentSession()
        {
            if (AppGlobalData.SelectedSession != null)
            {
                ViewModel.CurrentChat =
                    ChatPageService.GetPage(AppGlobalData.SelectedSession.Id);
            }
        }
    }
}
