using UniOwl.Components;
using UnityEngine;

namespace UniOwl.Celestials
{
    using static ColorUtils;
    
    [SearchMenu("Planet", "Rings Layer")]
    public class PlanetRingsLayer : PlanetComponent
    {
        private static readonly int s_baseColor = Shader.PropertyToID("Base_Color");
        private static readonly int s_detailColor = Shader.PropertyToID("Detail_Color");
        private static readonly int s_innerRadius = Shader.PropertyToID("_InnerRadius");
        private static readonly int s_baseScale = Shader.PropertyToID("_BaseScale");
        private static readonly int s_detailScale = Shader.PropertyToID("_DetailScale");
        
        [SerializeField]
        private Color baseColor = HexToRGBA("FFFFFFFF");
        [SerializeField]
        private Color detailsColor = HexToRGBA("FFFFFF00");
        [SerializeField]
        private float outerRadius = 30f;
        [SerializeField, Range(0f, 1f)]
        private float innerRadius = 0.8f;
        [SerializeField, Range(0f, 100f)]
        private float baseScale = 10f;

        [SerializeField, Range(0f, 100f)]
        private float detailsScale = 15f;

        public override void UpdateVisual(GameObject editableGO)
        {
            if (!Application.isPlaying)
                editableGO.transform.localScale = Vector3.one * 2f * outerRadius;

            var mat = PlanetAssetUtils.GetMaterialInChildren(editableGO);
            
            mat.SetColor(s_baseColor, baseColor);
            mat.SetColor(s_detailColor, detailsColor);
            mat.SetFloat(s_innerRadius, innerRadius);
            mat.SetFloat(s_baseScale, baseScale);
            mat.SetFloat(s_detailScale, detailsScale);
        }
    }
}