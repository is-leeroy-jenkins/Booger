

namespace Booger
{
    using System.IO;
    using System.Text.Json;
    using Microsoft.Extensions.Options;

    public class ConfigurationService
    {
        public ConfigurationService(IOptions<AppConfig> configuration)
        {
            OptionalConfiguration = configuration;
        }

        private IOptions<AppConfig> OptionalConfiguration { get; }

        public AppConfig Configuration => OptionalConfiguration.Value;

        public void Save()
        {
            using FileStream _fs = File.Create(GlobalValues.JsonConfigurationFilePath);
            JsonSerializer.Serialize(_fs, OptionalConfiguration.Value, JsonHelper.ConfigurationOptions);
        }
    }
}
