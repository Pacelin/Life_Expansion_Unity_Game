using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace UniOwl.Celestials
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public float3 position;
        public float3 normal;
        //public half4 tangent;
        public half2 uv0;
    }
}