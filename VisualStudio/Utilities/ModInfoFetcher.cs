//using Newtonsoft.Json.Linq;

//namespace ModMenu.Utilities;

//internal class ModInfoFetcher
//{
//    private const string ModInfoApiUrl = "https://tld.xpazeapps.com/api.php?pp";
//    private readonly HttpClient _httpClient;

//    public ModInfoFetcher()
//    {
//        _httpClient = new HttpClient();
//    }

//    public async Task<Dictionary<string, string>> FetchModVersionsAsync()
//    {
//        var modVersions = new Dictionary<string, string>();
//        try
//        {
//            string jsonResponse = await _httpClient.GetStringAsync(ModInfoApiUrl);
//            var modsData = JObject.Parse(jsonResponse);

//            foreach (var mod in modsData)
//            {
//                string modName = mod.Key;
//                string version = mod.Value["Version"]?.ToString();

//                if (!string.IsNullOrEmpty(modName) && !string.IsNullOrEmpty(version))
//                {
//                    modVersions.Add(modName, version);
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            // Handle any errors that occur during the API call.
//            // This could be logging the exception or throwing it to be handled elsewhere.
//            Logging.LogError($"Error fetching mod versions: {ex.Message}");
//        }

//        return modVersions;
//    }
//}