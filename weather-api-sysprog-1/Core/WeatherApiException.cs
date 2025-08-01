namespace weather_api_sysprog_1.Core
{
    public class WeatherApiException : Exception
    {
        public int StatusCode { get; } // 200, 400, 404, 500 ...
        public string? ApiMessage { get; }

        public WeatherApiException(string message, int statusCode, string? apiMessage = null)
            : base(message)
        {
            StatusCode = statusCode;
            ApiMessage = apiMessage;
        }
    }
}
