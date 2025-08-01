using Microsoft.Extensions.Configuration;

namespace weather_api_sysprog_1.Configuration
{
    public class AppSettings
    {
        public string WeatherApiKey { get; set; } = string.Empty;
        public int Port { get; set; }
        public int MaxCacheSize { get; set; }

        public string GetListenerPrefix() => $"http://localhost:{Port}/";

        public string GetApiUrl(string query) =>
            $"http://api.weatherapi.com/v1/forecast.json?key={WeatherApiKey}{query}";

        public AppSettings()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                .Build();

            WeatherApiKey = config["WeatherApiKey"] ?? string.Empty;
            Port = int.TryParse(config["Port"], out var port) ? port : 8080;
            MaxCacheSize = int.TryParse(config["MaxCacheSize"], out var maxCacheSize) ? maxCacheSize : 100; // U AppSettings.json je kes stavljen na 2 radi testiranja
        }
    }
}
