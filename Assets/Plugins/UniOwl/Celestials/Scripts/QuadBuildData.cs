using Unity.Collections;

namespace UniOwl.Celestials
{
    public struct QuadBuildData
    {
        public NativeArray<Vertex> vertices;
        public NativeArray<ushort> indices;
    }
}
