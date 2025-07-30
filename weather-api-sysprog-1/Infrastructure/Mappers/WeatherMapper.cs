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
                ForecastDays = root.GetProperty("forecast").GetProperty("forecastday").EnumerateArray().Select(MapForecastDay).ToList(),
                Alerts = root.TryGetProperty("alerts", out var alerts) ? alerts.GetProperty("alert").EnumerateArray().Select(MapAlert).ToList() : null
            };
        }

        private static ForecastDay MapForecastDay(JsonElement element)
        {
            return new ForecastDay
            {
                Date = DateOnly.FromDateTime(element.GetProperty("date").GetDateTime()),
                MaxtempC = element.GetProperty("day").GetDoubleSafe("maxtemp_c"),
                MintempC = element.GetProperty("day").GetDoubleSafe("mintemp_c"),
                AvgtempC = element.GetProperty("day").GetDoubleSafe("avgtemp_c"),
                MaxwindKph = element.GetProperty("day").GetDoubleSafe("maxwind_kph"),
                Avghumidity = element.GetProperty("day").GetIntSafe("avghumidity"),
                DailyWillItRain = element.GetProperty("day").GetBoolFromInt("daily_will_it_rain"),
                DailyChanceOfRain = element.GetProperty("day").GetIntSafe("daily_chance_of_rain"),
                DailyWillItSnow = element.GetProperty("day").GetBoolFromInt("daily_will_it_snow"),
                DailyChanceOfSnow = element.GetProperty("day").GetIntSafe("daily_chance_of_snow"),
                Uv = element.GetProperty("day").GetDoubleSafe("uv"),
                Text = element.GetProperty("day").GetProperty("condition").GetStringSafe("text"),
                AirQuality = element.GetProperty("day").TryGetProperty("air_quality", out var dayAq) ? MapAirQuality(dayAq) : null
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
