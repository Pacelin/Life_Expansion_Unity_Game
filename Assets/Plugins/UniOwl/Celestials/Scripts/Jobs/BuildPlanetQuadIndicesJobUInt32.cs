using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace UniOwl.Celestials
{
    [BurstCompile]
    public struct BuildPlanetQuadIndicesJobUInt32 : IJob
    {
        [ReadOnly, NativeDisableParallelForRestriction, DeallocateOnJobCompletion]
        public NativeArray<int> indexOffset;

        [WriteOnly]
        public NativeArray<uint> indices;

        public int resolution, resolutionPlus1;
        
        // Not optimal
        public void Execute()
        {
            int triangleIndex = 0;

            for (int y = 0; y < resolutionPlus1; y++)
                for (int x = 0, index = y * resolutionPlus1; x < resolutionPlus1; x++, index++)
                {
                    if (x >= resolution || y >= resolution) continue;

                    for (int t = 0; t < 6; t++)
                        indices[triangleIndex++] = (uint)(index + indexOffset[t]);
                }
        }
    }
}