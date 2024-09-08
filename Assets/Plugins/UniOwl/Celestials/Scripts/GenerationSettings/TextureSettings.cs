using System;
using UnityEngine;

namespace UniOwl.Celestials
{
    [Serializable]
    public class TextureSettings
    {
        [Range(2f, 4096f)]
        public int resolution = 128;
        
        public bool generateNormals = true;
        public bool generateHeights = true;

        public bool compression;
        [Range(0, 100)]
        public int _compressionQuality = 100;
    }
}
