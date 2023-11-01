namespace ModMenu.Utilities;

internal class ModFetcher
{
    internal static List<string> GetInstalledMods()
    {
        List<string> installedMods = new();
        Logging.Log("Fetching installed mods...");

        foreach (var melon in MelonMod.RegisteredMelons)
        {
            installedMods.Add(melon.Info.Name);
            Logging.Log($"Added mod: {melon.Info.Name}");
        }

        Logging.Log($"Total installed mods found: {installedMods.Count}");

        return installedMods;
    }
}