using System.Text.Json;
using weather_api_sysprog_1.Configuration;
using weather_api_sysprog_1.Core;
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

            try
            {
                var response = _httpClient.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    string? apiErrorMessage = null;

                    /*.
                     * Api vraca JSON u formatu error: message
                     */
                    var errorJson = response.Content.ReadAsStringAsync().Result;
                    using var doc = JsonDocument.Parse(errorJson);
                    if (doc.RootElement.TryGetProperty("error", out var err))
                    {
                        apiErrorMessage = err.GetProperty("message").GetString();
                    }

                    // Propagira custom exception
                    throw new WeatherApiException(
                        $"Weather API greška: {response.StatusCode}",
                        (int)response.StatusCode,
                        apiErrorMessage
                    );
                }

                // Ako je sve ok parsira se JSON odgovor, mapira u WeatherForecast objekat i vraca
                var json = response.Content.ReadAsStringAsync().Result;
                return WeatherMapper.MapFromJson(json);
            }
            catch (HttpRequestException ex)
            {
                throw new WeatherApiException("Greška prilikom povezivanja sa Weather API-jem.", 0, ex.Message);
            }
            catch (TaskCanceledException)
            {
                throw new WeatherApiException("Zahtev ka Weather API-ju je istekao.", 0);
            }
        }
    }
}
