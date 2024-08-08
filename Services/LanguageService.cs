

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;


    public class LanguageService
    {
        private static string _resourceUriPrefix = "pack://application:,,,";

        private static Dictionary<CultureInfo, ResourceDictionary> _languageResources =
            new Dictionary<CultureInfo, ResourceDictionary>()
            {
                { new CultureInfo("en"), new ResourceDictionary() { Source = new Uri($"{LanguageService._resourceUriPrefix}/Resources/Languages/en.xaml" ) } },
                { new CultureInfo("zh-hans"), new ResourceDictionary() { Source = new Uri($"{LanguageService._resourceUriPrefix}/Resources/Languages/zh-hans.xaml" ) } },
                { new CultureInfo("zh-hant"), new ResourceDictionary() { Source = new Uri($"{LanguageService._resourceUriPrefix}/Resources/Languages/zh-hant.xaml" ) } },
                { new CultureInfo("ja"), new ResourceDictionary() { Source = new Uri($"{LanguageService._resourceUriPrefix}/Resources/Languages/ja.xaml" ) } },
                { new CultureInfo("ar"), new ResourceDictionary() { Source = new Uri($"{LanguageService._resourceUriPrefix}/Resources/Languages/ar.xaml" ) } },
                { new CultureInfo("es"), new ResourceDictionary() { Source = new Uri($"{LanguageService._resourceUriPrefix}/Resources/Languages/es.xaml" ) } },
                { new CultureInfo("fr"), new ResourceDictionary() { Source = new Uri($"{LanguageService._resourceUriPrefix}/Resources/Languages/fr.xaml" ) } },
                { new CultureInfo("ru"), new ResourceDictionary() { Source = new Uri($"{LanguageService._resourceUriPrefix}/Resources/Languages/ru.xaml" ) } },
                { new CultureInfo("ur"), new ResourceDictionary() { Source = new Uri($"{LanguageService._resourceUriPrefix}/Resources/Languages/ur.xaml" ) } },
                { new CultureInfo("tr"), new ResourceDictionary() { Source = new Uri($"{LanguageService._resourceUriPrefix}/Resources/Languages/tr.xaml" ) } },
            };

        private static CultureInfo _defaultLanguage =
            new CultureInfo("en");

        public LanguageService(
            ConfigurationService configurationService)
        {
            ConfigurationService = configurationService;
        }


        public void Init()
        {
            CultureInfo _language = CultureInfo.CurrentCulture;
            if (!string.IsNullOrWhiteSpace(ConfigurationService.Configuration.Language))
                _language = new CultureInfo(ConfigurationService.Configuration.Language);

            SetLanguage(_language);
        }

        private ConfigurationService ConfigurationService { get; }


        public IEnumerable<CultureInfo> Languages =>
            LanguageService._languageResources.Keys;


        private CultureInfo _currentLanguage =
            LanguageService._defaultLanguage;
        public CultureInfo CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (!SetLanguage(value))
                    throw new ArgumentException("Unsupport language");
            }
        }

        public bool SetLanguage(CultureInfo language)
        {
            CultureInfo _key = Languages
                .Where(key => key.Equals(language))
                .FirstOrDefault();

            if (_key == null)
                _key = Languages
                    .Where(key => key.TwoLetterISOLanguageName == language.TwoLetterISOLanguageName)
                    .FirstOrDefault();

            if (_key != null)
            {
                ResourceDictionary _resourceDictionary = LanguageService._languageResources[_key];

                var _oldLanguageResources =
                    Application.Current.Resources.MergedDictionaries
                        .Where(dict => dict.Contains("IsLanguageResource"))
                        .ToList();

                foreach (var _res in _oldLanguageResources)
                    Application.Current.Resources.MergedDictionaries.Remove(_res);

                Application.Current.Resources.MergedDictionaries.Add(_resourceDictionary);

                _currentLanguage = _key;
                return true;
            }

            return false;
        }
    }
}
