using System.Text.Json;

namespace ModMenu.Utilities;

internal class ModVersionChecker : IDisposable
{
    private const string ModInfoApiUrl = "https://tld.xpazeapps.com/api.php?pp";
    private static readonly HttpClient _httpClient = new();

    internal static async Task<Dictionary<string, string>> FetchModVersionsAsync()
    {
        var cachedModVersions = CacheManager.GetCachedModVersions();
        var modVersionsToUpdate = new Dictionary<string, string>();

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
                        if (!cachedModVersions.TryGetValue(modName, out string? cachedVersion) || cachedVersion != version)
                        {
                            modVersionsToUpdate[modName] = version;
                            LogVersionChange(modName, cachedVersion, version);
                        }
                    }
                }
            }

            if (modVersionsToUpdate.Count > 0)
            {
                CacheManager.UpdateCache(modVersionsToUpdate);
            }
            else
            {
                Logging.Log("No mod version updates found. Cache not updated.");
            }
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

    private static void LogVersionChange(string modName, string? oldVersion, string newVersion)
    {
        ConsoleColor logColor = ConsoleColor.Green; // Updated versions are logged in green
        string oldVersionDisplay = string.IsNullOrEmpty(oldVersion) ? "None" : oldVersion;
        Logging.LogColor(logColor, $"Mod = {modName}, Version: {oldVersionDisplay} --> {newVersion}");
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}