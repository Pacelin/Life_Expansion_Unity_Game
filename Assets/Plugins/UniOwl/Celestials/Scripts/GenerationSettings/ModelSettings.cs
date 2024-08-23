using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UniOwl.Celestials
{
    [Serializable]
    public class ModelSettings
    {
        [Range(2f, 256f)]
        public int resolution = 32;
        public MeshUpdateFlags updateFlags = MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontResetBoneBounds | MeshUpdateFlags.DontRecalculateBounds;

        public bool optimizeMesh = true;
        public bool recalculateNormals = false;
        public bool recalculateTangents = false;
    }
}
