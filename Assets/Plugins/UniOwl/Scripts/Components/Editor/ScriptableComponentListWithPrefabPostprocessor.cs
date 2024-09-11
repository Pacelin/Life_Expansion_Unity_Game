using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniOwl.Components.Editor
{
    public class ScriptableComponentListWithPrefabPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
                string[] importedAssets,
                string[] deletedAssets,
                string[] movedAssets,
                string[] movedFromAssetPaths
            )
        {
            foreach (var assetPath in importedAssets)
            {
                var type = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
                
                if (!TypeUtils.IsAssignableToGenericType(type , typeof(ScriptableComponentListWithPrefab<>)))
                    continue;
                CreatePrefab(assetPath, type);
            }
        }

        private static void CreatePrefab(string settingsPath, Type type)
        {
            Object settings = AssetDatabase.LoadAssetAtPath(settingsPath, type);

            SerializedObject so = new SerializedObject(settings);
            var prefab = (GameObject)so.FindProperty("_prefab").objectReferenceValue;

            var rootProp = so.FindProperty("_root");
            
            if (rootProp.objectReferenceValue)
                return;
            
            string folderPath = Path.GetDirectoryName(settingsPath);
            
            string prefix = settings.name.StartsWith("SO_") ? settings.name[3..] : settings.name;
            GameObject variant = PrefabManager.CreatePrefabVariant(folderPath, prefix, prefab);
            
            rootProp.objectReferenceValue = variant;
            so.ApplyModifiedProperties();
        }
    }
}
