using System.Text.Json;
using weather_api_sysprog_1.Core.Entities;
using weather_api_sysprog_1.Infrastructure.Extensions;

namespace weather_api_sysprog_1.Infrastructure.Mappers
{
    public static class WeatherMapper
    {
        public static WeatherForecast MapFromJson(string json)
        {
            using JsonDocument doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var location = root.GetProperty("location");
            var current = root.GetProperty("current");

            return new WeatherForecast
            {
                LocationName = location.GetStringSafe("name"),
                LocationRegion = location.GetStringSafe("region"),
                LocationCountry = location.GetStringSafe("country"),
                CurrentLastUpdated = current.GetStringSafe("last_updated"),
                CurrentTempC = current.GetDoubleSafe("temp_c"),
                CurrentFeelsLikeC = current.GetDoubleSafe("feelslike_c"),
                CurrentConditionText = current.GetProperty("condition").GetStringSafe("text"),
                CurrentWindSpeedKph = current.GetDoubleSafe("wind_kph"),
                CurrentPressureMb = current.GetDoubleSafe("pressure_mb"),
                AirQuality = current.TryGetProperty("air_quality", out var aq) ? MapAirQuality(aq) : null,
                ForecastDays = root.GetProperty("forecast").GetProperty("forecastday")
                    .EnumerateArray()
                    .Select(f => new ForecastDay
                    {
                        Date = DateOnly.FromDateTime(f.GetProperty("date").GetDateTime()),
                        MaxtempC = f.GetProperty("day").GetDoubleSafe("maxtemp_c"),
                        MintempC = f.GetProperty("day").GetDoubleSafe("mintemp_c"),
                        AvgtempC = f.GetProperty("day").GetDoubleSafe("avgtemp_c"),
                        MaxwindKph = f.GetProperty("day").GetDoubleSafe("maxwind_kph"),
                        Avghumidity = f.GetProperty("day").GetIntSafe("avghumidity"),
                        DailyWillItRain = f.GetProperty("day").GetBoolFromInt("daily_will_it_rain"),
                        DailyChanceOfRain = f.GetProperty("day").GetIntSafe("daily_chance_of_rain"),
                        DailyWillItSnow = f.GetProperty("day").GetBoolFromInt("daily_will_it_snow"),
                        DailyChanceOfSnow = f.GetProperty("day").GetIntSafe("daily_chance_of_snow"),
                        Uv = f.GetProperty("day").GetDoubleSafe("uv"),
                        Text = f.GetProperty("day").GetProperty("condition").GetStringSafe("text"),
                        AirQuality = f.GetProperty("day").TryGetProperty("air_quality", out var dayAq) ? MapAirQuality(dayAq) : null
                    })
                    .ToList(),
                Alerts = root.TryGetProperty("alerts", out var alerts) ? 
                       alerts.GetProperty("alert").EnumerateArray().Select(MapAlert).ToList()
                       : null
            };
        }

        private static AirQuality? MapAirQuality(JsonElement element)
        {
            if (!element.ValueKind.Equals(JsonValueKind.Object)) return null;

            return new AirQuality
            {
                CO = element.GetDoubleSafe("co"),
                NO2 = element.GetDoubleSafe("no2"),
                O3 = element.GetDoubleSafe("o3"),
                SO2 = element.GetDoubleSafe("so2"),
                PM10 = element.GetDoubleSafe("pm10"),
                PM2_5 = element.GetDoubleSafe("pm2_5"),
                USEPAIndex = element.GetIntSafe("us-epa-index"),
                GBDEFRAIndex = element.GetIntSafe("gb-defra-index")
            };
        }

        private static Alert MapAlert(JsonElement element)
        {
            if (!element.ValueKind.Equals(JsonValueKind.Object)) return null;
            
            return new Alert
            {
                Headline = element.GetStringSafe("headline"),
                Description = element.GetStringSafe("description"),
                Severity = element.GetStringSafe("severity"),
                Effective = element.GetDateTime("effective"),
                Expires = element.GetDateTime("expires"),
                Areas = element.GetStringSafe("areas")
            };
        }
    }
}
