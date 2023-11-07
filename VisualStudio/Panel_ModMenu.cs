using Il2CppTLD.UI;
using ModMenu.Utilities;
using static Il2Cpp.BasicMenu;

namespace ModMenu;

[RegisterTypeInIl2Cpp(false)]
public class Panel_ModMenu : Panel_AutoReferenced
{
    public Panel_ModMenu(IntPtr intPtr) : base(intPtr) { }

    // public ButtonLegendContainer m_ButtonLegendContainer = new();

    private GameObjectsInitializer? m_GameObjectInitializer;

    private Action[]? m_ItemClickedAction;

    public List<ModConfig>? ModMenuItems;

    #region Basic Menu
    private BasicMenu? m_BasicMenu;
    private GameObject? m_BasicMenuRoot;
    #endregion

    #region Header
    private readonly string m_HeaderTitle = "GAMEPLAY_ModMenu";
    private readonly string m_HeaderText = "";
    private Vector3 m_HeaderOffset = Vector3.zero;
    #endregion

    #region UILabels
    private UILabel? m_Name;
    private UILabel? m_Description;
    private UILabel? m_Author;
    private UILabel? m_Version;
    private UILabel? m_VersionAPI;
    private UILabel? m_VersionML;
    #endregion

    private void AddMenuItem(int itemIndex)
    {
        if (ModMenuItems != null && m_ItemClickedAction != null && itemIndex >= 0 && itemIndex < ModMenuItems.Count)
        {
            string? itemName = ModMenuItems[itemIndex].Name;
            if (itemName != null)
            {
                var modNameLocalization = GetLocalizations(itemName).Item1;

                m_BasicMenu?.AddItem(
                        itemName,
                        itemName.GetHashCode(),
                        itemIndex,
                        Localization.Get(modNameLocalization),
                        null,
                        null,
                        m_ItemClickedAction[itemIndex],
                        Color.clear,
                        Color.clear
                    );
            }
        }

        //m_GameObjectInitializer.InitializeModStatusSprites(menuItemGameObject.transform); // Try to get add a gameobject sprite to each menu item
    }

    private void ConfigureMenu()
    {
        if (m_BasicMenu == null)
        {
            return;
        }
        m_BasicMenu.Reset();
        m_BasicMenu.UpdateTitle(m_HeaderTitle, m_HeaderText, m_HeaderOffset);

        for (int i = 0; i < ModMenuItems?.Count; i++)
        {
            AddMenuItem(i);
        }

        m_BasicMenu.SetBackAction(new Action(OnClickBack));
        //m_BasicMenu.EnableConfirm(true, "");                                                 // A start for adding more buttons?
    }

    public override void Enable(bool enable)
    {
        if (enable)
        {
            ConfigureMenu();
            m_BasicMenu?.Enable(true);
            m_Panel.alpha = 1;
            //GameManager.GetCameraEffects().DepthOfFieldTurnOn();                                          // Causing an issue with the duplicated Copyright GameObject - so disablling for now.
            return;
        }
        m_BasicMenu?.Enable(false);
        m_Panel.alpha = 0;
        //GameManager.GetCameraEffects().DepthOfFieldTurnOff(false);
    }

    private static (string, string) GetLocalizations(string modName)
    {
        string modNameLocalizationKey = "GAMEPLAY_" + modName;
        string modDescriptionLocalizationKey = "GAMEPLAY_" + modName + "Description";
        return (modNameLocalizationKey, modDescriptionLocalizationKey);
    }

    public override void Initialize()
    {
        m_BasicMenuRoot = UserInterfaceUtilities.SetupGameObject("MenuRoot", transform, new Vector3(-473.2906f, 48, 0));
        m_BasicMenu = InstantiateMenu(InterfaceManager.s_BasicMenuPrefab, m_BasicMenuRoot, gameObject, this);
        m_BasicMenu.m_CanScroll = true;

        ModMenuItems = ModConfigManager.CreateModConfigs();

        m_GameObjectInitializer = gameObject.AddComponent<GameObjectsInitializer>();
        m_GameObjectInitializer.InitializeDetails(transform);

        m_Name = m_GameObjectInitializer.Name;
        m_Description = m_GameObjectInitializer.Description;
        m_Author = m_GameObjectInitializer.Author;
        m_Version = m_GameObjectInitializer.Version;
        m_VersionAPI = m_GameObjectInitializer.VersionAPI;
        m_VersionML = m_GameObjectInitializer.VersionML;

        m_ItemClickedAction = new Action[ModMenuItems.Count];
        for (int i = 0; i < ModMenuItems.Count; i++)
        {
            int index = i;
            m_ItemClickedAction[i] = () => OnClickMod(index);
        }
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
        m_GameObjectInitializer?.SetGameObjectsActive(false);
        Enable(false);
    }

    private void OnClickMod(int itemIndex)              // Update this to use a fallback system, e.g if no version is found or translation is found then roll back to N/A or the mod name. Then log if it's used the rollback system.
    {
        GameAudioManager.PlayGUIButtonClick();
        m_GameObjectInitializer?.SetGameObjectsActive(true);
        if (itemIndex >= 0 && ModMenuItems != null && itemIndex < ModMenuItems.Count)
        {
            var modConfig = ModMenuItems[itemIndex];
            if (modConfig != null)
            {
                var (modNameLocalizationKey, modDescriptionLocalizationKey) = GetLocalizations(modConfig.Name!);

                if (m_Name != null) m_Name.text = Localization.Get(modNameLocalizationKey);
                if (m_Description != null) m_Description.text = Localization.Get(modDescriptionLocalizationKey);
                if (m_Author != null) m_Author.text = modConfig.Author ?? string.Empty;
                if (m_Version != null) m_Version.text = "v" + modConfig.Version ?? string.Empty;
                if (m_VersionAPI != null) m_VersionAPI.text = "v" + modConfig.VersionAPI ?? string.Empty;
                if (m_VersionML != null) m_VersionML.text = "v" + modConfig.VersionML ?? string.Empty;

                m_GameObjectInitializer?.SetMelonSprite(modConfig.Type == "Plugin");
            }
        }
    }

    internal void Update()
    {
        m_BasicMenu?.ManualUpdate();
    }
}