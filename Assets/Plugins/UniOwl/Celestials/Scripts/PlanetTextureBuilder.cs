using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace UniOwl.Celestials
{
    public static class PlanetTextureBuilder
    {
        private static readonly int s_mainMap = Shader.PropertyToID("_MainMap");
        private static readonly int s_normalMap = Shader.PropertyToID("_NormalMap");
        private static readonly int s_heightMap = Shader.PropertyToID("_HeightMap");

        public static void BuildTextures(PlanetSettings settings, in PlanetHeightData heightData)
        {
            BuildColorTextures(settings,heightData, settings.Textures.compression);
            BuildHeightTextures(settings, heightData, settings.Textures.compression);
            BuildNormalTextures(settings, heightData, settings.Textures.compression);
        }
        
        private static void BuildColorTextures(PlanetSettings settings, in PlanetHeightData heightData, bool compress)
        {
            if (!settings.Textures.generateColors) return;
            PlanetCreator.ReportStage("Build Textures (Color)");
            
            for (int face = 0; face < 6; face++)
            {
                PlanetCreator.ReportDescription($"Face {face + 1} of 6");
                
                Material material = settings.Planet.SurfaceFaces[face].Renderer.sharedMaterial;
                NativeArray<float> heights = heightData.heights[face];
                NativeArray<float3> normals = heightData.normals[face];
                
                BuildColorTexture(settings, material, heights, normals, compress);
            }
        }
        
        private static void BuildHeightTextures(PlanetSettings settings, in PlanetHeightData heightData, bool compress)
        {
            if (!settings.Textures.generateHeights) return;
            PlanetCreator.ReportStage("Build Textures (Height)");
            
            for (int face = 0; face < 6; face++)
            {
                PlanetCreator.ReportDescription($"Face {face + 1} of 6");
                
                Material material = settings.Planet.SurfaceFaces[face].Renderer.sharedMaterial;
                NativeArray<float> heights = heightData.heights[face];
                
                BuildHeightTexture(material, heights, compress);
            }
        }
        
        private static void BuildNormalTextures(PlanetSettings settings, in PlanetHeightData heightData, bool compress)
        {
            if (!settings.Textures.generateNormals) return;
            PlanetCreator.ReportStage("Build Textures (Normal)");
            
            for (int face = 0; face < 6; face++)
            {
                PlanetCreator.ReportDescription($"Face {face + 1} of 6");
                
                Material material = settings.Planet.SurfaceFaces[face].Renderer.sharedMaterial;
                NativeArray<float3> normals = heightData.normals[face];
                
                BuildNormalTexture(material, normals, compress);
            }
        }
        
        private static void BuildColorTexture(PlanetSettings settings, Material material, in NativeArray<float> heights, in NativeArray<float3> normals, bool compress)
        {
            var colors = new NativeArray<byte3>(heights.Length, Allocator.TempJob,
                NativeArrayOptions.UninitializedMemory);

            var colorJob = new MakeColorsJob()
            {
                heights = heights,
                normals = normals,
                mainColors = colors,

                rockColor = settings.RockColor.ToFloat3(),
                grassColor = settings.GrassColor.ToFloat3(),
            }.ScheduleParallel(colors.Length, 0, default);
            colorJob.Complete();
            
            ApplyTexture(material, s_mainMap, colors, compress);
            colors.Dispose();
        }

        private static void BuildHeightTexture(Material material, in NativeArray<float> heights, bool compress)
        {
            var heightColors = new NativeArray<byte>(heights.Length, Allocator.TempJob,
                NativeArrayOptions.UninitializedMemory);
            
            var heightJob = new MakeHeightJob()
            {
                heights = heights,
                heightColors = heightColors,
            }.ScheduleParallel(heights.Length, 0, default);
            heightJob.Complete();
            
            ApplyTexture(material, s_heightMap, heightColors, compress);
            heightColors.Dispose();
        }

        private static void BuildNormalTexture(Material material, in NativeArray<float3> normals, bool compress)
        {
            var normalColors = new NativeArray<byte3>(normals.Length, Allocator.TempJob,
                NativeArrayOptions.UninitializedMemory);
            
            var normalJob = new MakeNormalsJob()
            {
                normals = normals,
                normalColors = normalColors,
            }.ScheduleParallel(normals.Length, 0, default);
            normalJob.Complete();
            
            ApplyTexture(material, s_normalMap, normalColors, compress);
            normalColors.Dispose();
        }

        private static void ApplyTexture<T>(Material material, int textureID, in NativeArray<T> colors, bool compress) where T : unmanaged
        {
            PlanetCreator.ReportDescription("ApplyTexture()");
            var texture = (Texture2D)material.GetTexture(textureID);

            texture.SetPixelData(colors, 0, 0);
            
            if (compress)
                texture.Compress(true);
            
            texture.Apply(false, true);
        }

        [BurstCompile]
        private struct MakeColorsJob : IJobFor
        {
            [ReadOnly]
            public NativeArray<float> heights;
            [ReadOnly]
            public NativeArray<float3> normals;
            [WriteOnly]
            public NativeArray<byte3> mainColors;
            
            public float3 rockColor, grassColor;
            
            public void Execute(int index)
            {
                float3 color = math.lerp(rockColor, grassColor, math.saturate(math.dot(normals[index], new float3(0f, 1f, 0f))));
                mainColors[index] = MathUtils.Float3ToRGB24(color);
            }
        }
        
        [BurstCompile]
        private struct MakeHeightJob : IJobFor
        {
            [ReadOnly]
            public NativeArray<float> heights;
            [WriteOnly]
            public NativeArray<byte> heightColors;

            public void Execute(int index)
            {
                float normalizedHeight = math.saturate(heights[index]) * 255f;
                heightColors[index] = (byte)normalizedHeight;
            }
        }
        
        [BurstCompile]
        private struct MakeNormalsJob : IJobFor
        {
            [ReadOnly]
            public NativeArray<float3> normals;
            [WriteOnly]
            public NativeArray<byte3> normalColors;
            
            public void Execute(int index)
            {
                normalColors[index] = MathUtils.Float3ToRGB24((normals[index] + 1f) * 0.5f);
            }
        }
    }
}