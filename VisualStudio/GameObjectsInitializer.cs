using ModMenu.Utilities;
using UnityEngine;

namespace ModMenu;

[RegisterTypeInIl2Cpp(false)]
internal class GameObjectsInitializer : MonoBehaviour
{ 

    public GameObjectsInitializer(IntPtr intPtr) : base(intPtr) { }
    public GameObject DetailsGO { get; private set; }
    public GameObject MiddleSpriteGO { get; private set; }

    public UISprite MelonSprite { get; private set; }

    private readonly string ModSpriteName = "ico_crafting";
    private readonly string PluginSpriteName = "ico_crafting2";

    private readonly int VersionTitleLabelY = 53;
    private readonly int VersionLabelY = 40;
    private readonly int VersionCachedLabelsX = 125;
    private readonly int VersionMLLabelsX = 240;

    private readonly Color GreyColor = new(0.7843f, 0.7843f, 0.7843f, 1);
    private readonly Color DarkGreyColor = new(0.235f, 0.2353f, 0.2353f, 1);

    public UILabel ModName { get; private set; }
    public UILabel ModDescription { get; private set; } 
    public UILabel ModAuthor { get; private set; }
    public UILabel ModVersion { get; private set; }
    public UILabel ModVersionCached { get; private set; }
    public UILabel ModLoaderVersion { get; private set; }

