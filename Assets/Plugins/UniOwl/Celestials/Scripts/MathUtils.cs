using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace UniOwl.Celestials
{
    public static class MathUtils
    {    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 GetSpherePosition(in float3 cubePos, int resolution)
        {
            float3 v = 2f / resolution * cubePos - 1f;

            float x2 = v.x * v.x;
            float y2 = v.y * v.y;
            float z2 = v.z * v.z;

            // Better approximation on a sphere.
            float3 s = new float3()
            {
                x = 1f - y2 * .5f - z2 * .5f + y2 * z2 / 3f,
                y = 1f - x2 * .5f - z2 * .5f + x2 * z2 / 3f,
                z = 1f - x2 * .5f - y2 * .5f + x2 * y2 / 3f
            };

            return v * math.sqrt(s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float4 ToFloat4(this Color color)
        {
            return new float4(color.r, color.g, color.b, color.a);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float3 ToFloat3(this Color color)
        {
            return new float3(color.r, color.g, color.b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Float4ToRGBA32(float4 color)
        {
            color = math.clamp(color, 0f, 1f) * 255f;

            uint r = (uint)color.w;
            uint g = (uint)color.z;
            uint b = (uint)color.y;
            uint a = (uint)color.x;

            return (r << 24) | (g << 16) | (b << 8) | a;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte3 Float3ToRGB24(float3 color)
        {
            color = math.clamp(color, 0f, 1f) * 255f;

            byte r = (byte)color.x;
            byte g = (byte)color.y;
            byte b = (byte)color.z;

            return new byte3(r, g, b);
        }
    }
}
