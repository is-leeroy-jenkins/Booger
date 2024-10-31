

namespace Booger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    public class TitleGenerationService
    {
        public TitleGenerationService(
            LanguageService languageService)
        {
            LanguageService = languageService;
        }

        HttpClient _httpClient = new HttpClient();

        public LanguageService LanguageService { get; }

        public async Task<string> GenerateAsync(string[] messages)
        {
            string _languageCode = LanguageService.CurrentLanguage.Name;

            if (_languageCode == "zh-Hans")
                _languageCode = "zh-CN";      // 伞兵 Edge API 只能识别 zh-CN, 不能识别 zh-Hans

            object _payload = new
            {
                experimentId = string.Empty,
                language = _languageCode,
                targetGroup = messages
                    .Select(msg => new
                    {
                        title = msg,
                        url = "https://question.com"
                    })
                    .ToArray()
            };

            var _response = await _httpClient.PostAsJsonAsync(
                "https://edge.microsoft.com/taggrouptitlegeneration/api/TitleGeneration/gen", _payload);

            if (!_response.IsSuccessStatusCode)
                return null;

            try
            {
                Dictionary<string, double> _titles = await _response.Content.ReadFromJsonAsync<Dictionary<string, double>>();

                if (_titles == null || _titles.Count == 0)
                    return null;

                return _titles.MaxBy(title => title.Value).Key;
            }
            catch
            {
                return null;
            }
        }
    }
}
