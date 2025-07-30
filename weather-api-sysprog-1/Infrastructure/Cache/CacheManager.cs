using weather_api_sysprog_1.Configuration;
using weather_api_sysprog_1.Core.Entities;
using weather_api_sysprog_1.Infrastructure.Logging;

namespace weather_api_sysprog_1.Infrastructure.Cache
{
    public class CacheManager
    {
        private readonly AppSettings _settings;

        private readonly Dictionary<string, WeatherForecast> _cache = new();
        private static LinkedList<string> accessOrder = new LinkedList<string>();

        // Sinhronizacioni mehanizam
        private readonly object _lock = new();

        public CacheManager(AppSettings appSettings)
        {
            _settings = appSettings;
        }

        public bool TryGet(string key, out WeatherForecast? value)
        {
            lock (_lock)
            {
                bool isInCache = _cache.TryGetValue(key, out value);
                
                // Ako je u kesu postavi ga na kraj liste pristupa jer je najskorije koriscen
                if (isInCache)
                {
                    accessOrder.Remove(key);
                    accessOrder.AddLast(key);
                }

                return isInCache;
            }
        }

        public void Add(string key, WeatherForecast value)
        {
            lock (_lock)
            {
                if (_cache.Count >= _settings.MaxCacheSize)
                {
                    RemoveLRU();
                }

                if (_cache.TryAdd(key, value))
                {
                    accessOrder.AddLast(key);
                }
            }
        }

        private void RemoveLRU()
        {
            try
            {
                if (accessOrder.First == null)
                    return;

                // Izbaci least recently used (LRU) stavku iz kesa
                string lruKey = accessOrder.First.Value;
                accessOrder.RemoveFirst();
                _cache.Remove(lruKey);
            }
            catch (Exception e)
            {
                Logger.Log($"Greška prilikom uklanjanja LRU stavke iz keša: {e.Message}");
            }

        }
    }
}
