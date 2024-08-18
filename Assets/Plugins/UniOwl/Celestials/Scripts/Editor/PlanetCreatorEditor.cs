using System.IO;
using UnityEditor;

namespace UniOwl.Celestials.Editor
{
    public static class PlanetCreatorEditor
    {
        public static void CreatePlanet(PlanetSettings settings)
        {
            if (!settings.Planet)
            {
                string settingsPath = AssetDatabase.GetAssetPath(settings);
                string folderName = settings.name.Substring(3, settings.name.Length - 3);
                string folderPath = Path.Combine(settingsPath, folderName);

                string path = EditorUtility.SaveFilePanelInProject("Create new planet", folderName, "", "", settingsPath);
            
                if (!Directory.Exists(folderPath) && !string.IsNullOrEmpty(path))    
                    PlanetAssetCreator.CreateAssets(settings, path);
            }

            PlanetCreator.CreatePlanet(settings);
        }
    }
}
