using UniOwl.Components;
using UnityEngine;

namespace UniOwl.Celestials
{
    using static ColorUtils;
    
    [SearchMenu("Planet", "Sea")]
    [DisallowMultiple]
    public class PlanetSea : PlanetComponent
    {
        private static readonly int s_rimColor = Shader.PropertyToID("_RimColor");
        private static readonly int s_tempTint = Shader.PropertyToID("_TempTint");
        private static readonly int s_shoreColor = Shader.PropertyToID("_ShoreColor");
        private static readonly int s_deepColor = Shader.PropertyToID("_DeepColor");
        private static readonly int s_deepDistance = Shader.PropertyToID("_DeepDistance");
        private static readonly int s_temperature = Shader.PropertyToID("_Temperature");
        
        [SerializeField]
        private Color rimColor = HexToRGBA("38434C");
        [SerializeField]
        private Color shoreColor = HexToRGBA("4B97AB");
        [SerializeField]
        private Color deepColor = HexToRGBA("1C286C");
        [SerializeField, Range(0f, 5f)]
        private float deepDistance = 0.5f;

        public override void UpdateVisual(GameObject editableGO)
        {
            float seaRadius = Planet.Physical.radius * Planet.Terraforming.seaLevel;
            editableGO.transform.localScale = 2f * seaRadius * Vector3.one;
            
            var mat = editableGO.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            
            mat.SetColor(s_rimColor, rimColor);
            mat.SetColor(s_shoreColor, shoreColor);
            mat.SetColor(s_deepColor, deepColor);
            mat.SetFloat(s_deepDistance, deepDistance);
            mat.SetFloat(s_temperature, Planet.Terraforming.temperatureLevel);

            Color tint = Planet.Terraforming.GetTemperatureTint();
            mat.SetColor(s_tempTint, tint);
        }
    }
}