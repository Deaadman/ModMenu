using ModMenu.Utilities;

namespace ModMenu;

internal class ModDetailsFetcher
{
    internal static List<ModDetails> GetLoadedMods()
    {
        List<ModDetails> installedMods = new();
        IEnumerable<MelonBase> allMelons = MelonMod.RegisteredMelons.Cast<MelonBase>()
                               .Concat(MelonPlugin.RegisteredMelons.Cast<MelonBase>());
        Dictionary<string, string> VersionsAPI = CacheManager.GetVersionsAPI();

        foreach (MelonBase melon in allMelons)
        {
            string Type = melon is MelonMod ? "Mod" : "Plugin";
            string Name = melon.Info.Name.Replace(" ", "");
            string Description = GetAttributeValue<AssemblyDescriptionAttribute>(melon.GetType().Assembly, attr => attr.Description) ?? string.Empty;
            string Author = melon.Info.Author ?? string.Empty;
            string Version = GetAttributeValue<AssemblyFileVersionAttribute>(melon.GetType().Assembly, attr => attr.Version) ?? string.Empty;
            string VersionAPI = VersionsAPI.TryGetValue(Name, out string? version) ? version : string.Empty;
            string VersionML = GetModLoaderVersion(melon.GetType().Assembly) ?? string.Empty;

            installedMods.Add(new ModDetails
            {
                Type = Type,
                Name = Name,
                Description = Description,
                Author = Author,
                Version = Version,
                VersionAPI = VersionAPI,
                VersionML = VersionML
            });
        }

        return installedMods;
    }

    internal struct ModDetails
    {
        public string Type;
        public string Name;
        public string Description;
        public string Author;
        public string Version;
        public string VersionAPI;
        public string VersionML;
    }

    private static string GetModLoaderVersion(Assembly assembly)
    {
        var verifyLoaderVersionAttribute = assembly.GetCustomAttributes(typeof(VerifyLoaderVersionAttribute), false)
                                                   .OfType<VerifyLoaderVersionAttribute>().FirstOrDefault();
        if (verifyLoaderVersionAttribute != null)
        {
            return $"{verifyLoaderVersionAttribute.SemVer.Major}.{verifyLoaderVersionAttribute.SemVer.Minor}.{verifyLoaderVersionAttribute.SemVer.Patch}";
        }
        return string.Empty;
    }

    private static string GetAttributeValue<TAttr>(Assembly assembly, Func<TAttr, string> valueSelector) where TAttr : Attribute
    {
        var attribute = assembly.GetCustomAttributes(typeof(TAttr), false).OfType<TAttr>().FirstOrDefault();
        return attribute != null ? valueSelector(attribute) : string.Empty;
    }

    // Below is testing for a version higher / lower detection system

    internal static void LogVersionDifferences()
    {
        var installedMods = GetLoadedMods();
        bool foundDifferences = false;

        foreach (var mod in installedMods)
        {
            if (NormalizeVersion(mod.Version) != NormalizeVersion(mod.VersionAPI))
            {
                foundDifferences = true;
                Logging.Log($"Version difference detected for mod '{mod.Name}': Installed version ({mod.Version}) does not match API version ({mod.VersionAPI}).");
            }
        }

        if (!foundDifferences)
        {
            Logging.Log("No version differences found among installed mods.");
        }
    }

    // Normalizes version by adding trailing zeros if necessary
    private static string NormalizeVersion(string version)
    {
        // Assuming the version string is in the format "v1.2.3" or "v1.2.3.0"
        version = version.Replace("v", "").Trim();

        // Split the version into components
        var versionComponents = version.Split('.');
        // Fill the missing components with "0"
        while (versionComponents.Length < 4)
        {
            version += ".0";
            versionComponents = version.Split('.');
        }

        return version;
    }
}