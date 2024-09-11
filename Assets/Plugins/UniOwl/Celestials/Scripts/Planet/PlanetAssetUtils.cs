using System.Linq;
using UnityEngine;

namespace UniOwl.Celestials
{
    public static class PlanetAssetUtils
    {
        private static readonly int s_normalMap = Shader.PropertyToID("_NormalMap");
        private static readonly int s_heightMap = Shader.PropertyToID("_HeightMap");
        
        public const TextureFormat DefaultNormalFormat = TextureFormat.RGB24;
        public const TextureFormat DefaultHeightFormat = TextureFormat.R8;

        public static void CreateTextures(PlanetSurface surface, GameObject editableGO)
        {
            Material[] materials = editableGO.GetComponentsInChildren<MeshRenderer>().Select(renderer => renderer.sharedMaterial).ToArray();
            
            for (int face = 0; face < 6; face++)
            {
                Material mat = materials[face];
                
                if (surface.Textures.generateNormals)
                    CreateTexture(surface, mat, s_normalMap, face, true);
                if (surface.Textures.generateHeights)
                    CreateTexture(surface, mat, s_heightMap, face, false);
            }
        }
        
        private static void CreateTexture(PlanetSurface surface, Material mat, int id, int face, bool isNormalMap)
        {
            TextureFormat format = isNormalMap ? DefaultNormalFormat : DefaultHeightFormat;
            string prefix = surface.List.name.StartsWith("SO_") ? surface.List.name[3..] : surface.List.name;

            var texture = new Texture2D(
                surface.Textures.resolution,
                surface.Textures.resolution,
                format,
                false,
                true,
                true)
            {
                hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector,
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp,
                name = $"T_{prefix}_{face}_{(isNormalMap ? "N" : "H")}",
            };
            mat.SetTexture(id, texture);
        }

        public static void CreateMeshes(PlanetSurface surface, GameObject editableGO)
        {
            MeshFilter[] filters = editableGO.GetComponentsInChildren<MeshFilter>();
            string prefix = surface.List.name.StartsWith("SO_") ? surface.List.name[3..] : surface.List.name;
            
            for (int face = 0; face < 6; face++)
            {
                var mesh = new Mesh
                {
                    name = $"SM_{prefix}_{face}",
                };

                filters[face].sharedMesh = mesh;
            }
        }
    }
}