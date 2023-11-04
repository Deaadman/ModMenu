namespace ModMenu;

internal class InitializeComponents
{
    private static GameObject clonedCopyrightGameObject;

    [HarmonyPatch(typeof(Panel_Sandbox), nameof(Panel_Sandbox.Initialize))]
    private static class InitializeModMenuPanel
    {
        private static void Postfix(Panel_Sandbox __instance)
        {
            GameObject modMenuGO = new("Panel_ModMenu");
            modMenuGO.transform.SetParent(__instance.gameObject.transform, false);

            UIPanel uiPanel = modMenuGO.GetComponent<UIPanel>() ?? modMenuGO.AddComponent<UIPanel>();
            uiPanel.depth = 60;

            Panel_ModMenu modMenu = modMenuGO.GetComponent<Panel_ModMenu>() ?? modMenuGO.AddComponent<Panel_ModMenu>();
            modMenu.m_Panel = uiPanel;
            modMenu.Initialize();
            modMenu.Enable(false);

            if (clonedCopyrightGameObject != null)
            {
                GameObject sandboxCopyrightClone = UnityEngine.Object.Instantiate(clonedCopyrightGameObject);
                sandboxCopyrightClone.transform.SetParent(__instance.transform, false);
                sandboxCopyrightClone.name = "ModMenuInformation";

                Transform anyfeedbackCloneTransform = sandboxCopyrightClone.transform.Find("Anyfeedback");
                if (anyfeedbackCloneTransform != null) UnityEngine.Object.Destroy(anyfeedbackCloneTransform.gameObject);
            }
        }
    }

    [HarmonyPatch(typeof(Panel_MainMenu), nameof(Panel_MainMenu.Initialize))]
    private static class Patch_Panel_MainMenu_Initialize
    {
        private static void Postfix(Panel_MainMenu __instance)
        {
            Transform mainWindowTransform = __instance.m_MainWindow?.transform;
            if (mainWindowTransform == null) return;

            Transform copyrightTransform = mainWindowTransform.Find("copyright");
            if (copyrightTransform == null) return;

            UIAnchor anchor = copyrightTransform.GetComponent<UIAnchor>();
            if (anchor != null) anchor.enabled = false;

            copyrightTransform.localPosition = new Vector3(-564, -330, 0);

            Transform copyrightClone = UnityEngine.Object.Instantiate(copyrightTransform, mainWindowTransform);
            clonedCopyrightGameObject = copyrightClone.gameObject;
            copyrightClone.localPosition = new Vector3(-564, -360, 0);
            copyrightClone.name = "ModMenuInformation";

            Transform anyfeedbackCloneTransform = copyrightClone.Find("Anyfeedback");
            if (anyfeedbackCloneTransform != null) UnityEngine.Object.Destroy(anyfeedbackCloneTransform.gameObject);

            Transform copyrightLabelTransform = copyrightClone.Find("Label_Copyright");
            copyrightLabelTransform.name = "ModMenuLabel";

            if (copyrightLabelTransform != null)
            {
                Transform newLabelClone = UnityEngine.Object.Instantiate(copyrightLabelTransform, copyrightClone);
                newLabelClone.name = "LoadedLabel";
                newLabelClone.localPosition = new Vector3(-6, 27.3f, 0);

                UILabel originalLabel = copyrightLabelTransform.GetComponent<UILabel>();
                string modName = "ModMenu";
                var modVersion = ModDetailsFetcher.GetLoadedMods()
                    .FirstOrDefault(mod => mod.ModName.Equals(modName, StringComparison.OrdinalIgnoreCase))
                    .ModVersion;

                if (originalLabel != null && !string.IsNullOrEmpty(modVersion))
                {
                    originalLabel.text = Localization.Get("GAMEPLAY_ModMenu") + $" v{modVersion}";
                }

                UILabel newLabel = newLabelClone.GetComponent<UILabel>();
                var loadedMods = ModDetailsFetcher.GetLoadedMods();
                int modsCount = loadedMods.Count(mod => mod.ModType.Equals("Mod", StringComparison.OrdinalIgnoreCase));
                int pluginsCount = loadedMods.Count(mod => mod.ModType.Equals("Plugin", StringComparison.OrdinalIgnoreCase));

                string modsLocalized = Localization.Get("GAMEPLAY_Mods");
                string pluginsSingularLocalized = Localization.Get("GAMEPLAY_Plugin");
                string pluginsPluralLocalized = Localization.Get("GAMEPLAY_Plugins");
                string loadedLocalized = Localization.Get("GAMEPLAY_Loaded");

                string modsText = $"{modsCount} {modsLocalized}";

                string pluginsText = pluginsCount == 1 ?
                    $"{Localization.Get("GAMEPLAY_And")} {pluginsCount} {pluginsSingularLocalized}" :
                    pluginsCount > 1 ?
                    $"{Localization.Get("GAMEPLAY_And")} {pluginsCount} {pluginsPluralLocalized}" :
                    "";

                string displayText = pluginsCount > 0 ?
                    $"{modsText} {pluginsText} {loadedLocalized}" :
                    $"{modsText} {loadedLocalized}";

                if (newLabel != null)
                {
                    newLabel.text = displayText;
                }
            }
        }
    }
}