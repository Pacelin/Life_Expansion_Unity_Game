using UniOwl.Components;
using UnityEngine;

namespace UniOwl.Celestials
{
    using static ColorUtils;
    
    [SearchMenu("Planet", "Cloud Layer")]
    public class PlanetCloudLayer : PlanetComponent
    {
        private static readonly int s_tempTint = Shader.PropertyToID("_TempTint");
        private static readonly int s_thickness = Shader.PropertyToID("_Thickness");
        private static readonly int s_baseColor = Shader.PropertyToID("_BaseColor");
        private static readonly int s_overcastColor = Shader.PropertyToID("_OvercastColor");

        [SerializeField]
        private Color baseColor = HexToRGBA("FFFFFF");
        [SerializeField]
        private Color overcastColor = HexToRGBA("737373");
        [SerializeField, Min(0f)]
        private float maxThickness = 0.45f;
        [SerializeField, Min(0f)]
        private float height = 8f;

        public override void UpdateVisual(GameObject editableGO)
        {
            editableGO.transform.localScale = Vector3.one * (2f * (_planetObject.Physical.radius + height));

            var mat = editableGO.GetComponentInChildren<MeshRenderer>().sharedMaterial;
            
            Color tint = _planetObject.Terraforming.GetTemperatureTint();
            mat.SetColor(s_baseColor, baseColor);
            mat.SetColor(s_overcastColor, overcastColor);
            mat.SetColor(s_tempTint, tint);
            mat.SetFloat(s_thickness, maxThickness * _planetObject.Terraforming.atmosphereLevel);
        }
    }
}