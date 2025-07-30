using System.Text.Json;

namespace weather_api_sysprog_1.Infrastructure.Extensions
{
    public static class JsonExtensions
    {
        public static string GetStringSafe(this JsonElement element, string propertyName) =>
            element.TryGetProperty(propertyName, out var prop) ? prop.GetString() ?? string.Empty : string.Empty;

        public static double GetDoubleSafe(this JsonElement element, string propertyName) =>
            element.TryGetProperty(propertyName, out var prop) && prop.TryGetDouble(out var value) ? value : 0;

        public static int GetIntSafe(this JsonElement element, string propertyName) =>
            element.TryGetProperty(propertyName, out var prop) && prop.TryGetInt32(out var value) ? value : 0;

        public static bool GetBoolFromInt(this JsonElement element, string propertyName) =>
            element.GetIntSafe(propertyName) != 0;
    
        public static DateTime GetDateTime(this JsonElement element, string propertyName) =>
            element.TryGetProperty(propertyName, out var prop) && prop.TryGetDateTime(out var value) ? value : DateTime.MinValue;

        public static DateOnly GetDateOnly(this JsonElement element, string propertyName) =>
            DateOnly.FromDateTime(element.GetDateTime(propertyName));
    }
}
