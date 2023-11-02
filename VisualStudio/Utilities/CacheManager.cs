using System.Text.Json;

namespace ModMenu.Utilities;

internal static class CacheManager
{
    private static readonly string cacheFilePath = "APICachedMods.json";

    internal static Dictionary<string, string> GetCachedModVersions()
    {
        if (!File.Exists(cacheFilePath))
        {
            Logging.Log("Cache file not found. Will fetch from API.");
            return new Dictionary<string, string>();
        }

        string json = File.ReadAllText(cacheFilePath);
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
    }

    internal static void UpdateCache(Dictionary<string, string> updates)
    {
        var currentCache = GetCachedModVersions();
        bool cacheUpdated = false;
        foreach (var update in updates)
        {
            if (!currentCache.TryGetValue(update.Key, out string? currentVersion) || currentVersion != update.Value)
            {
                currentCache[update.Key] = update.Value;
                cacheUpdated = true;
            }
        }

        if (cacheUpdated)
        {
            string json = JsonSerializer.Serialize(currentCache, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(cacheFilePath, json);
            Logging.Log("Cache updated with new mod versions.");
        }
    }
}