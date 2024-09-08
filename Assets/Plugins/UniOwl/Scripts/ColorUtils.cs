using UnityEngine;

namespace UniOwl
{
    public static class ColorUtils
    {
        public static Color HexToRGBA(string hex)
        {
            if (!hex.StartsWith('#'))
                hex = "#" + hex;
            if (ColorUtility.TryParseHtmlString(hex, out var color))
                return color;
            return Color.black;
        }
    }
}