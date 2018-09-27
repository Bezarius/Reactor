using System;
using System.Linq;
using Assets.Game.Enums;

public static class EnumHelper
{
    public static string[] MaskToStringArray(this Enum @enum)
    {
        return @enum.ToString()
            .Split(new[] { ", " }, StringSplitOptions.None)
            .ToArray();
    }
}