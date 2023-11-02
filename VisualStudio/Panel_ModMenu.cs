using ModMenu.Utilities;
using static Il2Cpp.BasicMenu;

namespace ModMenu;

[RegisterTypeInIl2Cpp(false)]
public class Panel_ModMenu : MonoBehaviour
{
    public Panel_ModMenu(IntPtr intPtr) : base(intPtr) { }

    private BasicMenu m_BasicMenu;

    #region GameObjects
    private GameObject m_BasicMenuRoot;
    private GameObject m_ModsLoaded;
    private GameObject m_RightSide;
    #endregion

    private UISprite? m_BackgroundSprite;

    #region Components
    private UILabel m_ModsLoadedLabel;

    private UILabel m_ModsTitle;

    private UILabel m_ModsDescription;

    private UILabel m_ModsVersion;

    private UILabel m_AuthorName;

    private UILabel m_MelonVersion;
    #endregion

    #region Fields
    private string m_ModsLoadedLabelText;
    #endregion

    #region Lists
    private List<(string ModName, string Description)> m_LoadedModsList;
    private List<ModMenuItems> m_MenuItems;
    #endregion

    internal class ModMenuItems
    {
        internal string m_Type; // "Mod" or "Plugin"
        internal string m_LabelText;
        internal string m_LabelDescription;
        internal string m_LabelVersion;
        internal string m_LabelAuthor;
        internal string m_LabelLoaderVersion;
    }

    private void AddMenuItem(int itemIndex)
    {
        string modType = m_MenuItems[itemIndex].m_Type;

        string labelText = m_MenuItems[itemIndex].m_LabelText;
        //string labelDescription = m_MenuItems[itemIndex].m_LabelDescription;

        Action actionFromType = GetActionFromType(modType);

        m_BasicMenu.AddItem(modType, modType.GetHashCode(), itemIndex, Localization.Get(labelText), "", null, actionFromType, Color.clear, Color.clear);
    }

    private void ConfigureMenu()
    {
        if (m_BasicMenu == null)
        {
            return;
        }

        m_BasicMenu.Reset();
        m_BasicMenu.UpdateTitle("GAMEPLAY_ModMenu", "", Vector3.zero);

        var loadedMods = ModFetcher.GetLoadedMods();
        int modsCount = loadedMods.Count(m => m.ModType == "Mod");
        int pluginsCount = loadedMods.Count(m => m.ModType == "Plugin");

        string pluginTextKey = pluginsCount == 1 ? "GAMEPLAY_PluginLoaded" : "GAMEPLAY_PluginsLoaded";
        m_ModsLoadedLabelText = $"{modsCount} {Localization.Get("GAMEPLAY_ModsAnd")} {pluginsCount} {Localization.Get(pluginTextKey)}";

        if (m_ModsLoadedLabel != null)
        {
            m_ModsLoadedLabel.text = m_ModsLoadedLabelText;
        }

        for (int i = 0; i < m_MenuItems.Count; i++)
        {
            string modType = m_MenuItems[i].m_Type;
            Action actionFromType = () => OnSlotClicked(modType);

            AddMenuItem(i);
        }

        m_BasicMenu.SetBackAction(new Action(OnClickBack));
        m_BasicMenu.EnableConfirm(false, "GAMEPLAY_Select");
    }

    internal void Enable(bool enable)
    {
        if (enable)
        {
            ConfigureMenu();
            m_BasicMenu.Enable(true);
            GameManager.GetCameraEffects().DepthOfFieldTurnOn();
            m_ModsLoaded.SetActive(true);
            return;
        }
        m_BasicMenu.Enable(false);
        GameManager.GetCameraEffects().DepthOfFieldTurnOff(false);
        m_ModsLoaded.SetActive(false);
        m_RightSide.SetActive(false);
    }

    private Action GetActionFromType(string type)
    {
        return new Action(() => OnSlotClicked(type));
    }

    internal void Initialize()
    {
        InitializeGameObjects();
        InitializeModDescriptionPage();

        m_BasicMenu = InstantiateMenu(InterfaceManager.s_BasicMenuPrefab, m_BasicMenuRoot, gameObject, this);
        m_BasicMenu.m_CanScroll = true;
        m_MenuItems = new List<ModMenuItems>();

        var loadedMods = ModFetcher.GetLoadedMods();
        foreach (var (modType, modName, modDescription, modVersion, modAuthor, loaderVersion) in loadedMods)
        {
            string labelTextKey = $"GAMEPLAY_{modName.Replace(" ", "")}";
            string descriptionKey = $"GAMEPLAY_{modName.Replace(" ", "")}Description";

            string fallbackTitle = modName;
            string fallbackDescription = modDescription;

            string labelText = Localization.Get(labelTextKey);
            if (labelText == labelTextKey) labelText = fallbackTitle;

            string description = Localization.Get(descriptionKey);
            if (description == descriptionKey) description = fallbackDescription;

            m_MenuItems.Add(new ModMenuItems
            {
                m_Type = modName,
                m_LabelText = labelText,
                m_LabelDescription = description,
                m_LabelVersion = modVersion,
                m_LabelAuthor = modAuthor,
                m_LabelLoaderVersion = loaderVersion,
            });
        }
    }

