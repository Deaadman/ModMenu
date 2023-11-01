using ModMenu.Utilities;
using static Il2Cpp.BasicMenu;

namespace ModMenu;

[RegisterTypeInIl2Cpp]
public class Panel_ModMenu : MonoBehaviour
{
    public Panel_ModMenu(IntPtr intPtr) : base(intPtr) { }

    private BasicMenu m_BasicMenu;

    public GameObject m_BasicMenuRoot;

    public List<ModMenuItems> m_MenuItems;

    public class ModMenuItems
    {
        public string m_Type;

        public string m_LabelLocalizationId;
    }

    private void AddMenuItem(int itemIndex)
    {
        string modType = m_MenuItems[itemIndex].m_Type;
        Logging.Log("Adding menu item with type: " + modType);

        string labelLocalizationId = m_MenuItems[itemIndex].m_LabelLocalizationId;
        string description = "GAMEPLAY_Description" + modType;

        Action actionFromType = GetActionFromType(modType);
        if (actionFromType == null)
        {
            Logging.LogWarning("Action for menu item type: " + modType + " is null");
        }

        // NOTE: Modify the below line if m_BasicMenu.AddItem doesn't accept string for type
        m_BasicMenu.AddItem(modType, modType.GetHashCode(), itemIndex, Localization.Get(labelLocalizationId), Localization.Get(description), null, actionFromType, Color.clear, Color.clear);
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

    public void Enable(bool enable)
    {
        if (enable)
        {
            ConfigureMenu();
            m_BasicMenu.Enable(true);
            GameManager.GetCameraEffects().DepthOfFieldTurnOn();
            return;
        }
        m_BasicMenu.Enable(false);
        GameManager.GetCameraEffects().DepthOfFieldTurnOff(false);
    }

    public Action GetActionFromType(string type)
    {
        Logging.Log("Fetching action for mod type: " + type);
        return new Action(OnModMenuItemClicked);
    }

    public void Initialize()
    {
        m_BasicMenu = InstantiateMenu(InterfaceManager.s_BasicMenuPrefab, m_BasicMenuRoot, gameObject, this);
        m_MenuItems = new List<ModMenuItems>();

        var installedMods = ModFetcher.GetInstalledMods();
        foreach (var mod in installedMods)
        {
            m_MenuItems.Add(new ModMenuItems { m_Type = mod, m_LabelLocalizationId = $"GAMEPLAY_{mod.Replace(" ", "")}" });
        }

        Logging.Log("Menu initialized with " + m_MenuItems.Count + " items.");
    }

    public void OnClickBack()
    {
        Logging.Log("Back button clicked.");
        GameAudioManager.PlayGUIButtonBack();
        Panel_Sandbox panelSandboxInstance = GetComponentInParent<Panel_Sandbox>();

        if (panelSandboxInstance)
        {
            Logging.Log("Found Panel_Sandbox instance. Enabling it.");
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
        else
        {
            Logging.LogWarning("No Panel_Sandbox instance found on back button click.");
        }
        Enable(false);
    }

    public void OnModMenuItemClicked()
    {
        // Handle what should happen when any mod menu item is clicked.
        Logging.Log("Mod menu item clicked.");
    }

    private void Update()
    {
        m_BasicMenu?.ManualUpdate();
    }
}