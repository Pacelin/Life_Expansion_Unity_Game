using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace UniOwl.Celestials
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public float3 position;
        public half4 normal;
        public half2 uv0;
    }
}