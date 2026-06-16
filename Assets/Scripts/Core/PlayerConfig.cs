using System;
using UnityEngine;

public static class PlayerConfig
{
    public enum ColorTarget { Body, Armor }

    public static Color? BodyColor  { get; private set; }
    public static Color? ArmorColor { get; private set; }
    public static ColorTarget ActiveTarget { get; set; } = ColorTarget.Body;

    public static event Action<ColorTarget, Color> OnColorChanged;

    public static void SetColor(Color color)
    {
        if (ActiveTarget == ColorTarget.Body)
            BodyColor = color;
        else
            ArmorColor = color;
        OnColorChanged?.Invoke(ActiveTarget, color);
    }
}
