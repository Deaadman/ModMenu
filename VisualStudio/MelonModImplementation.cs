namespace ModMenu;

internal sealed class MelonModInitializer : MelonMod
{
    [HarmonyPatch(typeof(Panel_Sandbox), nameof(Panel_Sandbox.Initialize))]
    private static class InitializeModMenu
    {
        private static void Postfix(Panel_Sandbox __instance)
        {
            GameObject modMenuGO = new("Panel_ModMenu");
            modMenuGO.transform.SetParent(__instance.gameObject.transform, false);

            Panel_ModMenu modMenu = modMenuGO.GetComponent<Panel_ModMenu>();
            if (modMenu == null)
            {
                modMenu = modMenuGO.AddComponent<Panel_ModMenu>();

                GameObject basicMenuRootGO = new("BasicMenuRoot");
                basicMenuRootGO.transform.SetParent(modMenuGO.transform, false);
                basicMenuRootGO.transform.localPosition = new Vector3(-473.2906f, 48, 0);

                modMenu.m_BasicMenuRoot = basicMenuRootGO;

                modMenu.Initialize();
                modMenu.Enable(false);
            }
        }
    }
}