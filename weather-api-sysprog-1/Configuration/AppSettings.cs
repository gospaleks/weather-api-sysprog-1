namespace weather_api_sysprog_1.Configuration
{
    public class AppSettings
    {
        public string WeatherApiKey { get; } = "f933a2760e414bcbbe2145350253007";
        
        public int Port { get; } = 8080;

        public int MaxCacheSize { get; } = 2;

        public string GetListenerPrefix() => $"http://localhost:{Port}/";

        public string GetApiUrl(string query) =>
            $"http://api.weatherapi.com/v1/forecast.json?key={WeatherApiKey}&{query}";
    }
}
