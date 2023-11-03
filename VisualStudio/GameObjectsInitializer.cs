using ModMenu.Utilities;

namespace ModMenu;

[RegisterTypeInIl2Cpp(false)]
internal class GameObjectsInitializer : MonoBehaviour
{
    public GameObjectsInitializer(IntPtr intPtr) : base(intPtr) { }

    public UILabel Title { get; private set; }
    public UILabel Description { get; private set; }

    public void InitializeDetails(GameObject menuRoot)
    {
        GameObject Details = UserInterfaceUtilities.SetupGameObject("Details", menuRoot.transform, Vector3.zero);
        GameObject DescriptionObject = UserInterfaceUtilities.SetupGameObject("Description", Details.transform, new Vector3(500, 150, 0));

        GameObject Label_Name = UserInterfaceUtilities.SetupGameObject("Label_Name", DescriptionObject.transform, new Vector3(0, 0, 0));
        Label_Name.AddComponent<UILabel>();
        Title = Label_Name.GetComponent<UILabel>();

        GameObject Label_Desc = UserInterfaceUtilities.SetupGameObject("Label_Desc", DescriptionObject.transform, new Vector3(0, -98, 0));
        Label_Desc.AddComponent<UILabel>();
        Description = Label_Desc.GetComponent<UILabel>();

        UserInterfaceUtilities.SetupLabelWithHeightAndWidth(Title, "", FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Center, UILabel.Overflow.ResizeFreely, true, 20, 20, Color.white, true, 20, 180);
        UserInterfaceUtilities.SetupLabelWithHeightAndWidth(Description, "", FontStyle.Normal, UILabel.Crispness.Always, NGUIText.Alignment.Left, UILabel.Overflow.ResizeHeight, true, 20, 14, new Color(0.7843f, 0.7843f, 0.7843f, 1), false, 34, 280);
    }
}