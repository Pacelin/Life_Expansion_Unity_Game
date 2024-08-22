using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UniOwl.Celestials
{
    [BurstCompile]
    public struct BuildPlanetQuadVerticesJob : IJobFor
    {
        [WriteOnly]
        public NativeArray<Vertex> vertices;
        [ReadOnly]
        public NativeArray<float> heights;
        [ReadOnly]
        public NativeArray<float3> normals;

        public int ax1, ax2;
        public int resolution;
        public int resolutionPlus1;
        public float3 baseVertex;

        public float radius, amplitude;

        public void Execute(int vertexIndex)
        {
            int x = vertexIndex / resolutionPlus1, y = vertexIndex % resolutionPlus1;
            
            float3 unitPosition = baseVertex;
            unitPosition[ax1] += x;
            unitPosition[ax2] += y;

            unitPosition = MathUtils.GetSpherePosition(unitPosition, resolution);

            float3 vertex = unitPosition * (radius + amplitude * heights[vertexIndex]);
            float3 normal = normals[vertexIndex];
            half2 uv0 = new half2((half)((float)y / resolution), (half)((float)x / resolution));
            
            vertices[vertexIndex] = new Vertex()
            {
                position = vertex,
                normal = normal,
                uv0 = uv0,
            };
        }
    }
}
