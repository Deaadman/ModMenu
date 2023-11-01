using ModMenu.Utilities;
using static Il2Cpp.BasicMenu;

namespace ModMenu;

[RegisterTypeInIl2Cpp(false)]
public class Panel_ModMenu : MonoBehaviour
{
    public Panel_ModMenu(IntPtr intPtr) : base(intPtr) { }

    private BasicMenu m_BasicMenu;

    internal GameObject m_BasicMenuRoot;

    internal GameObject m_ModsInstalledGameObject;

    private List<ModMenuItems> m_MenuItems;

    internal class ModMenuItems
    {
        internal string m_Type;
        internal string m_LabelText;
        internal string m_LabelDescription;
    }

    private void AddMenuItem(int itemIndex)
    {
        string modType = m_MenuItems[itemIndex].m_Type;

        string labelText = m_MenuItems[itemIndex].m_LabelText;
        string labelDescription = m_MenuItems[itemIndex].m_LabelDescription;

        Action actionFromType = GetActionFromType(modType);

        m_BasicMenu.AddItem(modType, modType.GetHashCode(), itemIndex, Localization.Get(labelText), Localization.Get(labelDescription), null, actionFromType, Color.clear, Color.clear);
    }

    private void ConfigureMenu()
    {
        m_BasicMenu.Reset();
        m_BasicMenu.UpdateTitle("GAMEPLAY_ModMenu", "", Vector3.zero);

        for (int i = 0; i < m_MenuItems.Count; i++)
        {
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
            m_ModsInstalledGameObject.SetActive(true);
            return;
        }
        m_BasicMenu.Enable(false);
        GameManager.GetCameraEffects().DepthOfFieldTurnOff(false);
        m_ModsInstalledGameObject.SetActive(false);
    }

    private static Action GetActionFromType(string type)
    {
        return new Action(() => OnModMenuItemClicked(type));
    }

    internal void Initialize()
    {
        m_BasicMenu = InstantiateMenu(InterfaceManager.s_BasicMenuPrefab, m_BasicMenuRoot, gameObject, this);
        m_BasicMenu.m_CanScroll = true;
        m_MenuItems = new List<ModMenuItems>();

        var loadedMods = ModFetcher.GetLoadedMods();
        foreach (var (modName, modDescription) in loadedMods)
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
                m_LabelDescription = description
            });
        }
    }

    internal void InitializeGameObjects()
    {
        GameObject basicMenuRootGameObject = new("BasicMenuRoot");
        basicMenuRootGameObject.transform.SetParent(transform, false);
        basicMenuRootGameObject.transform.localPosition = new Vector3(-473.2906f, 48, 0);

        m_BasicMenuRoot = basicMenuRootGameObject;

        m_ModsInstalledGameObject = new("ModsInstalled");
        m_ModsInstalledGameObject.transform.SetParent(transform, false);
        m_ModsInstalledGameObject.SetActive(false);

        GameObject modsInstalledLabelGameObject = new("ModsInstalledLabel");
        modsInstalledLabelGameObject.transform.SetParent(m_ModsInstalledGameObject.transform, false);
        modsInstalledLabelGameObject.AddComponent<UILabel>();
        modsInstalledLabelGameObject.transform.localPosition = new Vector3(-505, 255, 0);

        UILabel modsInstalledLabel = modsInstalledLabelGameObject.GetComponent<UILabel>();

        Panel_ChooseSandbox chooseSandboxPanel = InterfaceManager.GetPanel<Panel_ChooseSandbox>();
        if (chooseSandboxPanel != null && chooseSandboxPanel.m_SlotsUsedLabel != null)
        {
            modsInstalledLabel.ambigiousFont = chooseSandboxPanel.m_SlotsUsedLabel.ambigiousFont;
            modsInstalledLabel.bitmapFont = chooseSandboxPanel.m_SlotsUsedLabel.bitmapFont;
            modsInstalledLabel.font = chooseSandboxPanel.m_SlotsUsedLabel.font;
        }

        var installedMods = ModFetcher.GetLoadedMods();
        int numberOfModsLoaded = installedMods.Count;
        string labelText = $"{numberOfModsLoaded} {Localization.Get("GAMEPLAY_ModsLoaded")}";

        UserInterfaceUtilities.SetupLabel(modsInstalledLabel, labelText, FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Center, UILabel.Overflow.ResizeFreely, true, 20, 15, new Color(0.4784314f, 0.4784314f, 0.4784314f), true);
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

    private static void OnModMenuItemClicked(string modType)
    {
        GameAudioManager.PlayGUIButtonClick();
    }

    private void Update()
    {
        m_BasicMenu?.ManualUpdate();
    }
}