using System;
using UnityEngine;

namespace UniOwl.Celestials
{
    using static ColorUtils;

    [Serializable]
    public class TerraformingSettings
    {
        [Range(0f, 100f)]
        public float seaLevel;
        [Range(0f, 1f)]
        public float temperatureLevel, atmosphereLevel, overallLevel;
        
        [SerializeField]
        private Color coldTint = HexToRGBA("FFFFFF"), warmTint = HexToRGBA("FFFFFF");
        
        public Color GetTemperatureTint()
        {
            Color fromTint = temperatureLevel < 0.5f ? coldTint : Color.white;
            Color toTint = temperatureLevel < 0.5f ? Color.white : warmTint;

            float temperatureT = temperatureLevel < 0.5f ? temperatureLevel * 2f : (temperatureLevel - 0.5f) * 2f;
            return Color.Lerp(fromTint, toTint, temperatureT);
        }
    }
}