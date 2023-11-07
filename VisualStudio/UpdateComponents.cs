namespace ModMenu;

internal class UpdateComponents
{
    internal static void DestroyChildByName(Transform parent, string name)
    {
        var child = parent.Find(name);
        if (child != null)
        {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    internal static void UpdateModMenuInformation(Transform copyrightClone)
    {
        var copyrightLabelTransform = copyrightClone.Find("Label_Copyright");
        if (copyrightLabelTransform != null)
        {
            copyrightLabelTransform.name = "ModMenuLabel";
            var newLabelClone = UnityEngine.Object.Instantiate(copyrightLabelTransform, copyrightClone);
            newLabelClone.name = "LoadedLabel";
            newLabelClone.localPosition = new Vector3(-6, 27.3f, 0);

            UpdateLabelWithModVersion(copyrightLabelTransform);
            UpdateLabelWithLoadedMods(newLabelClone);
        }
    }

    private static void UpdateLabelWithModVersion(Transform labelTransform)
    {
        var originalLabel = labelTransform.GetComponent<UILabel>();
        if (originalLabel is not null)
        {
            var modDetails = ModDetailsFetcher.GetLoadedMods()
                .FirstOrDefault(mod => mod.Name.Equals("ModMenu", StringComparison.OrdinalIgnoreCase));

            if (modDetails.Equals(default(ModDetailsFetcher.ModDetails)) == false)
            {
                string modVersion = modDetails.Version;
                if (!string.IsNullOrEmpty(modVersion))
                {
                    originalLabel.text = $"{Localization.Get("GAMEPLAY_ModMenu")} v{modVersion}";
                }
            }
        }
    }

    private static void UpdateLabelWithLoadedMods(Transform labelTransform)
    {
        if (labelTransform.GetComponent<UILabel>() is UILabel newLabel)
        {
            var loadedMods = ModDetailsFetcher.GetLoadedMods();
            int modsCount = loadedMods.Count(mod => mod.Type.Equals("Mod", StringComparison.OrdinalIgnoreCase));
            int pluginsCount = loadedMods.Count(mod => mod.Type.Equals("Plugin", StringComparison.OrdinalIgnoreCase));

            var modsText = $"{modsCount} {Localization.Get("GAMEPLAY_Mods")}";
            var pluginsText = GetPluginsText(pluginsCount);
            var displayText = pluginsCount > 0 ? $"{modsText} {pluginsText} {Localization.Get("GAMEPLAY_Loaded")}" : $"{modsText} {Localization.Get("GAMEPLAY_Loaded")}";

            newLabel.text = displayText;
        }
    }

    private static string GetPluginsText(int count)
    {
        return count switch
        {
            1 => $"{Localization.Get("GAMEPLAY_And")} {count} {Localization.Get("GAMEPLAY_Plugin")}",
            > 1 => $"{Localization.Get("GAMEPLAY_And")} {count} {Localization.Get("GAMEPLAY_Plugins")}",
            _ => ""
        };
    }
}