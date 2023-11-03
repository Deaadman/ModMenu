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

            UIPanel uiPanel = modMenuGO.GetComponent<UIPanel>();
            if (uiPanel == null)
            {
                uiPanel = modMenuGO.AddComponent<UIPanel>();
                uiPanel.depth = 60;
            }

            Panel_ModMenu modMenu = modMenuGO.GetComponent<Panel_ModMenu>();
            if (modMenu == null)
            {
                modMenu = modMenuGO.AddComponent<Panel_ModMenu>();

                modMenu.m_Panel = uiPanel;

                modMenu.Initialize();
                modMenu.Enable(false);
            }
        }
    }
}