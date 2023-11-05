namespace ModMenu;

internal class ModConfigManager
{
    internal static List<ModConfig> CreateModConfigs()
    {
        List<ModConfig> modConfigs = new()
        {
            Capacity = 500
        };

        var modDetails = ModDetailsFetcher.GetLoadedMods();
        foreach (var (ModType, ModName, ModDescription, ModVersion, ModAuthor, ModLoaderVersion, modVersionCached) in modDetails)
        {
            ModConfig config = new()
            {
                m_ModType = ModType,
                m_ModName = ModName,
                m_ModDescription = ModDescription,
                m_ModVersion = ModVersion,
                m_ModAuthor = ModAuthor,
                m_ModVersionMelonLoader = ModLoaderVersion,
                m_ModVersionCached = modVersionCached
            };

            modConfigs.Add(config);
        }

        return modConfigs;
    }
}