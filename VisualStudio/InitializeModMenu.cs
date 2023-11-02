namespace ModMenu;

internal class InitializeModMenu
{
    [HarmonyPatch(typeof(Panel_Sandbox), nameof(Panel_Sandbox.Initialize))]
    private static class InitializeModMenuPanel
    {
        private static void Postfix(Panel_Sandbox __instance)
        {
            GameObject modMenuGO = new("Panel_ModMenu");
            modMenuGO.transform.SetParent(__instance.gameObject.transform, false);

            Panel_ModMenu modMenu = modMenuGO.GetComponent<Panel_ModMenu>();
            if (modMenu == null)
            {
                modMenu = modMenuGO.AddComponent<Panel_ModMenu>();
                modMenu.Initialize();
                modMenu.Enable(false);
            }
        }
    }
}