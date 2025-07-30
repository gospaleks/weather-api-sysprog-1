namespace weather_api_sysprog_1.Core.Entities
{
    public class WeatherForecast
    {
        // Lokacija
        public string LocationName { get; set; } = string.Empty;
        public string LocationRegion { get; set; } = string.Empty;
        public string LocationCountry { get; set; } = string.Empty;

        // Trenutno stanje
        public string CurrentLastUpdated { get; set; } = string.Empty;
        public double CurrentTempC { get; set; }
        public double CurrentFeelsLikeC { get; set; }
        public string CurrentConditionText { get; set; } = string.Empty;
        public double CurrentWindSpeedKph { get; set; } 
        public double CurrentPressureMb { get; set; }

        // Prognoza
        public List<ForecastDay> ForecastDays { get; set; } = new List<ForecastDay>();
    }
}
