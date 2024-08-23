using System.IO;
using UnityEditor;

namespace UniOwl.Celestials.Editor
{
    public static class PlanetCreatorEditor
    {
        public static void CreatePlanet(PlanetSettings settings)
        {
            string path = null;
            
            if (!settings.Planet)
            {
                string settingsPath = AssetDatabase.GetAssetPath(settings);
                string folderName = settings.name.Substring(3, settings.name.Length - 3);
                string folderPath = Path.Combine(settingsPath, folderName);

                path = EditorUtility.SaveFilePanelInProject("Create new planet", folderName, "", "", settingsPath);
            
                if (!Directory.Exists(folderPath) && !string.IsNullOrEmpty(path))    
                    PlanetAssetCreator.CreateAssets(settings, path);
            }

            if (!settings.Planet && string.IsNullOrEmpty(path)) return;

            try
            {
                PlanetCreator.progressUpdated += UpdateProgress;
                PlanetCreator.CreatePlanet(settings);
            }
            finally
            {
                PlanetCreator.progressUpdated -= UpdateProgress;
                EditorUtility.ClearProgressBar();
            }

            PrefabUtility.SavePrefabAsset(settings.Planet.gameObject);
        }

        private static void UpdateProgress(string stage, string description)
        {
            EditorUtility.DisplayProgressBar(stage, description, -1f);
        }
    }
}
