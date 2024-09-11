using UniOwl.Components;
using UnityEngine;

namespace UniOwl.Celestials
{
    using static ColorUtils;
    
    [SearchMenu("Planet", "Atmosphere")]
    [DisallowMultiple]
    public class PlanetAtmosphere : PlanetComponent
    {
        private static readonly int s_tempTint = Shader.PropertyToID("_TempTint");
        private static readonly int s_color = Shader.PropertyToID("_Color");
        private static readonly int s_thickness = Shader.PropertyToID("_Thickness");
        
        [SerializeField]
        private Color tint = HexToRGBA("BFF2FF");
        [SerializeField, Min(0f)]
        private float maxThickness = 0.1f;
        [SerializeField, Min(0f)]
        private float height = 10f;
        
        public override void UpdateVisual(GameObject editableGO)
        {
            editableGO.transform.localScale = Vector3.one * (2f * (Planet.Physical.radius + height));

            var mat = editableGO.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            
            mat.SetColor(s_color, tint);
            mat.SetFloat(s_thickness, maxThickness * Planet.Terraforming.atmosphereLevel);
            
            Color tempTint = Planet.Terraforming.GetTemperatureTint();
            mat.SetColor(s_tempTint, tempTint);
        }
    }
}