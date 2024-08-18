using System.IO;
using UnityEditor;
using UnityEngine;

namespace UniOwl.Celestials.Editor
{
    public static class PlanetAssetCreator
    {
        private static readonly int s_mainMap = Shader.PropertyToID("_MainMap");
        private static readonly int s_normalMap = Shader.PropertyToID("_NormalMap");

        private const string PrefabPath = "Assets/Plugins/UniOwl/Celestials/Planet_Blank.prefab";
        private const string ShaderPath = "Assets/Plugins/UniOwl/Celestials/SG_Terrain.shadergraph";
        
        public static void CreateAssets(PlanetSettings settings, string path)
        {
            var planetFolder = CreatePlanetFolder(path);
            
            if (planetFolder == null)
                return;
            
            var diffuse = CreateTextures(settings, planetFolder, false);
            var normals = CreateTextures(settings, planetFolder, true);

            var materials = CreateMaterials(planetFolder, diffuse, normals);
            var meshes = CreateMeshes(planetFolder);

            var prefab = CreatePrefabVariant(settings, planetFolder, meshes, materials);
        }

        private static string CreatePlanetFolder(string path)
        {
            var folderName = Path.GetFileName(path);
            var folderPath = Path.GetDirectoryName(path);
            
            AssetDatabase.CreateFolder(folderPath, folderName);
            AssetDatabase.Refresh();

            return path;
        }

        private static Mesh[] CreateMeshes(string folderPath)
        {
            var folderName = Path.GetFileName(folderPath);

            Mesh[] meshes = new Mesh[6];

            var blankMesh = new Mesh();
            
            for (int i = 0; i < 6; i++)
            {
                var mesh = SaveUtility.SaveMesh(blankMesh, $"SM_{folderName}_{i}.asset", true, folderPath);
                meshes[i] = mesh;
            }

            return meshes;
        }

        private static Texture2D[] CreateTextures(PlanetSettings settings, string folderPath, bool normal)
        {
            if (!normal && !settings.Textures.generateTextures)
                return null;
            if (normal && !settings.Textures.generateNormals)
                return null;
            
            var folderName = Path.GetFileName(folderPath);

            var textures = new Texture2D[6];
            
            for (int i = 0; i < 6; i++)
            {
                var texture = new Texture2D(settings.Textures.resolution, settings.Textures.resolution, TextureFormat.RGB24, false, true, true)
                {
                    filterMode = FilterMode.Point,
                };

                textures[i] = texture;
                
                string suffix = normal ? "N" : "D";
                SaveUtility.SaveTexture(texture, $"T_{folderName}_{i}_{suffix}.asset", folderPath);
            }

            return textures;
        }

        private static Material[] CreateMaterials(string folderPath, Texture2D[] diffuse, Texture2D[] normals)
        {
            var shader = AssetDatabase.LoadAssetAtPath<Shader>(ShaderPath);
            
            var original = new Material(shader);
            
            var folderName = Path.GetFileName(folderPath);

            SaveUtility.SaveMaterial(original, $"M_{folderName}.mat", folderPath);

            var materials = new Material[6];
            
            for (int i = 0; i < 6; i++)
            {
                var material = new Material(original)
                {
                    parent = original,
                };

                materials[i] = material;
                
                if (diffuse != null)
                    material.SetTexture(s_mainMap, diffuse[i]);
                
                if (normals != null)
                    material.SetTexture(s_normalMap, normals[i]);
                
                SaveUtility.SaveMaterial(material, $"MV_{folderName}_{i}.mat", folderPath);
            }

            return materials;
        }

        private static Planet CreatePrefabVariant(PlanetSettings settings, string folderPath, Mesh[] meshes, Material[] materials)
        {
            var folderName = Path.GetFileName(folderPath);

            var name = $"{folderName}.prefab";
            var path = Path.Combine(folderPath, name);
            
            var prefabOriginal = AssetDatabase.LoadAssetAtPath<Planet>(PrefabPath);
            var prefabSource = (Planet)PrefabUtility.InstantiatePrefab(prefabOriginal);
            GameObject obj = PrefabUtility.SaveAsPrefabAsset(prefabSource.gameObject, path);

            var planet = obj.GetComponent<Planet>();
            settings.Planet = planet;

            for (int i = 0; i < 6; i++)
            {
                planet.Faces[i].Filter.sharedMesh = meshes[i];
                planet.Faces[i].Renderer.sharedMaterial = materials[i];
            }
            
            AssetDatabase.SaveAssets();
            
            return planet;
        }
    }
}