    internal void InitializeDetails(Transform defaultRoot)
    {
        DetailsGO = UserInterfaceUtilities.SetupGameObject("Details", defaultRoot.transform, Vector3.zero);
        GameObject DescriptionGO = UserInterfaceUtilities.SetupGameObject("Description", DetailsGO.transform, new Vector3(409, 137, 0));
        GameObject VersionsGO = UserInterfaceUtilities.SetupGameObject("Versions", DescriptionGO.transform, new Vector3(-90, -106, 0));

        #region DescriptionGO Child Objects
        GameObject ModNameLabel = UserInterfaceUtilities.SetupGameObject("ModNameLabel", DescriptionGO.transform, new Vector3(-1, 1, 0));
        Transform ModNameLabelTransform = ModNameLabel.transform;
        ModNameLabel.AddComponent<UILabel>();
        ModName = ModNameLabel.GetComponent<UILabel>();

        GameObject ModDescriptionLabel = UserInterfaceUtilities.SetupGameObject("ModDescriptionLabel", DescriptionGO.transform, new Vector3(0, -105, 0));
        ModDescriptionLabel.AddComponent<UILabel>();
        ModDescription = ModDescriptionLabel.GetComponent<UILabel>();
        ModDescription.topAnchor.target = ModNameLabelTransform;

        GameObject ModAuthorLabel = UserInterfaceUtilities.SetupGameObject("ModAuthorLabel", DescriptionGO.transform, new Vector3(0, -20, 0));
        ModAuthorLabel.AddComponent<UILabel>();
        ModAuthor = ModAuthorLabel.GetComponent<UILabel>();

        GameObject MelonSpriteGO = UserInterfaceUtilities.SetupGameObject("MelonSprite", DescriptionGO.transform, new Vector3(-120, 50, 0));
        MelonSpriteGO.AddComponent<UISprite>();
        MelonSprite = MelonSpriteGO.GetComponent<UISprite>();
        #endregion

        #region VersionsGO Child Objects
        GameObject CurrentVersionLabelGO = UserInterfaceUtilities.SetupGameObject("CurrentVersionLabel", VersionsGO.transform, new Vector3(0, VersionTitleLabelY, 0));
        CurrentVersionLabelGO.AddComponent<UILabel>();
        UILabel CurrentVersionLabel = CurrentVersionLabelGO.GetComponent<UILabel>();

        GameObject ModVersionLabel = UserInterfaceUtilities.SetupGameObject("ModVersionLabel", VersionsGO.transform, new Vector3(0, VersionLabelY, 0));
        ModVersionLabel.AddComponent<UILabel>();
        ModVersion = ModVersionLabel.GetComponent<UILabel>();

        GameObject LineBreakGO = UserInterfaceUtilities.SetupGameObjectExtended("LineBreak", VersionsGO.transform, new Vector3(60, 43, 0), Quaternion.Euler(0, 0, 90));
        LineBreakGO.AddComponent<UISprite>();
        UISprite LineBreak = LineBreakGO.GetComponent<UISprite>();

        GameObject CachedVersionLabelGO = UserInterfaceUtilities.SetupGameObject("CachedVersionLabel", VersionsGO.transform, new Vector3(VersionCachedLabelsX, VersionTitleLabelY, 0));
        CachedVersionLabelGO.AddComponent<UILabel>();
        UILabel CachedVersionLabel = CachedVersionLabelGO.GetComponent<UILabel>();

        GameObject ModVersionCachedLabel = UserInterfaceUtilities.SetupGameObject("ModVersionCachedLabel", VersionsGO.transform, new Vector3(VersionCachedLabelsX, VersionLabelY, 0));
        ModVersionCachedLabel.AddComponent<UILabel>();
        ModVersionCached = ModVersionCachedLabel.GetComponent<UILabel>();

        GameObject LineBreakGO2 = UserInterfaceUtilities.SetupGameObjectExtended("LineBreak", VersionsGO.transform, new Vector3(175, 43, 0), Quaternion.Euler(0, 0, 90));
        LineBreakGO2.AddComponent<UISprite>();
        UISprite LineBreak2 = LineBreakGO2.GetComponent<UISprite>();

        GameObject MLVersionGO = UserInterfaceUtilities.SetupGameObject("MLVersion", VersionsGO.transform, new Vector3(VersionMLLabelsX, VersionTitleLabelY, 0));
        MLVersionGO.AddComponent<UILabel>();
        UILabel MLVersionLabel = MLVersionGO.GetComponent<UILabel>();

        GameObject MelonLoaderVersionLabel = UserInterfaceUtilities.SetupGameObject("MelonLoaderVersionLabel", VersionsGO.transform, new Vector3(VersionMLLabelsX, VersionLabelY, 0));
        MelonLoaderVersionLabel.AddComponent<UILabel>();
        ModLoaderVersion = MelonLoaderVersionLabel.GetComponent<UILabel>();

        #endregion

        MiddleSpriteGO = UserInterfaceUtilities.SetupGameObject("MiddleSprite", defaultRoot.transform, Vector3.zero);
        MiddleSpriteGO.AddComponent<UISprite>();
        UISprite MiddleSprite = MiddleSpriteGO.GetComponent<UISprite>();

        UserInterfaceUtilities.SetupLabelExtended(ModName, null, FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Left, UILabel.Overflow.ResizeHeight, false, 20, 20, Color.white, true, 18, 280, 0, 1);
        UserInterfaceUtilities.SetupLabelExtended(ModDescription, null, FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Left, UILabel.Overflow.ResizeHeight, true, 20, 14, GreyColor, false, 20, 280, 3, 1);
        UserInterfaceUtilities.SetupLabelExtended(ModAuthor, null, FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Left, UILabel.Overflow.ShrinkContent, false, 20, 14, GreyColor, true, 20, 280, 3, 1);
        UserInterfaceUtilities.SetupLabel(CurrentVersionLabel, Localization.Get("GAMEPLAY_CurrentVersion"), FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Left, UILabel.Overflow.ClampContent, false, 20, 12, DarkGreyColor, true);
        UserInterfaceUtilities.SetupLabel(ModVersion, null, FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Left, UILabel.Overflow.ShrinkContent, false, 20, 12, GreyColor, false);
        UserInterfaceUtilities.SetupLabel(CachedVersionLabel, Localization.Get("GAMEPLAY_LatestVersion"), FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Left, UILabel.Overflow.ClampContent, false, 20, 12, DarkGreyColor, true);
        UserInterfaceUtilities.SetupLabel(ModVersionCached, null, FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Left, UILabel.Overflow.ClampContent, false, 20, 12, GreyColor, false);
        UserInterfaceUtilities.SetupLabel(MLVersionLabel, Localization.Get("GAMEPLAY_MLVersion"), FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Left, UILabel.Overflow.ClampContent, false, 20, 12, DarkGreyColor, true);
        UserInterfaceUtilities.SetupLabel(ModLoaderVersion, null, FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Left, UILabel.Overflow.ClampContent, false, 20, 12, GreyColor, false);

        UserInterfaceUtilities.SetupUISprite(LineBreak, "linebreak_description", new Color(0.4779f, 0.4779f, 0.4779f, 1), 8, 30);
        UserInterfaceUtilities.SetupUISprite(LineBreak2, "linebreak_description", new Color(0.4779f, 0.4779f, 0.4779f, 1), 8, 30);

        UserInterfaceUtilities.SetupUISprite(MelonSprite, null, Color.white, 64, 64);

        UserInterfaceUtilities.SetupUISprite(MiddleSprite, "ico_log_Knowledge", Color.white, 128, 128);

        SetGameObjectsActive(false);
    }

    internal void InitializeModStatusSprites(Transform menuItemRoot)
    {
        GameObject statusGO = UserInterfaceUtilities.SetupGameObject("ModStatus", menuItemRoot.transform, Vector3.zero);
        statusGO.AddComponent<UISprite>();
        UISprite statusSprite = statusGO.GetComponent<UISprite>();

        UserInterfaceUtilities.SetupUISprite(statusSprite, "ico_log_Knowledge", Color.white, 64, 64);
        // Code here for the gameobject which switches between icons such as arrow, exclimation mark, question mark and check mark to determine the status of the mod.
        // Check mark = no errors, fully updated.
        // Question mark = Potentional errors, version newer than API
        // Exclimation mark = Old mod version, missing dependecies or error
        // Up arrow mark = Mod update avaliable.
    }

    internal void SetGameObjectsActive(bool isActive)
    {
        DetailsGO?.SetActive(isActive);
        MiddleSpriteGO?.SetActive(isActive);
    }

    internal void SetMelonSprite(bool isPlugin)
    {
        if (MelonSprite != null)
        {
            MelonSprite.spriteName = isPlugin ? PluginSpriteName : ModSpriteName;
        }
    }
}