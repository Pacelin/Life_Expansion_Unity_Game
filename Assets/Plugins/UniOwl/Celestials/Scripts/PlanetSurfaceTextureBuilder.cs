using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace UniOwl.Celestials
{
    public static class PlanetSurfaceTextureBuilder
    {
        private static readonly int s_normalMap = Shader.PropertyToID("_NormalMap");
        private static readonly int s_heightMap = Shader.PropertyToID("_HeightMap");

        public static void BuildTextures(PlanetSurface surface, GameObject surfaceGO, in PlanetHeightData heightData)
        {
            var sharedMaterials = surfaceGO.GetComponentsInChildren<MeshRenderer>().Select(renderer => renderer.sharedMaterial).ToArray();
            
            BuildHeightTextures(surface, sharedMaterials, heightData);
            BuildNormalTextures(surface, sharedMaterials, heightData);
        }
        
        private static void BuildHeightTextures(PlanetSurface surface, Material[] materials, in PlanetHeightData heightData)
        {
            if (!surface.Textures.generateHeights) return;
            PlanetProgressReporter.ReportStage("Build Textures (Height)");
            
            for (int face = 0; face < 6; face++)
            {
                PlanetProgressReporter.ReportDescription($"Face {face + 1} of 6");
                
                Material material = materials[face];
                NativeArray<float> heights = heightData.heights[face];
                
                BuildHeightTexture(material, heights);
            }
        }
        
        private static void BuildNormalTextures(PlanetSurface surface, Material[] materials, in PlanetHeightData heightData)
        {
            if (!surface.Textures.generateNormals) return;
            PlanetProgressReporter.ReportStage("Build Textures (Normal)");
            
            for (int face = 0; face < 6; face++)
            {
                PlanetProgressReporter.ReportDescription($"Face {face + 1} of 6");
                
                Material material = materials[face];
                NativeArray<float3> normals = heightData.normals[face];
                
                BuildNormalTexture(material, normals);
            }
        }
        
        private static void BuildHeightTexture(Material material, in NativeArray<float> heights)
        {
            var heightColors = new NativeArray<byte>(heights.Length, Allocator.TempJob,
                NativeArrayOptions.UninitializedMemory);
            
            var heightJob = new MakeHeightJob()
            {
                heights = heights,
                heightColors = heightColors,
            }.ScheduleParallel(heights.Length, 0, default);
            heightJob.Complete();
            
            ApplyTexture(material, s_heightMap, heightColors);
            heightColors.Dispose();
        }

        private static void BuildNormalTexture(Material material, in NativeArray<float3> normals)
        {
            var normalColors = new NativeArray<byte3>(normals.Length, Allocator.TempJob,
                NativeArrayOptions.UninitializedMemory);
            
            var normalJob = new MakeNormalsJob()
            {
                normals = normals,
                normalColors = normalColors,
            }.ScheduleParallel(normals.Length, 0, default);
            normalJob.Complete();
            
            ApplyTexture(material, s_normalMap, normalColors);
            normalColors.Dispose();
        }
        
        private static void ApplyTexture<T>(Material material, int textureID, in NativeArray<T> colors) where T : unmanaged
        {
            PlanetProgressReporter.ReportDescription("ApplyTexture()");
            var texture = (Texture2D)material.GetTexture(textureID);

            texture.SetPixelData(colors, 0, 0);
            texture.Apply(false, true);
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