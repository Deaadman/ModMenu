namespace ModMenu;

internal class ModConfigManager
{
    internal static List<ModConfig> CreateModConfigs()
    {
        List<ModConfig> modConfigs = new(capacity: 500);

        IEnumerable<ModDetailsFetcher.ModDetails> modDetails = ModDetailsFetcher.GetLoadedMods();

        foreach (ModDetailsFetcher.ModDetails detail in modDetails)
        {
            ModConfig config = new()
            {
                Type = detail.Type,
                Name = detail.Name,
                Description = detail.Description,
                Author = detail.Author,
                Version = detail.Version,
                VersionAPI = detail.VersionAPI,
                VersionML = detail.VersionML
            };

            modConfigs.Add(config);
        }

        return modConfigs;
    }
}