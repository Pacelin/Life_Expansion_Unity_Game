using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

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
            
            float3 vertex = baseVertex;
            vertex[ax1] += x;
            vertex[ax2] += y;

            vertex = MathUtils.GetSpherePosition(vertex, resolution);

            vertices[vertexIndex] = new Vertex()
            {
                position = vertex * (radius + amplitude * heights[vertexIndex]),
                normal = normals[vertexIndex], //vertex, //new half4((half3)normals[vertexIndex], half.zero), //(half[w float4(vertex, 0f),
                uv0 = new half2((half)((float)y / resolution), (half)((float)x / resolution)),
            };
        }
    }
}
