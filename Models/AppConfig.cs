﻿
namespace Booger
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class AppConfig : ObservableObject
    {
        [ObservableProperty]
        private string _apiHost = "openaiapi.elecho.org";

        [ObservableProperty]
        private string _apiKey = string.Empty;

        [ObservableProperty]
        private string _organization = string.Empty;

        [ObservableProperty]
        private string _model = "gpt-3.5-turbo";

        [ObservableProperty]
        private int _apiTimeout = 5000;

        [ObservableProperty]
        private double _temerature = .5;

        [ObservableProperty]
        private bool _enableChatContext = true;

        [ObservableProperty]
        private string[] _systemMessages = new string[]
        {

        };

        [ObservableProperty]
        private string _language = string.Empty;

        [ObservableProperty]
        private ColorMode _colorMode = ColorMode.Auto;

        [ObservableProperty]
        private bool _enableTitleGeneration = true;

        [ObservableProperty]
        private bool _windowAlwaysOnTop = false;

        [ObservableProperty]
        private bool _disableChatAnimation = false;

        [ObservableProperty]
        private string _chatStoragePath = "AppChatStorage.db";
    }
}
