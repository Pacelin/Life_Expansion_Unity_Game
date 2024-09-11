using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace UniOwl.Celestials
{
    public static class PlanetSurfaceGenerator
    {
        public static void GeneratePlanetTerrain(PlanetSurface surface, GameObject surfaceGO)
        {
            GeneratePlanetTerrain(surface, surfaceGO, surface.Model.resolution, surface.Textures.resolution - 1);
        }

        public static void GeneratePlanetTerrain(PlanetSurface surface, GameObject surfaceGO, int meshResolution, int textureResolution)
        {
            var planet = (PlanetObject)surface.List;
            
            PlanetProgressReporter.ReportStage("Generate Terrain (Mesh)");
            var meshHeightData = GenerateTerrain(meshResolution, planet.Physical, surface);
            PlanetProgressReporter.ReportStage("Generate Terrain (Texture)");
            var textureHeightData = GenerateTerrain(textureResolution, planet.Physical, surface);
            
            PlanetSurfaceMeshBuilder.BuildMeshes(surface, surfaceGO, meshHeightData, meshResolution);
            PlanetSurfaceTextureBuilder.BuildTextures(surface, surfaceGO, textureHeightData);
            
            PlanetProgressReporter.ReportStage("Complete");
            meshHeightData.Dispose();
            textureHeightData.Dispose();
        }
        
        private static PlanetHeightData GenerateTerrain(int resolution, PhysicalSettings physical, PlanetSurface surface)
        {
            int size = (resolution + 1) * (resolution + 1);

            PlanetHeightData heightData = new()
            {
                heights = new NativeArray<float>[6],
                normals = new NativeArray<float3>[6],
            };
            
            for (int face = 0; face < 6; face++)
            {
                PlanetProgressReporter.ReportDescription($"Face {face + 1} of 6");
                
                NativeArray<float> heights = new NativeArray<float>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                NativeArray<float3> normals = new NativeArray<float3>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                
                GenerateQuadTerrain(face, resolution, heights, normals, physical, surface);

                heightData.heights[face] = heights;
                heightData.normals[face] = normals;
            }

            return heightData;
        }
        
        private static void GenerateQuadTerrain(int face, int resolution, in NativeArray<float> heights, in NativeArray<float3> normals, PhysicalSettings physical, PlanetSurface surface)
        {
            int dir = face % 3;
            int ax1 = (face + 1) % 3;
            int ax2 = (face + 2) % 3;

            bool backFace = face > 2;
            
            float3 baseVertex = float3.zero;
            baseVertex[dir] = math.select(0, resolution, backFace);

            uint seed = surface.Generation.seed;
            Random rnd = new(seed);
            float3 offset = rnd.NextFloat3() * 1125.123f;
            
            JobHandle buildTerrainJob = new GenerateTerrainJob
            {
                resolution = resolution,
                resolutionPlus1 = resolution + 1,

                ax1 = ax1, ax2 = ax2,
                baseVertex = baseVertex,
                offset = offset,

                heights = heights,
                normals = normals,
                
                settings = surface.Generation,
                radius = physical.radius,
                amplitude = physical.amplitude,
            }.ScheduleParallel(heights.Length, 0, default);
            buildTerrainJob.Complete();
        }
    }
}