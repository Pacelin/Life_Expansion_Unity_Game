using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace UniOwl.Celestials
{
    [Serializable]
    public struct TerrainGeneratorSettings
    {
        public uint seed;

        [Range(0f, 10f)]
        public float frequency;
        [Range(0f, 1f)]
        public float persistence;
        [Range(1f, 4f)]
        public float lacunarity;
        [Range(1, 16)]
        public int octaves;
        [Range(0f, 10f)]
        public float redistributionPower;
        [Range(0f, 10f)]
        public float erosionPower;

        [Header("Domain Warping")]
        public float3 warpingOffset;
        public float warpingStrength;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public noise3 Evaluate(in float3 position)
        {
            var warpedPos = DomainWarping(position);
            var value = FBM(warpedPos);
            value = Redistribute(warpedPos, value);
            
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float3 DomainWarping(in float3 position)
        {
            var q = new float3(
                FBM(position - warpingOffset).value,
                FBM(position).value,
                FBM(position + warpingOffset).value
            );
            return position + q * warpingStrength;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public noise3 FBM(in float3 position)
        {
            noise3 totalDensity = new noise3();

            float freq = frequency;
            float amp = 1f, ampSum = 0f;
            
            for (int i = 0; i < octaves; i++)
            {
                float height = noise.snoise(position * freq, out float3 grad);
                noise3 density = new noise3(height, grad * freq);
                density = (density + 1f) * 0.5f;

                totalDensity += amp * density;

                ampSum += amp;
                amp *= persistence;
                freq *= lacunarity;
            }

            return totalDensity / ampSum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public noise3 Redistribute(in float3 position, in noise3 value)
        {
            return noise3.pow(value, redistributionPower);
        }
    }
}
