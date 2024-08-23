using System.IO;
using UnityEditor;
using UnityEngine;

namespace UniOwl.Celestials.Editor
{
    public static class PlanetAssetCreator
    {
        private static readonly int s_mainMap = Shader.PropertyToID("_MainMap");
        private static readonly int s_normalMap = Shader.PropertyToID("_NormalMap");
        private static readonly int s_heightMap = Shader.PropertyToID("_HeightMap");

        private const string PrefabPath = "Assets/Plugins/UniOwl/Celestials/Prefabs/Planet_Blank.prefab";
        private const string MaterialPath = "Assets/Plugins/UniOwl/Celestials/Materials/M_Celestials_PlanetTerrain.mat";
        
        public static void CreateAssets(PlanetSettings settings, string path)
        {
            var planetFolder = CreatePlanetFolder(path);
            
            if (planetFolder == null)
                return;

            Texture2D[] diffuse = null, normals = null, heights = null;
            if (settings.Textures.generateColors)
                diffuse = CreateTextures(settings, planetFolder, "D", TextureFormat.RGB24);
            if (settings.Textures.generateNormals)
                normals = CreateTextures(settings, planetFolder, "N", TextureFormat.RGB24);
            if (settings.Textures.generateHeights)
                heights = CreateTextures(settings, planetFolder, "H", TextureFormat.R8);

            var materials = CreateMaterials(planetFolder, diffuse, normals, heights);
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

        private static Texture2D[] CreateTextures(PlanetSettings settings, string folderPath, string suffix, TextureFormat format)
        {
            var folderName = Path.GetFileName(folderPath);

            var textures = new Texture2D[6];
            
            for (int i = 0; i < 6; i++)
            {
                var texture = new Texture2D(settings.Textures.resolution, settings.Textures.resolution, format, false, true, true)
                {
                    filterMode = FilterMode.Point,
                    wrapMode = TextureWrapMode.Clamp,
                };
                
                textures[i] = texture;
                
                SaveUtility.SaveTexture(texture, $"T_{folderName}_{i}_{suffix}.asset", folderPath);
            }

            return textures;
        }

        private static Material[] CreateMaterials(string folderPath, Texture2D[] diffuse, Texture2D[] normals, Texture2D[] heights)
        {
            var parent = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
            var original = new Material(parent);
            
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
                
                if (heights != null)
                    material.SetTexture(s_heightMap, heights[i]);
                
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
            planet.settings = settings;

            for (int i = 0; i < 6; i++)
            {
                planet.SurfaceFaces[i].Filter.sharedMesh = meshes[i];
                planet.SurfaceFaces[i].Renderer.sharedMaterial = materials[i];
            }
            
            MakeChildMaterial(planet.SeaRenderer, folderName, folderPath);
            MakeChildMaterial(planet.AtmosphereRenderer, folderName, folderPath);
            MakeChildMaterial(planet.CloudsRenderer, folderName, folderPath);
            MakeChildMaterial(planet.RingsRenderer, folderName, folderPath);
            
            AssetDatabase.SaveAssets();
            
            return planet;
        }

        private static void MakeChildMaterial(MeshRenderer renderer, string planetName, string folderPath)
        {
            var mat = renderer.sharedMaterial;
            mat = new Material(mat);
            renderer.sharedMaterial = mat;
            
            SaveUtility.SaveMaterial(mat, $"M_{planetName}_{mat.name.Substring(2, mat.name.Length - 2)}.mat", folderPath);
        }
    }
}