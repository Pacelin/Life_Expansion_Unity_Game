using System;
using System.Reflection;

namespace UniOwl.Components.Editor
{
    public static class ScriptableComponentUtils
    {
        public static string GetDisplayName(Type type)
        {
            var attr = type.GetCustomAttribute<SearchMenuAttribute>();
            return attr?.Name ?? type.FullName;
        }

        public static string GetSearchPath(Type type)
        {
            var attr = type.GetCustomAttribute<SearchMenuAttribute>();
            return attr?.Path ?? "Custom/";
        }
    }
}