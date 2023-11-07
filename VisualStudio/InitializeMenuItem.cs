namespace ModMenu;

internal static class InitializeMenuItem
{
    private const int ModMenuId = 0x4d4d;

    [HarmonyPatch(typeof(Panel_Sandbox), nameof(Panel_Sandbox.ConfigureMenu))]
    private static class AddModMenuItem
    {
        private static void Postfix(Panel_Sandbox __instance)
        {
            BasicMenu basicMenu = __instance.m_BasicMenu;
            if (basicMenu == null) return;

            AddAnotherMenuItem(basicMenu);
            BasicMenu.BasicMenuItemModel firstItem = basicMenu.m_ItemModelList[0];

            int desiredIndex = Math.Max(0, basicMenu.m_ItemModelList.Count - 2);
            Color defaultHighlightTint = Color.white;

            Action onClickAction = new(() => ShowModMenu(__instance));

            BasicMenu.BasicMenuItemModel newItem = new(
                id: "",
                value: ModMenuId,
                itemIndex: desiredIndex,
                labelText: Localization.Get("GAMEPLAY_Mods"),
                descriptionText: Localization.Get("GAMEPLAY_ModMenuDescription"),
                secondaryText: null,
                onClickAction: onClickAction,
                tintNormal: firstItem.m_NormalTint,
                tintHighlight: defaultHighlightTint
            );

            basicMenu.m_ItemModelList.Insert(desiredIndex, newItem);
        }

        private static void ShowModMenu(Panel_Sandbox __instance)
        {
            GameAudioManager.PlayGUIButtonClick();

            Transform modMenuTransform = __instance.gameObject.transform.Find("Panel_ModMenu");
            if (modMenuTransform == null) return;

            Panel_ModMenu modMenu = modMenuTransform.GetComponent<Panel_ModMenu>();
            __instance.enabled = false;

            for (int i = 0; i < __instance.gameObject.transform.childCount; i++)
            {
                Transform child = __instance.gameObject.transform.GetChild(i);
                if (child.name != "Panel_ModMenu")
                {
                    child.gameObject.SetActive(false);
                }
            }

            modMenu?.Enable(true);
        }

        private static void AddAnotherMenuItem(BasicMenu basicMenu)
        {
            GameObject menuItem = NGUITools.AddChild(basicMenu.m_MenuGrid.gameObject, basicMenu.m_BasicMenuItemPrefab);
            BasicMenuItem itemComponent = menuItem.GetComponent<BasicMenuItem>();
            BasicMenu.BasicMenuItemView itemView = itemComponent.m_View;
            int itemIndex = basicMenu.m_MenuItems.Count;

            EventDelegate.Callback onClickCallback = new Action(() => basicMenu.OnItemClicked(itemIndex));
            itemView.m_Button.onClick.Add(new EventDelegate(onClickCallback));

            EventDelegate.Callback onDoubleClickCallback = new Action(() => basicMenu.OnItemDoubleClicked(itemIndex));
            itemView.m_DoubleClickButton.m_OnDoubleClick.Add(new EventDelegate(onDoubleClickCallback));

            basicMenu.m_MenuItems.Add(itemView);
        }
    }
}