using System;
using UnityEngine;

using static UniOwl.ColorUtils;

namespace UniOwl.Celestials
{
    [Serializable]
    public class AppearanceSettings
    {
        [Header("Lighting")]
        public Color rimColor = HexToRGBA("38434C");
        
        [Header("Surface")]
        public Color grassColor = HexToRGBA("56BC4F");
        public Color sandColor = HexToRGBA("56BC4F");
        public Color snowColor = HexToRGBA("FFFFFF");
        public Color dryColor = HexToRGBA("CDB375");
        public Color rockColor = HexToRGBA("8E8E8E");

        public Vector2 heightRangeSand = new Vector2(0f, 0.2f);
        public Vector2 heightRangeSnow = new Vector2(0.8f, 1f);
        [InspectorName("Slope Range (Degrees)")]
        public Vector2 slopeRange = new Vector2(0f, 90f);

        [Header("Water")]
        public Color waterShoreColor = HexToRGBA("4B97AB");
        public Color waterDeepColor = HexToRGBA("1C286C");
        [Range(0f, 5f)]
        public float deepDistance = 0.5f;

        [Header("Atmosphere")]
        public bool hasAtmosphere = false;
        public Color atmosphereColor = HexToRGBA("BFF2FF");
        public float maxAtmosphereThickness = 0.1f;
        public float atmosphereHeight = 10f;

        [Header("Temperature")]
        public Color coldTint = HexToRGBA("");
        public Color warmTint = HexToRGBA("");

        [Header("Clouds")]
        public bool hasClouds = false;
        public Color baseCloudsColor = HexToRGBA("FFFFFF");
        public Color overcastCloudsColor = HexToRGBA("737373");
        public float maxCloudThickness = 0.45f;
        public float cloudsHeight = 8f;
        
        [Header("Rings")]
        public bool hasRings = false;
        public float ringOuterRadius = 30f;
        [Range(0f, 1f)]
        public float ringInnerRadius = 0.8f;
        [Range(0f, 100f)]
        public float ringScale = 10f, detailsScale = 15f;
    }
}