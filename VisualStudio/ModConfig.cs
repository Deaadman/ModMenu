namespace ModMenu;

[RegisterTypeInIl2Cpp(false)]
public class ModConfig : ScriptableObject
{
    public ModConfig() { }
    public ModConfig(IntPtr intPtr) : base(intPtr) { }

    public string? Type { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Author { get; set; }
    public string? Version { get; set; }
    public string? VersionAPI { get; set; }
    public string? VersionML { get; set; }
}