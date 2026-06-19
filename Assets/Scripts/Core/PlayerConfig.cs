using System;
using UnityEngine;

public static class PlayerConfig
{
    public enum ColorTarget { Body, Armor }

    public static Color? BodyColor  { get; private set; } = ParseHex("#E76405");
    public static Color? ArmorColor { get; private set; } = ParseHex("#C52800");
    public static ColorTarget ActiveTarget { get; set; } = ColorTarget.Body;

    public static event Action<ColorTarget, Color> OnColorChanged;

    private static Color ParseHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }

    public static void SetColor(Color color)
    {
        if (ActiveTarget == ColorTarget.Body)
            BodyColor = color;
        else
            ArmorColor = color;
        OnColorChanged?.Invoke(ActiveTarget, color);
    }
}
