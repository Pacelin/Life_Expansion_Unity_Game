using UnityEditor;
using UnityEngine;

namespace UniOwl.Editor
{
    public static class EditorStyles
    {
        public static readonly Texture2D paneOptionsIcon;
        public static readonly Texture2D binIcon;

        public static readonly Color lightThemeIconTint = ColorUtils.HexToRGBA("#000000");
        public static readonly Color darkThemeIconTint = ColorUtils.HexToRGBA("#FFFFFF");

        public static Color iconTint => EditorGUIUtility.isProSkin ? darkThemeIconTint : lightThemeIconTint;
        
        static EditorStyles()
        {
            paneOptionsIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/UniOwl/Editor Default Resources/Icons/T_Icon_More_Dots.png");
            binIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Plugins/UniOwl/Editor Default Resources/Icons/T_Icon_Bin.png");
        }
    }
}