using System.Text.Json;

namespace ModMenu.Utilities;

internal static class CacheManager
{
    private static readonly string cacheFilePath = "APICachedMods.json";
    private static readonly TimeSpan cacheDuration = TimeSpan.FromHours(1);

    public static Dictionary<string, string>? GetCachedModVersions()
    {
        if (!File.Exists(cacheFilePath))
        {
            Logging.Log("Cache file not found. Will fetch from API.");
            return null;
        }

        string json = File.ReadAllText(cacheFilePath);
        var cachedData = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        Logging.Log("Fetched mod versions from cache.");
        return cachedData;
    }

    public static void UpdateCache(Dictionary<string, string> modVersions)
    {
        string json = JsonSerializer.Serialize(modVersions);
        File.WriteAllText(cacheFilePath, json);
        Logging.Log("Cache updated with new mod versions.");
    }

    public static bool IsCacheExpired()
    {
        if (!File.Exists(cacheFilePath))
        {
            return true;
        }

        var lastWriteTime = File.GetLastWriteTime(cacheFilePath);
        bool expired = DateTime.Now - lastWriteTime > cacheDuration;
        Logging.Log(expired ? "Cache has expired. Need to fetch from API." : "Cache is still valid.");
        return expired;
    }
}