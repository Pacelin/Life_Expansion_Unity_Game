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

        public void Execute(int vertexIndex)
        {
            int x = vertexIndex / resolutionPlus1, y = vertexIndex % resolutionPlus1;
            
            float3 vertex = baseVertex;
            vertex[ax1] += x;
            vertex[ax2] += y;

            vertex = MathUtils.GetSpherePosition(vertex, resolution);

            vertices[vertexIndex] = new Vertex()
            {
                position = vertex * heights[vertexIndex],
                normal = (half4)new float4(vertex, 0f),
                uv0 = new half2((half)((float)y / resolution), (half)((float)x / resolution)),
            };
        }
    }
}
