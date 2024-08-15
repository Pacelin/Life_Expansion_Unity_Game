using System.IO;
using Jamcelin.Runtime.Scoping;
using UnityEditor;
using UnityEngine;

namespace Jamcelin.Editor
{
    public static class ScopeEditor
    {
        [MenuItem("Assets/Create/Jamcelin/Scope Folder")]
        private static void CreateScopeFolder()
        {
            var selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
            var choosedPath = EditorUtility.SaveFilePanelInProject(
                "Create Scope Folder", "Scope Folder", "", "", selectedPath);
            if (string.IsNullOrEmpty(choosedPath))
                return;
            var folderName = Path.GetFileName(choosedPath);
            var folderPath = Path.GetDirectoryName(choosedPath);
            var scopePath = Path.Combine(choosedPath, folderName + " Scope.prefab");
            
            AssetDatabase.CreateFolder(folderPath, folderName);
            AssetDatabase.CreateFolder(choosedPath, "Installers");
            CreatePrefab(scopePath);
            if (folderName.Contains("Scope"))
                EditorExtensions.MarkAddressable(AssetDatabase.AssetPathToGUID(scopePath), folderName);
            else
                EditorExtensions.MarkAddressable(AssetDatabase.AssetPathToGUID(scopePath), folderName + " Scope");
            
            AssetDatabase.Refresh();
        }

        private static void CreatePrefab(string path)
        {
            var go = new GameObject();
            go.AddComponent<JamScope>();
            PrefabUtility.SaveAsPrefabAsset(go, path);
            Object.DestroyImmediate(go);
        }
    }
}