using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

using Random = Unity.Mathematics.Random;

namespace UniOwl.Celestials
{
    public static class PlanetCreator
    {
        public static event Action<string, string> progressUpdated;

        private static string stage;
        private static string description;
        
        public static void CreatePlanet(PlanetSettings settings)
        {
            GeneratePlanetTerrain(settings);
            UpdateSea(settings);
        }
        
        public static void ReportStage(string stage)
        {
            PlanetCreator.stage = stage;
            progressUpdated?.Invoke(stage, description);
        }

        public static void ReportDescription(string description)
        {
            PlanetCreator.description = description;
            progressUpdated?.Invoke(stage, description);
        }

        private static void GeneratePlanetTerrain(PlanetSettings settings)
        {
            ReportStage("Generate Terrain (Mesh)");
            var meshHeightData = GenerateTerrain(settings.Model.resolution, settings);
            ReportStage("Generate Terrain (Texture)");
            var textureHeightData = GenerateTerrain(settings.Textures.resolution - 1, settings);
            
            PlanetMeshBuilder.BuildMeshes(settings, meshHeightData);
            PlanetTextureBuilder.BuildTextures(settings, textureHeightData);
            
            ReportStage("Complete");
            meshHeightData.Dispose();
            textureHeightData.Dispose();
        }

        private static void UpdateSea(PlanetSettings settings)
        {
            ReportStage("Update Sea");
            settings.Planet.SeaTransform.localScale = 2f * (settings.Physical.radius + settings.Physical.seaLevel * settings.Physical.amplitude) * Vector3.one;
        }
        
        private static PlanetHeightData GenerateTerrain(int resolution, PlanetSettings settings)
        {
            int size = (resolution + 1) * (resolution + 1);

            PlanetHeightData heightData = new()
            {
                heights = new NativeArray<float>[6],
                normals = new NativeArray<float3>[6],
            };
            
            for (int face = 0; face < 6; face++)
            {
                ReportDescription($"Face {face + 1} of 6");
                
                NativeArray<float> heights = new NativeArray<float>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                NativeArray<float3> normals = new NativeArray<float3>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                
                GenerateQuadTerrain(face, resolution, heights, normals, settings);

                heightData.heights[face] = heights;
                heightData.normals[face] = normals;
            }

            return heightData;
        }
        
        private static void GenerateQuadTerrain(int face, int resolution, in NativeArray<float> heights, in NativeArray<float3> normals, PlanetSettings settings)
        {
            int dir = face % 3;
            int ax1 = (face + 1) % 3;
            int ax2 = (face + 2) % 3;

            bool backFace = face > 2;
            
            float3 baseVertex = float3.zero;
            baseVertex[dir] = math.select(0, resolution, backFace);

            uint seed = settings.Generation.seed;
            Random rnd = new(seed);
            float3 offset = rnd.NextFloat3() * 1125.123f;
            
            JobHandle buildTerrainJob = new GenerateTerrainJob()
            {
                resolution = resolution,
                resolutionPlus1 = resolution + 1,

                ax1 = ax1, ax2 = ax2,
                baseVertex = baseVertex,
                offset = offset,

                heights = heights,
                normals = normals,
                
                settings = settings.Generation,
                radius = settings.Physical.radius,
                amplitude = settings.Physical.amplitude,
            }.ScheduleParallel(heights.Length, 0, default);
            buildTerrainJob.Complete();
        }
    }
}
