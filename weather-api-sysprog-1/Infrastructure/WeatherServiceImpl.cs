using weather_api_sysprog_1.Configuration;
using weather_api_sysprog_1.Core.Entities;
using weather_api_sysprog_1.Core.Interfaces;
using weather_api_sysprog_1.Infrastructure.Mappers;

namespace weather_api_sysprog_1.Infrastructure
{
    public class WeatherServiceImpl : IWeatherService
    {
        private readonly AppSettings _settings;
        private readonly HttpClient _httpClient = new();

        public WeatherServiceImpl(AppSettings settings)
        {
            _settings = settings;
        }

        public WeatherForecast GetWeather(string query)
        {
            if (query.StartsWith('?'))
            {
                query = query.Substring(1); // Uklanja '?' ako je prisutan
            }

            // Kreira URL za third party api poziv
            var url = _settings.GetApiUrl(query);
            
            // HTTP GET zahtev
            var response = _httpClient.GetAsync(url).Result;
            
            response.EnsureSuccessStatusCode(); // ovo baca exception

            var json = response.Content.ReadAsStringAsync().Result;
            return WeatherMapper.MapFromJson(json);
        }
    }
}
