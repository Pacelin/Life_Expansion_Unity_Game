using System;
using Unity.Collections;
using Unity.Mathematics;

namespace UniOwl.Celestials
{
    public struct PlanetHeightData : IDisposable
    {
        public NativeArray<float>[] heights;
        public NativeArray<float3>[] normals;
        
        public void Dispose()
        {
            for (int i = 0; i < 6; i++)
            {
                heights[i].Dispose();
                normals[i].Dispose();
            }
        }
    }
}