using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniOwl.Celestials.Editor
{
    public static class EditorPlanetAssetUtils
    {
        private static readonly int s_normalMap = Shader.PropertyToID("_NormalMap");
        private static readonly int s_heightMap = Shader.PropertyToID("_HeightMap");

        private static readonly Dictionary<TextureFormat, TextureFormat> compressFormatMap = new()
        {
            { TextureFormat.R8, TextureFormat.BC4 },
            { TextureFormat.RG16, TextureFormat.BC5 },
            { TextureFormat.RGB24, TextureFormat.BC7 },
            { TextureFormat.RGBA32, TextureFormat.BC7 },
        };

        public static void SaveTextures(Object asset, GameObject editableGO)
        {
            Material[] materials = editableGO.GetComponentsInChildren<MeshRenderer>().Select(renderer => renderer.sharedMaterial).ToArray();

            for (int face = 0; face < 6; face++)
            {
                Material mat = materials[face];
                SaveTexture(asset, mat, s_normalMap);
                SaveTexture(asset, mat, s_heightMap);
            }
            AssetDatabase.SaveAssets();
        }

        private static void SaveTexture(Object asset, Material mat, int id)
        {
            Texture texture = mat.GetTexture(id);

            if (!texture || AssetDatabase.IsSubAsset(texture)) return;
            
            AssetDatabase.AddObjectToAsset(texture, asset);
        }
        
        public static void DestroyTextures(GameObject editableGO)
        {
            Material[] materials = editableGO.GetComponentsInChildren<MeshRenderer>().Select(renderer => renderer.sharedMaterial).ToArray();
            
            for (int face = 0; face < 6; face++)
            {
                Material mat = materials[face];
                
                DestroyTexture(mat, s_normalMap);
                DestroyTexture(mat, s_heightMap);
            }
        }
        
        private static void DestroyTexture(Material mat, int id)
        {
            Texture texture = mat.GetTexture(id);

            if (!texture) return;
            AssetDatabase.RemoveObjectFromAsset(texture);
            Object.DestroyImmediate(texture, true);
        }
        
        public static void CompressTextures(PlanetSurface surface, GameObject editableGO)
        {
            Material[] materials = editableGO.GetComponentsInChildren<MeshRenderer>().Select(renderer => renderer.sharedMaterial).ToArray();

            if (!surface.Textures.compression) return;
            
            for (int face = 0; face < 6; face++)
            {
                Material mat = materials[face];
                
                if (surface.Textures.generateNormals)
                    CompressTexture(surface, mat, s_normalMap, PlanetAssetUtils.DefaultNormalFormat);
                if (surface.Textures.generateHeights)
                    CompressTexture(surface, mat, s_heightMap, PlanetAssetUtils.DefaultHeightFormat);
            }
            AssetDatabase.SaveAssets();
        }
        
        private static void CompressTexture(PlanetSurface surface, Material mat, int id, TextureFormat format)
        {
            var texture = (Texture2D)mat.GetTexture(id);
            EditorUtility.CompressTexture(texture, compressFormatMap[format], surface.Textures._compressionQuality);
        }

        public static void SaveMeshes(Object asset, GameObject editableGO)
        {
            MeshFilter[] filters = editableGO.GetComponentsInChildren<MeshFilter>();
            
            for (int face = 0; face < 6; face++)
            {
                Mesh mesh = filters[face].sharedMesh;
                if (!mesh || AssetDatabase.IsSubAsset(mesh))
                    return;
                AssetDatabase.AddObjectToAsset(mesh, asset);
            }
        }

        public static void DestroyMeshes(GameObject editableGO)
        {
            MeshFilter[] filters = editableGO.GetComponentsInChildren<MeshFilter>();
            
            for (int face = 0; face < 6; face++)
            {
                Mesh mesh = filters[face].sharedMesh;
                AssetDatabase.RemoveObjectFromAsset(mesh);
                Object.DestroyImmediate(mesh, true);
            }
        }

        public static void ReinitializeTexturesIfNeeded(GameObject editableGO, int textureResolution)
        {
            Material[] materials = editableGO.GetComponentsInChildren<MeshRenderer>().Select(renderer => renderer.sharedMaterial).ToArray();
            
            for (int face = 0; face < 6; face++)
            {
                Material mat = materials[face];

                ReinitializeTextureIfNeeded(mat, s_normalMap, textureResolution, PlanetAssetUtils.DefaultNormalFormat);
                ReinitializeTextureIfNeeded(mat, s_heightMap, textureResolution, PlanetAssetUtils.DefaultHeightFormat);
            }
        }

        private static void ReinitializeTextureIfNeeded(Material mat, int id, int resolution, TextureFormat textureFormat)
        {
            var texture = (Texture2D)mat.GetTexture(id);

            if (texture.width != resolution || texture.height != resolution || texture.format != textureFormat)
                texture.Reinitialize(resolution, resolution, textureFormat, false);
        }
    }
}