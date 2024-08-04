﻿namespace Booger
{
    using System;
    using System.IO;
    using System.Windows;
    using RestoreWindowPlace;

    public partial class App : Application
    {
        private WindowPlace _windowPlace;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // TODO 1: See README.md and get your OpenAI API key: https://platform.openai.com/account/api-keys
                // TODO 2: You can modify ChatViewModel to set default selected language: _selectedLang = LangList[..]
                // TODO 3: Give my article 5 stars:) at https://www.codeproject.com/Tips/5377103/ChatGPT-API-in-Csharp-WPF-XAML-MVVM

                string openaiApiKey;
                if (e.Args?.Length > 0 && e.Args[0].StartsWith('/'))
                {
                    // OpenAI API key from command line parameter such as "/sk-Ih...WPd" after removing '/'
                    openaiApiKey = e.Args[0].Remove(0, 1);
                }
                else
                {                    
                    // Put your key from above here instead of using a command line parameter in the 'if' block
                    openaiApiKey = "<Your Open AI API Key is something like \"sk-Ih...WPd\">";
                }

                // Programmatically switch between SqlHistoryRepo and EmptyHistoryRepo
                // If you have configured SQL Server, try SqlHistoryRepo
                //IHistoryRepo historyRepo = new SqlHistoryRepo();
                IHistoryRepo historyRepo = new EmptyHistoryRepo();
                var chatGPTService = new ChatGptService(openaiApiKey);
                var mainViewModel = new ChatViewModel(historyRepo, chatGPTService);
                var mainWindow = new ChatWindow(mainViewModel);
                SetupRestoreWindowPlace(mainWindow);
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Booger will exit on error");
                Current?.Shutdown();
            }
        }        

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            try
            {
                _windowPlace?.Save();
            }
            catch (Exception)
            {
            }
        }

        private void SetupRestoreWindowPlace(ChatWindow mainWindow)
        {
            var windowPlaceConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Booger.config");
            _windowPlace = new WindowPlace(windowPlaceConfigFilePath);
            _windowPlace.Register(mainWindow);

            // This logic works but I don't like the window being maximized
            //if (!File.Exists(windowPlaceConfigFilePath))
            //{
            //    // For the first time, maximize the window, so it won't go off the screen on laptop
            //    // WindowPlacement will take care of future runs
            //    mainWindow.WindowState = WindowState.Maximized;
            //}
        }
    }
}