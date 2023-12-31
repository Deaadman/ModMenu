﻿namespace ModMenu.Utilities;

internal static class UserInterfaceUtilities
{
    private static UIFont? m_CachedFont;
    private static UIAtlas? m_CachedAtlas;

    private static void CacheResources()
    {
        if (m_CachedFont == null)
        {
            var versionLabel = InterfaceManager.GetPanel<Panel_MainMenu>().m_VersionLabel;
            m_CachedFont = versionLabel.font;
        }

        if (m_CachedAtlas == null)
        {
            m_CachedAtlas = InterfaceManager.GetPanel<Panel_HUD>().m_AltFireGamepadButtonSprite.atlas;
        }
    }

    internal static GameObject SetupGameObject(string name, Transform parent, Vector3 localPosition)
    {
        GameObject newGameObject = new(name);
        newGameObject.transform.SetParent(parent, false);
        newGameObject.transform.localPosition = localPosition;
        return newGameObject;
    }

    internal static GameObject SetupGameObjectExtended(string name, Transform parent, Vector3 localPosition, Quaternion localRotation)
    {
        GameObject newGameObject = SetupGameObject(name, parent, localPosition);
        newGameObject.transform.localRotation = localRotation;
        return newGameObject;
    }

    internal static void SetupLabel(UILabel label, string text, FontStyle fontStyle, UILabel.Crispness crispness, NGUIText.Alignment alignment, UILabel.Overflow overflow, bool mulitLine, int depth, int fontSize, Color color, bool capsLock)
    {
        CacheResources();

        label.text = text;
        label.font = m_CachedFont;

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

    internal static void SetupLabelExtended(UILabel label, string text, FontStyle fontStyle, UILabel.Crispness crispness, NGUIText.Alignment alignment, UILabel.Overflow overflow, bool mulitLine, int depth, int fontSize, Color color, bool capsLock, int lineHeight, int lineWidth, int spacingY, int spacingX)
    {
        SetupLabel(label, text, fontStyle, crispness, alignment, overflow, mulitLine, depth, fontSize, color, capsLock);

        label.lineHeight = lineHeight;
        label.lineWidth = lineWidth;
        label.spacingY = spacingY;
        label.spacingX = spacingX;
    }

    internal static void SetupUISprite(UISprite sprite, string spriteName, Color color, int height, int width)
    {
        CacheResources();

        UISpriteData spriteData = m_CachedAtlas.GetSprite(spriteName);
        sprite.atlas = m_CachedAtlas;
        sprite.spriteName = spriteName;
        sprite.mSprite = spriteData;
        sprite.mSpriteSet = true;
        sprite.color = color;
        sprite.height = height;
        sprite.width = width;
    }
}