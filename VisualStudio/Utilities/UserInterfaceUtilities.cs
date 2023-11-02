using UnityEngine;

namespace ModMenu.Utilities;

internal class UserInterfaceUtilities
{
    internal static GameObject SetupGameObject(string name, Transform parent, Vector3 localPosition)
    {
        GameObject newGameObject = new(name);
        newGameObject.transform.SetParent(parent, false);
        newGameObject.transform.localPosition = localPosition;
        return newGameObject;
    }

    internal static void SetupLabel(UILabel label, string text, FontStyle fontStyle, UILabel.Crispness crispness, NGUIText.Alignment alignment, UILabel.Overflow overflow, bool mulitLine, int depth, int fontSize, Color color, bool capsLock)
    {
        label.text = text;
        label.ambigiousFont = InterfaceManager.GetPanel<Panel_ChooseSandbox>().m_SlotsUsedLabel.ambigiousFont;
        label.bitmapFont = InterfaceManager.GetPanel<Panel_ChooseSandbox>().m_SlotsUsedLabel.bitmapFont;
        label.font = InterfaceManager.GetPanel<Panel_ChooseSandbox>().m_SlotsUsedLabel.font;

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

    internal static void SetupLabelWithHeightAndWidth(UILabel label, string text, FontStyle fontStyle, UILabel.Crispness crispness, NGUIText.Alignment alignment, UILabel.Overflow overflow, bool mulitLine, int depth, int fontSize, Color color, bool capsLock, int lineHeight, int lineWidth)
    {
        label.text = text;
        label.ambigiousFont = InterfaceManager.GetPanel<Panel_ChooseSandbox>().m_SlotsUsedLabel.ambigiousFont;
        label.bitmapFont = InterfaceManager.GetPanel<Panel_ChooseSandbox>().m_SlotsUsedLabel.bitmapFont;
        label.font = InterfaceManager.GetPanel<Panel_ChooseSandbox>().m_SlotsUsedLabel.font;

        label.fontStyle = fontStyle;
        label.keepCrispWhenShrunk = crispness;
        label.alignment = alignment;
        label.overflowMethod = overflow;
        label.multiLine = mulitLine;
        label.depth = depth;
        label.fontSize = fontSize;
        label.color = color;
        label.capsLock = capsLock;
        label.lineHeight = lineHeight;
        label.lineWidth = lineWidth;
    }

    public static void SetupUISprite(UISprite sprite, string spriteName, Color color, int height, int width)
    {
        UIAtlas baseAtlas = InterfaceManager.GetPanel<Panel_HUD>().m_AltFireGamepadButtonSprite.atlas;
        UISpriteData spriteData = baseAtlas.GetSprite(spriteName);

        sprite.atlas = baseAtlas;
        sprite.spriteName = spriteName;
        sprite.mSprite = spriteData;
        sprite.mSpriteSet = true;
        sprite.color = color;
        sprite.height = height;
        sprite.width = width;
    }
}