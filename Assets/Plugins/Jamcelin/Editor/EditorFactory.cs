using System.IO;
using Jamcelin.Runtime.Core;
using UnityEditor;
using UnityEngine;

namespace Jamcelin.Editor
{
    public class EditorFactory
    {
        [MenuItem("Assets/Create/Jamcelin/Scope")]
        private static void CreateScopeFolder()
        {
            var selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
            var choosedPath = EditorUtility.SaveFilePanelInProject(
                "Create Scope Folder", "Scope Folder", "", "", selectedPath);
            if (string.IsNullOrEmpty(choosedPath))
                return;
            var folderName = Path.GetFileName(choosedPath);
            var folderPath = Path.GetDirectoryName(choosedPath);
            
            AssetDatabase.CreateFolder(folderPath, folderName);
            if (folderName.Contains("Scope"))
                EditorExtensions.MarkAddressable(AssetDatabase.AssetPathToGUID(choosedPath), folderName, true);
            else
                EditorExtensions.MarkAddressable(AssetDatabase.AssetPathToGUID(choosedPath), folderName + " Scope", true);
            
            AssetDatabase.Refresh();
        }

        [MenuItem("GameObject/Jamcelin/Scene Context")]
        private static void CreateSceneContext()
        {
            var go = new GameObject();
            go.name = "Scene Context";
            if (Selection.activeTransform != null)
                go.transform.parent = Selection.activeTransform;
            go.AddComponent<JamSceneContext>();
        }
        
        [MenuItem("GameObject/Jamcelin/Game Object Context")]
        private static void CreateGameObjectContext()
        {
            var go = new GameObject();
            go.name = "Game Object Context";
            if (Selection.activeTransform != null)
                go.transform.parent = Selection.activeTransform;
            go.AddComponent<JamGameObjectContext>();
        }
    }
}