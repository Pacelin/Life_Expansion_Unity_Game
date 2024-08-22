using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UniOwl.Celestials
{
    public class Planet : MonoBehaviour
    {
        private static readonly int s_rimColor = Shader.PropertyToID("_RimColor");
        private static readonly int s_grassColor = Shader.PropertyToID("_GrassColor");
        private static readonly int s_dryColor = Shader.PropertyToID("_DryColor");
        private static readonly int s_rockColor = Shader.PropertyToID("_RockColor");
        private static readonly int s_tempTint = Shader.PropertyToID("_TempTint");
        private static readonly int s_overallLevel = Shader.PropertyToID("_OverallLevel");
        private static readonly int s_shoreColor = Shader.PropertyToID("_ShoreColor");
        private static readonly int s_deepColor = Shader.PropertyToID("_DeepColor");
        private static readonly int s_deepDistance = Shader.PropertyToID("_DeepDistance");
        private static readonly int s_temperature = Shader.PropertyToID("_Temperature");
        private static readonly int s_color = Shader.PropertyToID("_Color");
        private static readonly int s_thickness = Shader.PropertyToID("_Thickness");
        private static readonly int s_baseColor = Shader.PropertyToID("_BaseColor");
        private static readonly int s_overcastColor = Shader.PropertyToID("_OvercastColor");
        private static readonly int s_innerRadius = Shader.PropertyToID("_InnerRadius");

        
        [FormerlySerializedAs("_faces")]
        [SerializeField]
        private PlanetFace[] surfaceFaces;

        public PlanetFace[] SurfaceFaces => surfaceFaces;

        public Mesh[] SurfaceSharedMeshes => surfaceFaces.Select(face => face.Filter.sharedMesh).ToArray();

        [SerializeField]
        private PlanetSettings settings;
        
        [SerializeField]
        private MeshRenderer _sea;
        [SerializeField]
        private MeshRenderer _atmosphere;
        [SerializeField]
        private MeshRenderer _clouds;
        [SerializeField]
        private MeshRenderer _rings;

        public MeshRenderer SeaRenderer => _sea;
        public MeshRenderer AtmosphereRenderer => _atmosphere;
        public MeshRenderer CloudsRenderer => _clouds;
        public MeshRenderer RingsRenderer => _rings;
        
        public Transform SeaTransform => _sea.transform;

        private float currentTemperature, currentAtmosphere, currentOverall;
        
        private void Awake()
        {
            foreach (var mesh in SurfaceSharedMeshes)
                mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 2000);
        }

        /// <param name="temperatureLevel">Range [0, 1], 0 = very cold, 1 = very hot, 0.5 = normal temperature.</param>
        public void SetTemperatureLevel(float temperatureLevel)
        {
            currentTemperature = temperatureLevel;
            
            UpdateSurface();
            UpdateSea();
            UpdateAtmosphere();
            UpdateClouds();
        }
        
        /// <param name="atmosphereLevel">Range [0, 1], 0 = no atmosphere, 1 = dense atmosphere.</param>
        public void SetAtmosphereLevel(float atmosphereLevel)
        {
            currentAtmosphere = atmosphereLevel;
            
            UpdateAtmosphere();
            UpdateClouds();
        }

        /// <param name="overallLevel">Range [0, 1], 0 = no life, 1 = full life.</param>
        public void SetOverallLevel(float overallLevel)
        {
            currentOverall = overallLevel;
            UpdateSurface();
        }
        
        public void UpdatePlanetAppearance()
        {
            UpdateSurface();
            UpdateSea();
            UpdateAtmosphere();
            UpdateClouds();
            UpdateRings();
        }

        private void UpdateSurface()
        {
            foreach (PlanetFace face in surfaceFaces)
            {
                var mat = face.Renderer.sharedMaterial;
                mat.SetColor(s_rimColor, settings.Appearance.rimColor);
                mat.SetColor(s_grassColor, settings.Appearance.grassColor);
                mat.SetColor(s_dryColor, settings.Appearance.dryColor);
                mat.SetColor(s_rockColor, settings.Appearance.rockColor);

                Color tint = GetTemperatureTint(currentTemperature);
                mat.SetColor(s_tempTint, tint);
                
                mat.SetFloat(s_overallLevel, currentOverall);
            }
        }

        private Color GetTemperatureTint(float temperatureLevel)
        {
            Color fromTint = temperatureLevel < 0.5f ? settings.Appearance.coldTint : Color.white;
            Color toTint = temperatureLevel < 0.5f ? Color.white : settings.Appearance.warmTint;

            float temperatureT = temperatureLevel < 0.5f ? temperatureLevel * 2f : (temperatureLevel - 0.5f) * 2f;
            return Color.Lerp(fromTint, toTint, temperatureT);
        }

        private void UpdateSea()
        {
            var mat = _sea.sharedMaterial;
            mat.SetColor(s_rimColor, settings.Appearance.rimColor);
            mat.SetColor(s_shoreColor, settings.Appearance.waterShoreColor);
            mat.SetColor(s_deepColor, settings.Appearance.waterDeepColor);
            mat.SetFloat(s_deepDistance, settings.Appearance.deepDistance);
            mat.SetFloat(s_temperature, currentTemperature);

            Color tint = GetTemperatureTint(currentTemperature);
            mat.SetColor(s_tempTint, tint);
        }

        private void UpdateAtmosphere()
        {
            _atmosphere.gameObject.SetActive(settings.Appearance.hasAtmosphere);
            _atmosphere.transform.localScale = Vector3.one * (2f * (settings.Physical.radius + settings.Appearance.atmosphereHeight));

            var mat = _atmosphere.sharedMaterial;
            
            mat.SetColor(s_color, settings.Appearance.atmosphereColor);
            mat.SetFloat(s_thickness, settings.Appearance.maxAtmosphereThickness * currentAtmosphere);
            
            Color tint = GetTemperatureTint(currentTemperature);
            mat.SetColor(s_tempTint, tint);
        }

        private void UpdateClouds()
        {
            _clouds.gameObject.SetActive(settings.Appearance.hasClouds);
            _clouds.transform.localScale = Vector3.one * (2f * (settings.Physical.radius + settings.Appearance.cloudsHeight));

            var mat = _clouds.sharedMaterial;
            
            Color tint = GetTemperatureTint(currentTemperature);
            mat.SetColor(s_baseColor, settings.Appearance.baseCloudsColor);
            mat.SetColor(s_overcastColor, settings.Appearance.overcastCloudsColor);
            mat.SetColor(s_tempTint, tint);
            mat.SetFloat(s_thickness, settings.Appearance.maxCloudThickness * currentAtmosphere);
        }

        private void UpdateRings()
        {
            _rings.gameObject.SetActive(settings.Appearance.hasRings);
            _rings.transform.localScale = Vector3.one * (2f * (settings.Physical.radius + settings.Appearance.ringOuterRadius));

            var mat = _rings.sharedMaterial;
            mat.SetFloat(s_innerRadius, settings.Appearance.ringInnerRadius);
        }
    }
}
