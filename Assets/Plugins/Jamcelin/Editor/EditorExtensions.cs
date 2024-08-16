using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEngine;

namespace Jamcelin.Editor
{
    public static class EditorExtensions
    {
        public static void TryCreateAddressableSO<T>(string path, string key) where T : ScriptableObject
        {
            if (!AssetDatabase.AssetPathExists(path))
                AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<T>(), path);
            MarkAddressable(AssetDatabase.AssetPathToGUID(path), key);
        }
        
        public static void MarkAddressable(string guid, string key, bool setLabel = false)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var entry = settings.FindAssetEntry(guid);
            if (entry == null)
            {
                settings.CreateAssetReference(guid);
                entry = settings.FindAssetEntry(guid);
            }
            entry.address = key;
            if (setLabel)
                entry.SetLabel(key, true, true, false);
        }
        
        public static void MarkAddressable(string guid)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var entry = settings.FindAssetEntry(guid);
            if (entry == null)
                settings.CreateAssetReference(guid);
        }
    }
}