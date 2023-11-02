using System.Text.Json;

namespace ModMenu.Utilities;

internal class ModInfoFetcherAPI : IDisposable
{
    private const string ModInfoApiUrl = "https://tld.xpazeapps.com/api.php?pp";
    private static readonly HttpClient _httpClient = new();

    public static async Task<Dictionary<string, string>> FetchModVersionsAsync()
    {
        var cachedModVersions = CacheManager.GetCachedModVersions();
        if (cachedModVersions != null && !CacheManager.IsCacheExpired())
        {
            Logging.Log("Using cached mod versions.");
            return cachedModVersions;
        }

        Logging.Log("Fetching new mod versions from API.");
        var modVersions = new Dictionary<string, string>();
        try
        {
            string jsonResponse = await _httpClient.GetStringAsync(ModInfoApiUrl);
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            JsonElement root = doc.RootElement;

            foreach (JsonProperty mod in root.EnumerateObject())
            {
                string modName = mod.Name;
                if (mod.Value.TryGetProperty("Version", out JsonElement versionElement))
                {
                    string version = versionElement.GetString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(version))
                    {
                        modVersions[modName] = version;
                        ConsoleColor logColor = ConsoleColor.White;
                        if (cachedModVersions == null || !cachedModVersions.TryGetValue(modName, out string? cachedVersion) || cachedVersion != version)
                        {
                            logColor = ConsoleColor.Green;
                        }

                        LogModVersion(modName, version, logColor);
                    }
                }
            }

            CacheManager.UpdateCache(modVersions);
        }
        catch (HttpRequestException httpEx)
        {
            Logging.LogError($"HTTP error fetching mod versions: {httpEx.Message}");
        }
        catch (JsonException jsonEx)
        {
            Logging.LogError($"JSON error parsing mod versions: {jsonEx.Message}");
        }
        catch (Exception ex)
        {
            Logging.LogError($"General error fetching mod versions: {ex.Message}");
        }

        return modVersions;
    }

    private static void LogModVersion(string modName, string version, ConsoleColor color)
    {
        Logging.LogColor(color, $"Mod: {modName}, Version: {version}");
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}