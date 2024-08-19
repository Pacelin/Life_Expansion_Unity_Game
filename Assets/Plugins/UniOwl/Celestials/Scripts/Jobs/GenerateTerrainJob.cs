using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UniOwl.Celestials
{
    [BurstCompile]
    public struct GenerateTerrainJob : IJobFor
    {
        public int resolution, resolutionPlus1;

        public int ax1, ax2;

        public float3 baseVertex;

        public TerrainGeneratorSettings settings;
        
        [WriteOnly]
        public NativeArray<float> heights;
        [WriteOnly]
        public NativeArray<float3> normals;
    
        public void Execute(int index)
        {
            int x = index / resolutionPlus1, y = index % resolutionPlus1;
            
            float3 vertex = baseVertex;
            vertex[ax1] += x;
            vertex[ax2] += y;

            vertex = MathUtils.GetSpherePosition(vertex, resolution);

            var density = settings.Evaluate(vertex);
            var height = density.value;
            float3 normal = density.CalculateNormal(new float3(0f, 1f, 0f));

            heights[index] = height;
            normals[index] = normal;
        }
    }
}