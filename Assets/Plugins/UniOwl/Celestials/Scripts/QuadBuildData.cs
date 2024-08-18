using Unity.Collections;
using Unity.Mathematics;

namespace UniOwl.Celestials
{
    public struct QuadBuildData
    {
        public NativeArray<Vertex> vertices;
        public NativeArray<ushort> indices;

        public NativeArray<float> heights;
        public NativeArray<float3> normals;
    }
}
