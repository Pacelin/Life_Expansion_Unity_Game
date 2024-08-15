using System.IO;
using System.Linq;
using Jamcelin.Runtime;
using Jamcelin.Runtime.Scoping;
using UnityEditor;
using UnityEngine;

namespace Jamcelin.Editor
{
    public class JamcelinEditor
    {
        private const string SETTINGS_ASSET_PATH = "Assets/Plugins/Jamcelin/Entry Point Settings.asset";
        private const string SETTINGS_ASSET_KEY = "Entry Point Settings";
        
        [InitializeOnLoadMethod]
        private static void Load()
        {
            EditorApplication.update += UpdateEditor;
        }

        private static void UpdateEditor()
        {
            EditorExtensions.TryCreateAddressableSO<EntryPointSettings>(SETTINGS_ASSET_PATH, SETTINGS_ASSET_KEY);
            CheckScopes();
        }

        private static void CheckScopes()
        {
            var scopesGuids = AssetDatabase.FindAssets($"t:{nameof(GameObject)}");
            foreach (var scopeGuid in scopesGuids)
            {
                var scopePath = AssetDatabase.GUIDToAssetPath(scopeGuid);
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(scopePath);
                var scope = go.GetComponent<JamScope>();
                if (scope == null)
                    return;

                var serializedScope = new SerializedObject(scope);
                
                var folderPath = Path.GetDirectoryName(AssetDatabase.GUIDToAssetPath(scopeGuid));
                var prop = serializedScope.FindProperty("_installers");
                var installersArray = AssetDatabase.FindAssets(
                    $"t:{nameof(JamInstaller)}", new[] { folderPath })
                    .Select(guid =>
                    {
                        var installerPath = AssetDatabase.GUIDToAssetPath(guid);
                        return AssetDatabase.LoadAssetAtPath<JamInstaller>(installerPath);
                    })
                    .Where(installer => installer.Enabled).ToArray();
                prop.arraySize = installersArray.Length;
                for (int i = 0; i < installersArray.Length; i++)
                    prop.GetArrayElementAtIndex(i).objectReferenceValue = installersArray[i];
                serializedScope.ApplyModifiedProperties();
            }
        }
    }
}