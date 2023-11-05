namespace ModMenu;

[RegisterTypeInIl2Cpp(false)]
public class ModConfig : ScriptableObject
{
    public ModConfig() { }

    public ModConfig(IntPtr intPtr) : base(intPtr) { }

    internal string m_ModType;
    internal string m_ModName;
    internal string m_ModDescription;
    internal string m_ModVersion;
    internal string m_ModAuthor;
    internal string m_ModVersionMelonLoader;
    internal string m_ModVersionCached;
}