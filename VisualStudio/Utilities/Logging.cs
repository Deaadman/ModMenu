namespace ModMenu.Utilities;

public class Logging
{
    public static void LogStarter() => Melon<MelonModInitializer>.Logger.Msg($"Mod Loaded, Currently on Version v{Properties.BuildInfo.Version}");
    public static void Log(string message, params object[] parameters) => Melon<MelonModInitializer>.Logger.Msg($"{message}", parameters);
    public static void LogColor(ConsoleColor color, string message, params object[] parameters) => Melon<MelonModInitializer>.Logger.Msg(color, $"{message}", parameters);
    public static void LogWarning(string message, params object[] parameters) => Melon<MelonModInitializer>.Logger.Warning($"{message}", parameters);
    public static void LogError(string message, params object[] parameters) => Melon<MelonModInitializer>.Logger.Error($"{message}", parameters);
    public static void LogSeperator(params object[] parameters) => Melon<MelonModInitializer>.Logger.Msg("==============================================================================", parameters);
    public static void LogIntraSeparator(string message, params object[] parameters) => Melon<MelonModInitializer>.Logger.Msg($"=========================   {message}   =========================", parameters);
}