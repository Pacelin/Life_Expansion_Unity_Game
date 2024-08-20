using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace UniOwl.Celestials
{
    public static class PlanetCreator
    {
        private static readonly int s_radius = Shader.PropertyToID("_Radius");
        private static readonly int s_amplitude = Shader.PropertyToID("_Amplitude");

        public static void CreatePlanet(PlanetSettings settings)
        {
            GeneratePlanetTerrain(settings);
            
            settings.Planet.SeaTransform.localScale = 2f * (settings.Physical.radius + settings.Physical.seaLevel * settings.Physical.amplitude) * Vector3.one;
            UpdateMaterials(settings);
        }

        private static void GeneratePlanetTerrain(PlanetSettings settings)
        {
            var meshHeightData = GenerateTerrain(settings.Model.resolution, settings.Generation);
            var textureHeightData = GenerateTerrain(settings.Textures.resolution - 1, settings.Generation);

            PlanetMeshBuilder.BuildMeshes(settings, settings.Planet, meshHeightData);
            PlanetTextureBuilder.BuildTextures(settings, settings.Planet, textureHeightData);
            
            meshHeightData.Dispose();
            textureHeightData.Dispose();
        }
        
        private static PlanetHeightData GenerateTerrain(int resolution, in TerrainGeneratorSettings settings)
        {
            int size = (resolution + 1) * (resolution + 1);

            PlanetHeightData heightData = new()
            {
                heights = new NativeArray<float>[6],
                normals = new NativeArray<float3>[6],
            };
            
            for (int face = 0; face < 6; face++)
            {
                NativeArray<float> heights = new NativeArray<float>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                NativeArray<float3> normals = new NativeArray<float3>(size, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
                
                GenerateQuadTerrain(face, resolution, heights, normals, settings);

                heightData.heights[face] = heights;
                heightData.normals[face] = normals;
            }

            return heightData;
        }
        
        private static void GenerateQuadTerrain(int face, int resolution, in NativeArray<float> heights, in NativeArray<float3> normals, in TerrainGeneratorSettings settings)
        {
            int dir = face % 3;
            int ax1 = (face + 1) % 3;
            int ax2 = (face + 2) % 3;

            bool backFace = face > 2;
            
            float3 baseVertex = float3.zero;
            baseVertex[dir] = math.select(0, resolution, backFace);
            
            JobHandle buildTerrainJob = new GenerateTerrainJob()
            {
                resolution = resolution,
                resolutionPlus1 = resolution + 1,

                ax1 = ax1, ax2 = ax2,
                baseVertex = baseVertex,

                heights = heights,
                normals = normals,
                
                settings = settings,
            }.ScheduleParallel(heights.Length, 0, default);
            buildTerrainJob.Complete();
        }

        private static void UpdateMaterials(PlanetSettings settings)
        {
            Material parentMat = settings.Planet.SurfaceFaces[0].Renderer.sharedMaterial.parent;
            parentMat.SetFloat(s_radius, settings.Physical.radius);
            parentMat.SetFloat(s_amplitude, settings.Physical.amplitude);
        }
    }
}
