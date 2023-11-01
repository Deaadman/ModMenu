namespace ModMenu.Utilities;

internal class UserInterfaceUtilities
{
    internal static void SetupLabel(
        UILabel label,
        string text,
        FontStyle fontStyle,
        UILabel.Crispness crispness,
        NGUIText.Alignment alignment,
        UILabel.Overflow overflow,
        bool mulitLine,
        int depth,
        int fontSize,
        Color color,
        bool capsLock)
    {
        label.text = text;
        //label.ambigiousFont = GameManager.GetFontManager().GetUIFontForCharacterSet(FontManager.m_CurrentCharacterSet);
        //label.bitmapFont = GameManager.GetFontManager().GetUIFontForCharacterSet(FontManager.m_CurrentCharacterSet);
        //label.font = GameManager.GetFontManager().GetUIFontForCharacterSet(FontManager.m_CurrentCharacterSet);

        label.fontStyle = fontStyle;
        label.keepCrispWhenShrunk = crispness;
        label.alignment = alignment;
        label.overflowMethod = overflow;
        label.multiLine = mulitLine;
        label.depth = depth;
        label.fontSize = fontSize;
        label.color = color;
        label.capsLock = capsLock;
    }
}