    internal void InitializeGameObjects()
    {
        m_BasicMenuRoot = UserInterfaceUtilities.SetupGameObject("MenuRoot", transform, new Vector3(-473.2906f, 48, 0));

        m_ModsLoaded = UserInterfaceUtilities.SetupGameObject("ModsLoaded", transform, Vector3.zero);
        m_ModsLoaded.SetActive(false);

        GameObject ModsLoadedLabelGameObject = UserInterfaceUtilities.SetupGameObject("ModsLoadedLabel", m_ModsLoaded.transform, new Vector3(-505, 255, 0));
        ModsLoadedLabelGameObject.AddComponent<UILabel>();

        m_ModsLoadedLabel = ModsLoadedLabelGameObject.GetComponent<UILabel>();
        UserInterfaceUtilities.SetupLabel(m_ModsLoadedLabel, m_ModsLoadedLabelText, FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Center, UILabel.Overflow.ResizeFreely, true, 20, 15, new Color(0.4784314f, 0.4784314f, 0.4784314f), true);
    }

    internal void InitializeModDescriptionPage()
    {
        m_RightSide = UserInterfaceUtilities.SetupGameObject("Right Side", transform, new Vector3(0, 278, 0));
        m_RightSide.SetActive(false);



        GameObject BackgroundSprite = UserInterfaceUtilities.SetupGameObject("BackgroundSprite", m_RightSide.transform, new Vector3(0, -7, 0));
        BackgroundSprite.AddComponent<UISprite>();
        m_BackgroundSprite = BackgroundSprite.GetComponent<UISprite>();

        UserInterfaceUtilities.SetupUISprite(m_BackgroundSprite, "inv_statBack", new Color(0.4314f, 0.5137f, 0.4863f, 0.1569f), 76, 450);

        GameObject BackgroundVersionSprite = UserInterfaceUtilities.SetupGameObject("BackgroundVersionSprite", m_RightSide.transform, new Vector3(-173, -63, 0));
        BackgroundVersionSprite.AddComponent<UISprite>();
        m_BackgroundSprite = BackgroundVersionSprite.GetComponent<UISprite>();

        UserInterfaceUtilities.SetupUISprite(m_BackgroundSprite, "inv_statBack", new Color(0.4314f, 0.5137f, 0.4863f, 0.1569f), 35, 100);

        GameObject BackgroundAuthorSprite = UserInterfaceUtilities.SetupGameObject("BackgroundAuthorSprite", m_RightSide.transform, new Vector3(0, -63, 0));
        BackgroundAuthorSprite.AddComponent<UISprite>();
        m_BackgroundSprite = BackgroundAuthorSprite.GetComponent<UISprite>();

        UserInterfaceUtilities.SetupUISprite(m_BackgroundSprite, "inv_statBack", new Color(0.4314f, 0.5137f, 0.4863f, 0.1569f), 35, 240);

        GameObject BackgroundPlaceHolderSprite = UserInterfaceUtilities.SetupGameObject("BackgroundPlaceHolderSprite", m_RightSide.transform, new Vector3(173, -63, 0));
        BackgroundPlaceHolderSprite.AddComponent<UISprite>();
        m_BackgroundSprite = BackgroundPlaceHolderSprite.GetComponent<UISprite>();

        UserInterfaceUtilities.SetupUISprite(m_BackgroundSprite, "inv_statBack", new Color(0.4314f, 0.5137f, 0.4863f, 0.1569f), 35, 100);

        //
        //GameObject ButtonAuthor = UserInterfaceUtilities.SetupGameObject("ButtonAuthor", m_RightSide.transform, new Vector3(0, -278, 0));
        //ButtonAuthor.AddComponent<UISprite>();
        //ButtonAuthor.AddComponent<UIButton>();
        //ButtonAuthor.AddComponent<BoxCollider>();
        //m_BackgroundSprite = ButtonAuthor.GetComponent<UISprite>();

        //UIButton ButtonAuthorUIButton = ButtonAuthor.GetComponent<UIButton>();
        //ButtonAuthorUIButton.normalSprite = "ico_knowledge_people";
        //ButtonAuthorUIButton.hoverSprite = "";
        //ButtonAuthorUIButton.normalSprite = "";
        //ButtonAuthorUIButton.pressedSprite = "";
        //ButtonAuthorUIButton.isEnabled = true;
        //ButtonAuthorUIButton.mSprite = m_BackgroundSprite;
        //ButtonAuthorUIButton.onClick = null;

        //UserInterfaceUtilities.SetupUISprite(m_BackgroundSprite, "ico_knowledge_people", new Color(0.7843f, 0.7843f, 0.7843f, 0.4392f), 42, 42);
        //m_BackgroundSprite.depth = 1;

        //GameObject ButtonAuthorBG = UserInterfaceUtilities.SetupGameObject("ButtonAuthorBG", ButtonAuthor.transform, Vector3.zero);
        //ButtonAuthorBG.AddComponent<UISprite>();
        //m_BackgroundSprite = ButtonAuthorBG.GetComponent<UISprite>();

        //UserInterfaceUtilities.SetupUISprite(m_BackgroundSprite, "inv_Tab_selected", new Color(0.9804f, 0.9804f, 0.9804f, 1), 56, 56);
        //

        GameObject ModsTitle = UserInterfaceUtilities.SetupGameObject("ModsTitle", m_RightSide.transform, new Vector3(0, 5, 0));
        ModsTitle.AddComponent<UILabel>();
        m_ModsTitle = ModsTitle.GetComponent<UILabel>();

        GameObject ModsDescription = UserInterfaceUtilities.SetupGameObject("ModsDescription", m_RightSide.transform, new Vector3(0, -25, 0));
        ModsDescription.AddComponent<UILabel>();
        m_ModsDescription = ModsDescription.GetComponent<UILabel>();

        GameObject ModsVersion = UserInterfaceUtilities.SetupGameObject("ModsVersion", m_RightSide.transform, new Vector3(-174, -65, 0));
        ModsVersion.AddComponent<UILabel>();
        m_ModsVersion = ModsVersion.GetComponent<UILabel>();

        GameObject AuthorName = UserInterfaceUtilities.SetupGameObject("AuthorName", m_RightSide.transform, new Vector3(0, -65, 0));
        AuthorName.AddComponent<UILabel>();
        m_AuthorName = AuthorName.GetComponent<UILabel>();

        GameObject MelonVersion = UserInterfaceUtilities.SetupGameObject("MelonVersion", m_RightSide.transform, new Vector3(174, -65, 0));
        MelonVersion.AddComponent<UILabel>();
        m_MelonVersion = MelonVersion.GetComponent<UILabel>();

        UserInterfaceUtilities.SetupLabelWithHeightAndWidth(m_ModsTitle, null, FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Center, UILabel.Overflow.ShrinkContent, true, 20, 30, Color.white, true, 76, 450);
        UserInterfaceUtilities.SetupLabelWithHeightAndWidth(m_ModsDescription, null, FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Center, UILabel.Overflow.ShrinkContent, true, 20, 15, Color.white, false, 76, 450);
        UserInterfaceUtilities.SetupLabel(m_ModsVersion, null, FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Center, UILabel.Overflow.ShrinkContent, true, 20, 20, Color.white, false);
        UserInterfaceUtilities.SetupLabel(m_AuthorName, "TESTING", FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Center, UILabel.Overflow.ShrinkContent, true, 20, 20, Color.white, true);
        UserInterfaceUtilities.SetupLabel(m_MelonVersion, null, FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Center, UILabel.Overflow.ShrinkContent, true, 20, 20, Color.white, false);
    }

