namespace ModMenu;

internal class Panel_ModMenuModMenuItem
{
    private const int MOD_MENU_ID = 0x4d4d; // "MM" in hex for "ModMenu"

    [HarmonyPatch(typeof(Panel_Sandbox), nameof(Panel_Sandbox.ConfigureMenu), new Type[0])]
    internal static class AddModMenuButton
    {
        private static void Postfix(Panel_Sandbox __instance)
        {
            BasicMenu basicMenu = __instance.m_BasicMenu;
            if (basicMenu == null)
                return;

            AddAnotherMenuItem(basicMenu); // We need one more than they have...
            BasicMenu.BasicMenuItemModel firstItem = basicMenu.m_ItemModelList[0];
            int itemIndex = basicMenu.GetItemCount();
            basicMenu.AddItem("Mods", MOD_MENU_ID, itemIndex, "Mods", "???", null,
                    new Action(() => ShowModMenu(__instance)), firstItem.m_NormalTint, firstItem.m_HighlightTint);
        }

        private static void ShowModMenu(Panel_Sandbox __instance)
        {
            GameAudioManager.PlayGUIButtonClick();

            // Get the Panel_ModMenu GameObject that is a child of Panel_Sandbox
            Transform modMenuTransform = __instance.gameObject.transform.Find("Panel_ModMenu");
            if (modMenuTransform == null) return;  // Exit if Panel_ModMenu GameObject is not found

            // Get the Panel_ModMenu component from the Panel_ModMenu GameObject
            Panel_ModMenu modMenu = modMenuTransform.GetComponent<Panel_ModMenu>();

            // Disable the Panel_Sandbox component
            __instance.enabled = false;

            // Disable all child GameObjects of Panel_Sandbox except Panel_ModMenu
            Transform parentTransform = __instance.gameObject.transform;
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform child = parentTransform.GetChild(i);
                if (child.name != "Panel_ModMenu")
                {
                    child.gameObject.SetActive(false);
                }
            }

            // Enable (or activate) your mod menu here
            modMenu?.Enable(true);
        }

        internal static void AddAnotherMenuItem(BasicMenu basicMenu)
        {
            GameObject gameObject = NGUITools.AddChild(basicMenu.m_MenuGrid.gameObject, basicMenu.m_BasicMenuItemPrefab);
            gameObject.name = "ModMenu MenuItem";
            BasicMenuItem item = gameObject.GetComponent<BasicMenuItem>();
            BasicMenu.BasicMenuItemView view = item.m_View;
            int itemIndex = basicMenu.m_MenuItems.Count;
            EventDelegate onClick = new(new Action(() => basicMenu.OnItemClicked(itemIndex)));
            view.m_Button.onClick.Add(onClick);
            EventDelegate onDoubleClick = new(new Action(() => basicMenu.OnItemDoubleClicked(itemIndex)));
            view.m_DoubleClickButton.m_OnDoubleClick.Add(onDoubleClick);
            basicMenu.m_MenuItems.Add(view);
        }
    }
}