using System;
using System.Linq;

public static class EnumHelper
{
    public static string[] MaskToStringArray(this Enum @enum)
    {
        return @enum.ToString()
            .Split(new[] { ", " }, StringSplitOptions.None)
            .ToArray();
    }
}