using ModMenu.Utilities;

namespace ModMenu;

internal sealed class MelonModInitializer : MelonMod
{
    public override void OnEarlyInitializeMelon()
    {
        _ = ModVersionChecker.FetchModVersionsAsync();
    }

    public override void OnInitializeMelon()
    {
        LoadLocalizations();
        ModDetailsFetcher.LogVersionDifferences();
    }

    private static void LoadLocalizations()
    {
        const string JSONfile = "ModMenu.Resources.Localization.json";

        try
        {
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(JSONfile) ?? throw new InvalidOperationException($"Failed to load resource: {JSONfile}");
            using StreamReader reader = new(stream);

            string results = reader.ReadToEnd();

            LocalizationManager.LoadJsonLocalization(results);
        }
        catch (Exception ex)
        {
            Logging.LogError(ex.Message);
        }
    }
}