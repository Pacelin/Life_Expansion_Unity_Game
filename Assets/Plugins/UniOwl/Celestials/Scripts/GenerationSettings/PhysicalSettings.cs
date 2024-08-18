using System;
using UnityEngine;

namespace UniOwl.Celestials
{
    [Serializable]
    public class PhysicalSettings
    {
        [Range(10f, 100f)]
        public float radius;
        [Range(0f, 1f)]
        public float seaLevel;
    }
}
