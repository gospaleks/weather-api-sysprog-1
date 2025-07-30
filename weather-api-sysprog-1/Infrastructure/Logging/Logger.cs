namespace weather_api_sysprog_1.Infrastructure.Logging
{
    public static class Logger
    {
        private static readonly object _logLock = new();

        public static void Log(string message)
        {
            lock (_logLock)
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
            }
        }
    }
}
