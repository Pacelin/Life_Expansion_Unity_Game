﻿using Unity.Burst;
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

        public static void BuildTextures(PlanetSettings settings, Planet planet, PlanetHeightData heightData)
        {
            for (int face = 0; face < 6; face++)
            {
                var heights = heightData.heights[face];
                var normals = heightData.normals[face];
                
                BuildTexture(settings, face, heights, normals);
            }
        }

        private static void BuildTexture(PlanetSettings settings, int face, in NativeArray<float> heights, in NativeArray<float3> normals)
        {
            var material = settings.Planet.Faces[face].Renderer.sharedMaterial;
              
            if (settings.Textures.generateTextures)
                BuildMainTexture(settings, material, heights, normals);
            if (settings.Textures.generateNormals)
                BuildNormalTexture(settings, material, heights, normals);
        }

        private static void BuildMainTexture(PlanetSettings settings, Material material, in NativeArray<float> heights, in NativeArray<float3> normals)
        {
            var colors = new NativeArray<byte3>(heights.Length, Allocator.TempJob,
                NativeArrayOptions.UninitializedMemory);

            var colorJob = new MakeColorsJob()
            {
                heights = heights,
                normals = normals,
                mainColors = colors,

                rockColor = settings.RockColor.ToFloat4(),
                grassColor = settings.GrassColor.ToFloat4(),
            }.ScheduleParallel(colors.Length, 0, default);
            colorJob.Complete();
            
            ApplyTexture(material, s_mainMap, colors);
            colors.Dispose();
        }

        private static void BuildNormalTexture(PlanetSettings settings, Material material, in NativeArray<float> heights, in NativeArray<float3> normals)
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

        private static void ApplyTexture(Material material, int textureID, in NativeArray<byte3> colors)
        {
            var texture = (Texture2D)material.GetTexture(textureID);

            texture.SetPixelData(colors, 0, 0);
            texture.Apply(false);
            texture.Compress(true);
        }

        [BurstCompile]
        public struct MakeNormalsJob : IJobFor
        {
            [ReadOnly]
            public NativeArray<float3> normals;
            [WriteOnly]
            public NativeArray<byte3> normalColors;
            
            public void Execute(int index)
            {
                float4 normal = new float4(normals[index], 1f);
                normalColors[index] = MathUtils.Float4ToRGB24(normal);
            }
        }

        [BurstCompile]
        public struct MakeColorsJob : IJobFor
        {
            [ReadOnly]
            public NativeArray<float> heights;
            [ReadOnly]
            public NativeArray<float3> normals;
            [WriteOnly]
            public NativeArray<byte3> mainColors;
            
            
            public float4 rockColor, grassColor;
            
            public void Execute(int index)
            {
                float4 color = math.lerp(rockColor, grassColor, math.saturate(math.dot(normals[index], new float3(0f, 1f, 0f))));
                mainColors[index] = MathUtils.Float4ToRGB24(color);
            }
        }
    }
}