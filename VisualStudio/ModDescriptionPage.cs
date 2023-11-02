namespace ModMenu;

[RegisterTypeInIl2Cpp(false)]
public class ModDescriptionPage : MonoBehaviour
{
    public ModDescriptionPage(IntPtr intPtr) : base(intPtr) { }

    // Public properties for accessing the labels from outside
    public UILabel ItemNameLabel { get; private set; }
    public UILabel ItemDescLabel { get; private set; }

    // Method to set up the labels, called externally after instantiation
    public void Initialize(GameObject itemNameLabelGO, GameObject itemDescLabelGO)
    {
        // Attach the UILabel component to the GameObjects if not already attached
        ItemNameLabel = itemNameLabelGO.GetComponent<UILabel>() ?? itemNameLabelGO.AddComponent<UILabel>();
        ItemDescLabel = itemDescLabelGO.GetComponent<UILabel>() ?? itemDescLabelGO.AddComponent<UILabel>();

        // Here you can add additional setup if necessary, for example setting default values
        // ItemNameLabel.text = "Default Name";
        // ItemDescLabel.text = "Default Description";
    }
}