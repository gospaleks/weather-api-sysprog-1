using weather_api_sysprog_1.Core.Entities;

namespace weather_api_sysprog_1.Core.Interfaces
{
    public interface IWeatherService
    {
        WeatherForecast GetWeather(string query);
    }
}
