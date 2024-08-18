using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace UniOwl.Celestials
{
    [BurstCompile]
    public struct BuildPlanetQuadIndicesJob : IJob
    {
        [ReadOnly, NativeDisableParallelForRestriction, DeallocateOnJobCompletion]
        public NativeArray<int> indexOffset;

        [WriteOnly]
        public NativeArray<ushort> indices;

        public int resolution;
        
        // Not optimal
        public void Execute()
        {            
            for (int y = 0, triangleIndex = 0; y < resolution + 1; y++)
                for (int x = 0, index = y * (resolution + 1); x < resolution + 1; x++, index++)
                {
                    if (x >= resolution || y >= resolution) continue;

                    for (int t = 0; t < 6; t++)
                        indices[triangleIndex++] = (ushort)(index + indexOffset[t]);
                }
        }
    }
}