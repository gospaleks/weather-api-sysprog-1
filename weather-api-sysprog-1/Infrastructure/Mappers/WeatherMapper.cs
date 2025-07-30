using System.Text.Json;
using weather_api_sysprog_1.Core.Entities;

namespace weather_api_sysprog_1.Infrastructure.Mappers
{
    public static class WeatherMapper
    {
        public static WeatherForecast MapFromJson(string json)
        {
            using JsonDocument doc = JsonDocument.Parse(json);

            return new WeatherForecast
            {
                LocationName = doc.RootElement.GetProperty("location").GetProperty("name").GetString() ?? string.Empty,
                LocationRegion = doc.RootElement.GetProperty("location").GetProperty("region").GetString() ?? string.Empty,
                LocationCountry = doc.RootElement.GetProperty("location").GetProperty("country").GetString() ?? string.Empty,
                CurrentLastUpdated = doc.RootElement.GetProperty("current").GetProperty("last_updated").GetString() ?? string.Empty,
                CurrentTempC = doc.RootElement.GetProperty("current").GetProperty("temp_c").GetDouble(),
                CurrentFeelsLikeC = doc.RootElement.GetProperty("current").GetProperty("feelslike_c").GetDouble(),
                CurrentConditionText = doc.RootElement.GetProperty("current").GetProperty("condition").GetProperty("text").GetString() ?? string.Empty,
                CurrentWindSpeedKph = doc.RootElement.GetProperty("current").GetProperty("wind_kph").GetDouble(),
                CurrentPressureMb = doc.RootElement.GetProperty("current").GetProperty("pressure_mb").GetDouble(),
                
                ForecastDays = doc.RootElement.GetProperty("forecast").GetProperty("forecastday").EnumerateArray()
                    .Select(f => new ForecastDay
                    {
                        Date = DateOnly.FromDateTime(f.GetProperty("date").GetDateTime()),
                        MaxtempC = f.GetProperty("day").GetProperty("maxtemp_c").GetDouble(),
                        MintempC = f.GetProperty("day").GetProperty("mintemp_c").GetDouble(),
                        AvgtempC = f.GetProperty("day").GetProperty("avgtemp_c").GetDouble(),
                        MaxwindKph = f.GetProperty("day").GetProperty("maxwind_kph").GetDouble(),
                        Avghumidity = f.GetProperty("day").GetProperty("avghumidity").GetInt32(),
                        DailyWillItRain = f.GetProperty("day").GetProperty("daily_will_it_rain").GetInt32() != 0,
                        DailyChanceOfRain = f.GetProperty("day").GetProperty("daily_chance_of_rain").GetInt32(),
                        DailyWillItSnow = f.GetProperty("day").GetProperty("daily_will_it_snow").GetInt32() != 0,
                        DailyChanceOfSnow = f.GetProperty("day").GetProperty("daily_chance_of_snow").GetInt32(),
                        Uv = f.GetProperty("day").GetProperty("uv").GetDouble(),
                        Text = f.GetProperty("day").GetProperty("condition").GetProperty("text").GetString() ?? string.Empty
                    }).ToList()
            };
        }
    }
}
