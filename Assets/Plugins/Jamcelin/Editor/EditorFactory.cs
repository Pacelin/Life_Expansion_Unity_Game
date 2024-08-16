using System.IO;
using UnityEditor;

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
    }
}