namespace weather_api_sysprog_1.Infrastructure.Cache
{
    public class CacheManager
    {
        private readonly Dictionary<string, string> _cache = new();
        private readonly object _lock = new();

        public bool TryGet(string key, out string? value)
        {
            lock (_lock)
            {
                return _cache.TryGetValue(key, out value);
            }
        }

        public void Add(string key, string value)
        {
            lock (_lock)
            {
                _cache.TryAdd(key, value);
            }
        }
    }
}
