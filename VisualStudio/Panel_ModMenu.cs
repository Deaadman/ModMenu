using Il2CppTLD.UI;
using ModMenu.Utilities;
using static Il2Cpp.BasicMenu;

namespace ModMenu;

[RegisterTypeInIl2Cpp(false)]
public class Panel_ModMenu : Panel_AutoReferenced
{
    public Panel_ModMenu(IntPtr intPtr) : base(intPtr) { }

    private GameObjectsInitializer m_GameObjectInitializer;

    private Action[] onItemClickActions;

    #region Basic Menu
    private BasicMenu m_BasicMenu;
    private GameObject m_BasicMenuRoot;
    #endregion

    #region Header
    private readonly string m_HeaderTitle = "GAMEPLAY_ModMenu";
    private readonly string m_HeaderText = "";
    private Vector3 m_HeaderOffset = Vector3.zero;
    #endregion

    private UILabel m_Title;
    private UILabel m_Description;

    public List<ModConfig> m_ModItems;

    private void AddMenuItem(int itemIndex)
    {
        var (modNameLocalization, modDescriptionLocalization) = GetLocalizations(m_ModItems[itemIndex].m_ModName);
        m_BasicMenu.AddItem(m_ModItems[itemIndex].m_ModName, m_ModItems[itemIndex].m_ModName.GetHashCode(), itemIndex, Localization.Get(modNameLocalization), null, null, onItemClickActions[itemIndex], Color.clear, Color.clear);
    }

    private void ConfigureMenu()
    {
        if (m_BasicMenu == null)
        {
            return;
        }
        m_BasicMenu.Reset();
        m_BasicMenu.UpdateTitle(m_HeaderTitle, m_HeaderText, m_HeaderOffset);

        for (int i = 0; i < m_ModItems.Count; i++)
        {
            AddMenuItem(i);
        }
        m_BasicMenu.SetBackAction(new Action(OnClickBack));
    }

    public override void Enable(bool enable)
    {
        if (enable)
        {
            ConfigureMenu();
            m_BasicMenu.Enable(true);
            m_Panel.alpha = 1;
            GameManager.GetCameraEffects().DepthOfFieldTurnOn();
            return;
        }
        m_BasicMenu.Enable(false);
        m_Panel.alpha = 0;
        GameManager.GetCameraEffects().DepthOfFieldTurnOff(false);
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
        m_ModItems = ModConfigManager.CreateModConfigs();

        m_GameObjectInitializer = gameObject.AddComponent<GameObjectsInitializer>();
        m_GameObjectInitializer.InitializeDetails(m_BasicMenuRoot);

        m_Title = m_GameObjectInitializer.Title;
        m_Description = m_GameObjectInitializer.Description;

        onItemClickActions = new Action[m_ModItems.Count];
        for (int i = 0; i < m_ModItems.Count; i++)
        {
            int index = i;
            onItemClickActions[i] = () => OnClickMod(index);
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
        Enable(false);
    }

    private void OnClickMod(int itemIndex)
    {
        GameAudioManager.PlayGUIButtonClick();

        if (itemIndex >= 0 && itemIndex < m_ModItems.Count)
        {
            var (modNameLocalizationKey, modDescriptionLocalizationKey) = GetLocalizations(m_ModItems[itemIndex].m_ModName);
            m_Title.text = Localization.Get(modNameLocalizationKey);
            m_Description.text = Localization.Get(modDescriptionLocalizationKey);
        }
    }

    private void Update()
    {
        m_BasicMenu?.ManualUpdate();
    }
}