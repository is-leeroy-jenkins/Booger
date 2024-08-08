

namespace Booger
{
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;

    public partial class ConfigPageModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ValueWrapper<string>> _systemMessages =
            new ObservableCollection<ValueWrapper<string>>();
    }
}
