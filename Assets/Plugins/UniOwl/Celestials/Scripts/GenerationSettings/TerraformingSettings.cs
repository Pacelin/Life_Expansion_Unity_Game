using System;
using UnityEngine;

namespace UniOwl.Celestials
{
    using static ColorUtils;

    [Serializable]
    public class TerraformingSettings
    {
        [Range(0f, 1f)]
        public float seaLevel = .5f, temperatureLevel = .5f, atmosphereLevel = .5f, overallLevel = .5f;
        
        [SerializeField]
        private Color coldTint = HexToRGBA("FFFFFF"), warmTint = HexToRGBA("FFFFFF");

        public Color GetTemperatureTint() => GetTemperatureTint(temperatureLevel);
        
        public Color GetTemperatureTint(float temperatureLevel)
        {
            Color fromTint = temperatureLevel < 0.5f ? coldTint : Color.white;
            Color toTint = temperatureLevel < 0.5f ? Color.white : warmTint;

            float temperatureT = temperatureLevel < 0.5f ? temperatureLevel * 2f : (temperatureLevel - 0.5f) * 2f;
            return Color.Lerp(fromTint, toTint, temperatureT);
        }
    }
}