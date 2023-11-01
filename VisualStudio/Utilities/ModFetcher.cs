namespace ModMenu.Utilities
{
    internal class ModFetcher
    {
        internal static List<(string ModName, string Description)> GetLoadedMods()
        {
            List<(string ModName, string Description)> installedMods = new();

            foreach (var melon in MelonMod.RegisteredMelons)
            {
                string modName = melon.Info.Name;
                string description = GetAssemblyDescriptionForMod(modName) ?? "";
                installedMods.Add((modName, description));
            }

            return installedMods;
        }

        private static string GetAssemblyDescriptionForMod(string modGuiName)
        {
            try
            {
                string assemblyNameSearch = modGuiName.Replace(" ", "");
                var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name.Equals(assemblyNameSearch, StringComparison.OrdinalIgnoreCase));

                if (assembly != null)
                {
                    var descriptionAttribute = assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
                                                      .OfType<AssemblyDescriptionAttribute>().FirstOrDefault();

                    if (descriptionAttribute != null)
                    {
                        return descriptionAttribute.Description;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogError($"{ex.Message}");
            }

            return null;
        }
    }
}