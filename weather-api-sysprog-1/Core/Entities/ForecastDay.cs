namespace weather_api_sysprog_1.Core.Entities
{
    public class ForecastDay
    {
        public DateOnly Date { get; set; }
        public double MaxtempC { get; set; }
        public double MintempC { get; set; }
        public double AvgtempC { get; set; }
        public double MaxwindKph { get; set; }
        public int Avghumidity { get; set; }
        public bool DailyWillItRain { get; set; }
        public int DailyChanceOfRain { get; set; }
        public bool DailyWillItSnow { get; set; }
        public int DailyChanceOfSnow { get; set; }
        public double Uv { get; set; }
        public string Text { get; set; } = string.Empty;

        // Kvalitet vazduha
        public AirQuality? AirQuality { get; set; }
    }
}