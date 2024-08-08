

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;


    public class LanguageService
    {
        private static string resourceUriPrefix = "pack://application:,,,";

        private static Dictionary<CultureInfo, ResourceDictionary> languageResources =
            new Dictionary<CultureInfo, ResourceDictionary>()
            {
                { new CultureInfo("en"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Resources/Languages/en.xaml" ) } },
                { new CultureInfo("zh-hans"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Resources/Languages/zh-hans.xaml" ) } },
                { new CultureInfo("zh-hant"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Resources/Languages/zh-hant.xaml" ) } },
                { new CultureInfo("ja"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Resources/Languages/ja.xaml" ) } },
                { new CultureInfo("ar"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Resources/Languages/ar.xaml" ) } },
                { new CultureInfo("es"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Resources/Languages/es.xaml" ) } },
                { new CultureInfo("fr"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Resources/Languages/fr.xaml" ) } },
                { new CultureInfo("ru"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Resources/Languages/ru.xaml" ) } },
                { new CultureInfo("ur"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Resources/Languages/ur.xaml" ) } },
                { new CultureInfo("tr"), new ResourceDictionary() { Source = new Uri($"{resourceUriPrefix}/Resources/Languages/tr.xaml" ) } },
            };

        private static CultureInfo defaultLanguage =
            new CultureInfo("en");

        public LanguageService(
            ConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }


        public void Init()
        {
            CultureInfo language = CultureInfo.CurrentCulture;
            if (!string.IsNullOrWhiteSpace(ConfigurationService.Configuration.Language))
                language = new CultureInfo(ConfigurationService.Configuration.Language);

            SetLanguage(language);
        }

        private ConfigurationService ConfigurationService { get; }


        public IEnumerable<CultureInfo> Languages =>
            languageResources.Keys;


        private CultureInfo currentLanguage =
            defaultLanguage;
        public CultureInfo CurrentLanguage
        {
            get => currentLanguage;
            set
            {
                if (!SetLanguage(value))
                    throw new ArgumentException("Unsupport language");
            }
        }

        public bool SetLanguage(CultureInfo language)
        {
            CultureInfo key = Languages
                .Where(key => key.Equals(language))
                .FirstOrDefault();

            if (key == null)
                key = Languages
                    .Where(key => key.TwoLetterISOLanguageName == language.TwoLetterISOLanguageName)
                    .FirstOrDefault();

            if (key != null)
            {
                ResourceDictionary resourceDictionary = languageResources[key];

                var oldLanguageResources =
                    Application.Current.Resources.MergedDictionaries
                        .Where(dict => dict.Contains("IsLanguageResource"))
                        .ToList();

                foreach (var res in oldLanguageResources)
                    Application.Current.Resources.MergedDictionaries.Remove(res);

                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

                currentLanguage = key;
                return true;
            }

            return false;
        }
    }
}
