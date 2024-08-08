namespace Booger
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class MainPageModel : ObservableObject
    {
        [ObservableProperty]
        private ChatPage _currentChat;
    }
}
