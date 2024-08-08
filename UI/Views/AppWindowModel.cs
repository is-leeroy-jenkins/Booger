
namespace Booger
{
    using CommunityToolkit.Mvvm.ComponentModel;

    public class AppWindowModel : ObservableObject
    {
        public AppWindowModel(ConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }

        public string ApplicationTitle => App.AppName;

        public ConfigurationService ConfigurationService { get; }

        public AppConfig Configuration => ConfigurationService.Configuration;
    }
}
