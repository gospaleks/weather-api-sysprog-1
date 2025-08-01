using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using weather_api_sysprog_1.Configuration;
using weather_api_sysprog_1.Core;
using weather_api_sysprog_1.Core.Interfaces;
using weather_api_sysprog_1.Infrastructure.Cache;
using weather_api_sysprog_1.Infrastructure.Logging;

namespace weather_api_sysprog_1.Web
{
    public class WebServer
    {
        private readonly HttpListener _listener = new();

        private readonly CacheManager _cache;
        private readonly IWeatherService _weatherService;
        private readonly AppSettings _settings;

        public WebServer(AppSettings settings, IWeatherService weatherService, CacheManager cache)
        {
            _settings = settings;
            _weatherService = weatherService;
            _cache = cache;
            _listener.Prefixes.Add(_settings.GetListenerPrefix());
        }

        public void Start()
        {
            _listener.Start();
            Logger.Log($"Web server pokrenut na {_settings.GetListenerPrefix()}");

            while (true)
            {
                var context = _listener.GetContext();
                ThreadPool.QueueUserWorkItem(HandleRequest, context);
            }
        }
      
        private void HandleRequest(object? state)
        {
            var context = (HttpListenerContext)state!;
            var request = context.Request;
            
            // Preskoci favicon.ico da ne bi logovao bespotrebno gresku
            string absolutePath = context.Request.Url?.AbsolutePath ?? string.Empty;
            if (absolutePath.Equals("/favicon.ico", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 404;
                context.Response.Close();
                return;
            }

            // Extract query parameters into a dictionary
            string? city = request.QueryString["q"];
            string? days = request.QueryString["days"];
            string? aqi = request.QueryString["aqi"];
            string? alerts = request.QueryString["alerts"];

            // Kreiraj unique key kombinovanjem query parametara
            string customQuery = $"&q={city ?? ""}&days={days ?? ""}&aqi={aqi ?? ""}&alerts={alerts ?? ""}";

            if (string.IsNullOrEmpty(customQuery))
            {
                context.Response.StatusCode = 400;
                RespondWithText(context, "Greška: Niste prosledili upit.");
                Logger.Log("Zahtev bez query string-a.");
                return;
            }

            if (_cache.TryGet(customQuery, out var cached))
            {
                Logger.Log("Slanje keširanog odgovora.");
                RespondWithJson(context, cached);
                return;
            }

            try
            {
                var forecast = _weatherService.GetWeather(customQuery);
                _cache.Add(customQuery, forecast);

                Logger.Log("Novi odgovor dobijen i keširan.");
                RespondWithJson(context, forecast);
            }
            catch (WeatherApiException ex)
            {
                Logger.Log($"Greška: {ex.ApiMessage ?? ex.Message}");
                context.Response.StatusCode = ex.StatusCode != 0 ? ex.StatusCode : 500; // 500 Internal server error

                RespondWithText(context, ex.ApiMessage ?? ex.Message);
            }
        }

        private void RespondWithJson(HttpListenerContext context, object? content)
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };

            byte[] buffer = JsonSerializer.SerializeToUtf8Bytes(content, options);
            context.Response.ContentType = "application/json";
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }

        private void RespondWithText(HttpListenerContext context, string content)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
            context.Response.ContentType = "text/plain";
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.OutputStream.Close();
        }
    }
}
