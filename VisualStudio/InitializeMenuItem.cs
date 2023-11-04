namespace ModMenu;

internal class InitializeMenuItem
{
    private const int MOD_MENU_ID = 0x4d4d;

    [HarmonyPatch(typeof(Panel_Sandbox), nameof(Panel_Sandbox.ConfigureMenu))]
    private static class AddModMenuItem
    {
        private static void Postfix(Panel_Sandbox __instance)
        {
            BasicMenu basicMenu = __instance.m_BasicMenu;
            if (basicMenu == null) return;

            AddAnotherMenuItem(basicMenu);
            BasicMenu.BasicMenuItemModel firstItem = basicMenu.m_ItemModelList[0];

            int desiredIndex = basicMenu.m_ItemModelList.Count - 2;
            desiredIndex = Math.Max(0, desiredIndex);

            Color defaultHighlightTint = Color.white;

            BasicMenu.BasicMenuItemModel newItem = new("", MOD_MENU_ID, desiredIndex, Localization.Get("GAMEPLAY_Mods"), Localization.Get("GAMEPLAY_ModMenuDescription"), null, new Action(() => ShowModMenu(__instance)),firstItem.m_NormalTint,defaultHighlightTint);

            basicMenu.m_ItemModelList.Insert(desiredIndex, newItem);
        }

        private static void ShowModMenu(Panel_Sandbox __instance)
        {
            GameAudioManager.PlayGUIButtonClick();

            Transform modMenuTransform = __instance.gameObject.transform.Find("Panel_ModMenu");
            if (modMenuTransform == null) return;

            Panel_ModMenu modMenu = modMenuTransform.GetComponent<Panel_ModMenu>();

            __instance.enabled = false;

            Transform parentTransform = __instance.gameObject.transform;
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform child = parentTransform.GetChild(i);
                if (child.name != "Panel_ModMenu")
                {
                    child.gameObject.SetActive(false);
                }
            }

            modMenu?.Enable(true);
        }

        private static void AddAnotherMenuItem(BasicMenu basicMenu)
        {
            GameObject gameObject = NGUITools.AddChild(basicMenu.m_MenuGrid.gameObject, basicMenu.m_BasicMenuItemPrefab);
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