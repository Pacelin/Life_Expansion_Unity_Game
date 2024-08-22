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
        public float3 offset;

        public TerrainGeneratorSettings settings;
        public float radius, amplitude;
        
        [WriteOnly]
        public NativeArray<float> heights;
        [WriteOnly]
        public NativeArray<float3> normals;

        public void Execute(int index)
        {
            const float k = 0.25f; // 1f / math.TAU;
            
            int x = index / resolutionPlus1, y = index % resolutionPlus1;
            
            float3 vertex = baseVertex;
            vertex[ax1] += x;
            vertex[ax2] += y;

            vertex = MathUtils.GetSpherePosition(vertex, resolution);

            var noise = settings.Evaluate(vertex + offset);
            var height = noise.value;
            float3 normal = noise.CalculateNormal(vertex, k * amplitude / radius);

            heights[index] = height;
            normals[index] = normal;
        }
    }
}