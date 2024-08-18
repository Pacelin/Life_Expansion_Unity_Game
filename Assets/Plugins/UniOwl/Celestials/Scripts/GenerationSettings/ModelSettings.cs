using System;
using UnityEngine;

namespace UniOwl.Celestials
{
    [Serializable]
    public class ModelSettings
    {
        [Range(2f, 256f)]
        public int resolution = 32;
        public bool recalculateNormals = false;
    }
}
