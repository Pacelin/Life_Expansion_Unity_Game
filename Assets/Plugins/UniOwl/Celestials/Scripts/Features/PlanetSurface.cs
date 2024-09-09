using UniOwl.Components;
using UnityEngine;

namespace UniOwl.Celestials
{
    using static ColorUtils;
    
    [SearchMenu("Planet", "Surface")]
    [DisallowMultiple]
    public class PlanetSurface : PlanetComponent
    {
        private static readonly int s_rimColor = Shader.PropertyToID("_RimColor");
        private static readonly int s_grassColor = Shader.PropertyToID("_GrassColor");
        private static readonly int s_sandColor = Shader.PropertyToID("_SandColor");
        private static readonly int s_snowColor = Shader.PropertyToID("_SnowColor");
        private static readonly int s_dryColor = Shader.PropertyToID("_DryColor");
        private static readonly int s_rockColor = Shader.PropertyToID("_RockColor");
        private static readonly int s_tempTint = Shader.PropertyToID("_TempTint");
        private static readonly int s_overallLevel = Shader.PropertyToID("_OverallLevel");
        private static readonly int s_heightRangeSand = Shader.PropertyToID("_HeightRangeSand");
        private static readonly int s_heightRangeSnow = Shader.PropertyToID("_HeightRangeSnow");
        private static readonly int s_slopeRange = Shader.PropertyToID("_SlopeRange");
        private static readonly int s_tempLevel = Shader.PropertyToID("_TempLevel");

        [SerializeField]
        private ModelSettings _model;
        [SerializeField]
        private TextureSettings _textures;
        [SerializeField]
        private TerrainGeneratorSettings _generation;

        public ModelSettings Model => _model;
        public TextureSettings Textures => _textures;
        public TerrainGeneratorSettings Generation => _generation;

        [Header("Appearance")]
        [SerializeField]
        private Color rimColor = HexToRGBA("38434C");
        [SerializeField]
        private Color grassColor = HexToRGBA("56BC4F");
        [SerializeField]
        private Color sandColor = HexToRGBA("56BC4F");
        [SerializeField]
        private Color snowColor = HexToRGBA("FFFFFF");
        [SerializeField]
        private Color dryColor = HexToRGBA("CDB375");
        [SerializeField]
        private Color rockColor = HexToRGBA("8E8E8E");

        [SerializeField, MinMaxSlider(0f, 1f)]
        [InspectorName("Sand Range (In Radii)")]
        private Vector2 heightRangeSand = new Vector2(0f, 0.2f);
        [SerializeField, MinMaxSlider(0f, 1f)]
        [InspectorName("Snow Range (In Radii)")]
        private Vector2 heightRangeSnow = new Vector2(0.8f, 1f);
        [SerializeField, MinMaxSlider(0f, 90f)]
        [InspectorName("Slope Range (Degrees)")]
        private Vector2 slopeRange = new Vector2(0f, 90f);

        public override void UpdateVisual(GameObject editableGO)
        {
            var surfaceFaces = editableGO.GetComponentsInChildren<PlanetFace>();
            
            foreach (PlanetFace face in surfaceFaces)
            {
                var mat = face.Renderer.sharedMaterial;
                mat.SetColor(s_rimColor, rimColor);
                mat.SetColor(s_grassColor, grassColor);
                mat.SetColor(s_snowColor, snowColor);
                mat.SetColor(s_sandColor, sandColor);
                mat.SetColor(s_dryColor, dryColor);
                mat.SetColor(s_rockColor, rockColor);

                Color tint = _planetObject.Terraforming.GetTemperatureTint();
                mat.SetColor(s_tempTint, tint);
                
                mat.SetFloat(s_overallLevel, _planetObject.Terraforming.overallLevel);
                mat.SetFloat(s_tempLevel, _planetObject.Terraforming.temperatureLevel);
                mat.SetVector(s_heightRangeSand, heightRangeSand);
                mat.SetVector(s_heightRangeSnow, heightRangeSnow);
                mat.SetVector(s_slopeRange, slopeRange);
            }
        }
    }
}