using ModMenu.Utilities;

namespace ModMenu;

internal class ModDetailsFetcher
{
    internal static List<(string ModType, string ModName, string ModDescription, string ModVersion, string ModAuthor, string ModLoaderVersion, string modVersionCached)> GetLoadedMods()
    {
        var installedMods = new List<(string ModType, string ModName, string ModDescription, string ModVersion, string ModAuthor, string ModLoaderVersion, string modVersionCached)>();
        var allMelons = MelonMod.RegisteredMelons.Cast<MelonBase>().Concat(MelonPlugin.RegisteredMelons.Cast<MelonBase>());
        var cachedModVersions = CacheManager.GetCachedModVersions();

        foreach (var melon in allMelons)
        {
            var modType = melon is MelonMod ? "Mod" : "Plugin";
            var modName = melon.Info.Name.Replace(" ", "");
            var modDescription = GetAttributeValue<AssemblyDescriptionAttribute>(melon.GetType().Assembly, attr => attr.Description) ?? string.Empty;
            var modVersion = GetAttributeValue<AssemblyFileVersionAttribute>(melon.GetType().Assembly, attr => attr.Version) ?? string.Empty;
            var modAuthor = melon.Info.Author ?? string.Empty;
            var modLoaderVersion = GetModLoaderVersion(melon.GetType().Assembly) ?? string.Empty;
            var modVersionCached = cachedModVersions != null && cachedModVersions.TryGetValue(modName, out var version) ? version : string.Empty;

            installedMods.Add((
                ModType: modType,
                ModName: modName,
                ModDescription: modDescription,
                ModVersion: modVersion,
                ModAuthor: modAuthor,
                ModLoaderVersion: modLoaderVersion,
                modVersionCached
            ));
        }
        return installedMods;
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

    //internal static void LogVersionDifferences()
    //{
    //    var installedMods = GetLoadedMods();
    //    bool foundDifferences = false;

    //    foreach (var mod in installedMods)
    //    {
    //        if (NormalizeVersion(mod.ModVersion) != NormalizeVersion(mod.modVersionCached))
    //        {
    //            foundDifferences = true;
    //            Logging.Log($"Version difference detected for mod '{mod.ModName}': Installed version ({mod.ModVersion}) does not match Cached version ({mod.modVersionCached}).");
    //        }
    //    }

    //    if (!foundDifferences)
    //    {
    //        Logging.Log("No version differences found among installed mods.");
    //    }
    //}

    //// Normalizes version by adding trailing zeros if necessary
    //private static string NormalizeVersion(string version)
    //{
    //    // Assuming the version string is in the format "v1.2.3" or "v1.2.3.0"
    //    version = version.Replace("v", "").Trim();

    //    // Split the version into components
    //    var versionComponents = version.Split('.');
    //    // Fill the missing components with "0"
    //    while (versionComponents.Length < 4)
    //    {
    //        version = version + ".0";
    //        versionComponents = version.Split('.');
    //    }

    //    return version;
    //}
}