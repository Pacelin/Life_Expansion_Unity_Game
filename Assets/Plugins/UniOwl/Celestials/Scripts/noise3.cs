using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace UniOwl.Celestials
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct noise3
    {
        public readonly float value;
        public readonly float3 gradient;

        public noise3(in float value, in float3 gradient)
        {
            this.value = value;
            this.gradient = gradient;
        }
    }
}