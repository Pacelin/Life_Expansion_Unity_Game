using PCG.TerrainGeneration;
using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace UniOwl.Celestials
{
    [Serializable]
    public struct TerrainGeneratorSettings
    {
        public int seed;

        public float radius;
        [Range(0f, 20f)]
        public float amplitude;
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
        public terrainDensity Evaluate(in float3 position)
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
        public terrainDensity FBM(in float3 position)
        {
            terrainDensity totalDensity = new terrainDensity();

            float freq = frequency;
            float amp = 1f, ampSum = 0f;
            
            for (int i = 0; i < octaves; i++)
            {
                float height = noise.snoise(position * freq, out float3 grad);
                height = (height + 1) * 0.5f;

                totalDensity += amp * new terrainDensity(height, grad * amp);

                ampSum += amp;
                amp *= persistence;
                freq *= lacunarity;
            }

            return totalDensity / ampSum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public terrainDensity Redistribute(in float3 position, in terrainDensity value)
        {
            return terrainDensity.pow(value, redistributionPower);
        }
    }
}
