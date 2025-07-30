using System.Net;
using weather_api_sysprog_1.Configuration;
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

            string query = context.Request.Url?.Query ?? string.Empty;

            if (string.IsNullOrEmpty(query))
            {
                RespondWithText(context, "Greška: Niste prosledili upit.");
                Logger.Log("Zahtev bez query string-a.");
                return;
            }

            if (_cache.TryGet(query, out var cached))
            {
                Logger.Log("Slanje keširanog odgovora.");
                RespondWithJson(context, cached);
                return;
            }

            try
            {
                var response = _weatherService.GetWeather(query);
                _cache.Add(query, response);
                Logger.Log("Novi odgovor dobijen i keširan.");
                RespondWithJson(context, response);
            }
            catch (Exception ex)
            {
                Logger.Log("Greška: " + ex.Message);
                RespondWithText(context, "Došlo je do greške prilikom obrade zahteva.");
            }
        }

        private void RespondWithJson(HttpListenerContext context, string content)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(content);
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