    private void OnClickBack()
    {
        GameAudioManager.PlayGUIButtonBack();
        Panel_Sandbox panelSandboxInstance = GetComponentInParent<Panel_Sandbox>();

        if (panelSandboxInstance)
        {
            panelSandboxInstance.enabled = true;

            Transform parentTransform = panelSandboxInstance.gameObject.transform;
            for (int i = 0; i < parentTransform.childCount; i++)
            {
                Transform child = parentTransform.GetChild(i);

                if (child.name != "DeprecatedMenu" && child.name != "CloudSynch_Label")
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        Enable(false);
    }

    private void OnSlotClicked(string modType)
    {
        GameAudioManager.PlayGUIButtonClick();
        m_RightSide.SetActive(true);

        var menuItem = m_MenuItems.FirstOrDefault(item => item.m_Type == modType);
        if (menuItem != null)
        {
            // Update the title and description labels with the mod's details
            if (m_ModsTitle != null)
            {
                m_ModsTitle.text = menuItem.m_LabelText; // Update the title label with the mod's name
            }
            if (m_ModsDescription != null)
            {
                m_ModsDescription.text = menuItem.m_LabelDescription; // Update the description label with the mod's description
            }
            if (m_ModsVersion != null)
            {
                m_ModsVersion.text = !string.IsNullOrEmpty(menuItem.m_LabelVersion) ? $"v{menuItem.m_LabelVersion}" : "";
            }
            if (m_AuthorName != null)
            {
                m_AuthorName.text = menuItem.m_LabelAuthor;
            }
            if (m_MelonVersion != null)
            {
                m_MelonVersion.text = !string.IsNullOrEmpty(menuItem.m_LabelLoaderVersion) ? $"v{menuItem.m_LabelLoaderVersion}" : "";
            }
        }
    }

    private void Update()
    {
        m_BasicMenu?.ManualUpdate();
    }
}