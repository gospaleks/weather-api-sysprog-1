namespace weather_api_sysprog_1.Core.Entities
{
    public class AirQuality
    {
        public double CO { get; set; }
        public double NO2 { get; set; }
        public double O3 { get; set; }
        public double SO2 { get; set; }
        public double PM2_5 { get; set; }
        public double PM10 { get; set; }
        public int USEPAIndex { get; set; }
        public int GBDEFRAIndex { get; set; }
    }
}