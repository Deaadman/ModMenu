namespace ModMenu;

internal class InitializeComponents
{
    private static GameObject? m_ClonedModMenuInfo;

    [HarmonyPatch(typeof(Panel_Sandbox), nameof(Panel_Sandbox.Initialize))]
    private static class InitializeModMenuPanel
    {
        private static void Postfix(Panel_Sandbox __instance)
        {
            var ModMenuGameObject = new GameObject("Panel_ModMenu");
            ModMenuGameObject.transform.SetParent(__instance.transform, false);
            ModMenuGameObject.layer = vp_Layer.UI;

            var UIPanelComponent = ModMenuGameObject.GetComponent<UIPanel>() ?? ModMenuGameObject.AddComponent<UIPanel>();
            UIPanelComponent.depth = 60;

            var ModMenuComponent = ModMenuGameObject.GetComponent<Panel_ModMenu>() ?? ModMenuGameObject.AddComponent<Panel_ModMenu>();
            ModMenuComponent.m_Panel = UIPanelComponent;
            ModMenuComponent.Initialize();
            ModMenuComponent.Enable(false);

            if (m_ClonedModMenuInfo != null)
            {
                var clone = UnityEngine.Object.Instantiate(m_ClonedModMenuInfo, __instance.transform, false);
                clone.name = "ModMenuInformation";
                UpdateComponents.DestroyChildByName(clone.transform, "Anyfeedback");
            }
        }
    }

    [HarmonyPatch(typeof(Panel_MainMenu), nameof(Panel_MainMenu.Initialize))]
    private static class PatchPanelMainMenuInitialize
    {
        private static void Postfix(Panel_MainMenu __instance)
        {
            var MainWindow = __instance.m_MainWindow?.transform;
            if (MainWindow == null) return;

            var CopyrightGameObject = MainWindow.Find("copyright");
            if (CopyrightGameObject == null) return;

            if (CopyrightGameObject.GetComponent<UIAnchor>() is UIAnchor anchor)
            {
                anchor.enabled = false;
            }

            CopyrightGameObject.localPosition = new Vector3(-564, -330, 0);

            var CopyrightClone = UnityEngine.Object.Instantiate(CopyrightGameObject, MainWindow);
            m_ClonedModMenuInfo = CopyrightClone.gameObject;
            CopyrightClone.localPosition = new Vector3(-564, -360, 0);
            CopyrightClone.name = "ModMenuInformation";

            UpdateComponents.DestroyChildByName(CopyrightClone, "Anyfeedback");
            UpdateComponents.UpdateModMenuInformation(CopyrightClone);
        }
    }
}