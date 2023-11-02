namespace ModMenu.Utilities;

internal class ModFetcher
{
    internal static List<(string ModType, string ModName, string Description, string Version, string Author, string LoaderVersion)> GetLoadedMods()
    {
        List<(string ModType, string ModName, string Description, string Version, string Author, string LoaderVersion)> installedMods = new();
        var allMelons = MelonMod.RegisteredMelons.Cast<MelonBase>().Concat(MelonPlugin.RegisteredMelons.Cast<MelonBase>());
        foreach (var melon in allMelons)
        {
            string modType = melon is MelonMod ? "Mod" : "Plugin";
            string modName = melon.Info.Name;
            string description = GetAssemblyDescription(modName) ?? "";
            string version = GetAssemblyFileVersion(modName) ?? "";
            string author = GetModAuthor(melon) ?? "";
            string loaderVersion = GetModLoaderVersion(melon.GetType().Assembly) ?? "";

            installedMods.Add((modType, modName, description, version, author, loaderVersion));
        }
        return installedMods;
    }

    private static string? GetModLoaderVersion(Assembly assembly)
    {
        var customAttributes = assembly.GetCustomAttributes(false);
        var verifyLoaderVersionAttribute = customAttributes.OfType<VerifyLoaderVersionAttribute>().FirstOrDefault();

        if (verifyLoaderVersionAttribute != null)
        {
            string loaderVersion = $"{verifyLoaderVersionAttribute.SemVer.Major}.{verifyLoaderVersionAttribute.SemVer.Minor}.{verifyLoaderVersionAttribute.SemVer.Patch}";
            return loaderVersion;
        }
        else
        {
            return null;
        }
    }

    private static string? GetAssemblyDescription(string modGuiName)
    {
        var assembly = FindModAssembly(modGuiName);
        if (assembly != null)
        {
            var descriptionAttribute = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                                              .OfType<AssemblyDescriptionAttribute>().FirstOrDefault();
            return descriptionAttribute?.Description;
        }
        return null;
    }

    private static string? GetModAuthor(MelonBase melon)
    {
        return melon?.Info?.Author;
    }

    private static string? GetAssemblyFileVersion(string modGuiName)
    {
        var assembly = FindModAssembly(modGuiName);
        if (assembly != null)
        {
            var fileVersionAttribute = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false)
                                               .OfType<AssemblyFileVersionAttribute>().FirstOrDefault();
            return fileVersionAttribute?.Version;
        }
        else
        {
            return null;
        }
    }

    private static Assembly? FindModAssembly(string modGuiName)
    {
        string assemblyNameSearch = modGuiName.Replace(" ", "");
        return AppDomain.CurrentDomain.GetAssemblies()
                                      .FirstOrDefault(a => a.GetName().Name.Equals(assemblyNameSearch, StringComparison.OrdinalIgnoreCase));
    }
}