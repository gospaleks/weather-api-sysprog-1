using weather_api_sysprog_1.Configuration;
using weather_api_sysprog_1.Infrastructure;
using weather_api_sysprog_1.Infrastructure.Cache;
using weather_api_sysprog_1.Web;

var settings = new AppSettings();
var cache = new CacheManager(settings);
var weatherService = new WeatherServiceImpl(settings);

var server = new WebServer(settings, weatherService, cache);
server.Start();