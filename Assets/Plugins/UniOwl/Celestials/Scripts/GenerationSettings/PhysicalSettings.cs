using System;
using UnityEngine;

namespace UniOwl.Celestials
{
    [Serializable]
    public class PhysicalSettings
    {
        [Range(0f, 50f)]
        public float radius = 10f;
        [Range(0f, 50f)]
        public float amplitude = 5f;
    }
}
