using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UniOwl.Celestials
{
    public class Planet : MonoBehaviour
    {
        [FormerlySerializedAs("_faces")]
        [SerializeField]
        private PlanetFace[] surfaceFaces;

        public PlanetFace[] SurfaceFaces => surfaceFaces;

        public Mesh[] SurfaceSharedMeshes => surfaceFaces.Select(face => face.Filter.sharedMesh).ToArray();

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

        private void Awake()
        {
            foreach (var mesh in SurfaceSharedMeshes)
                mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 2000);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="seaLevel">Range [0, 2], 0 = no sea, 1 = full sea, 2 = double height sea.</param>
        /// <param name="temperatureLevel">Range [0, 1], 0 = very cold, 1 = very hot, 0.5 = normal temperature.</param>
        /// <param name="atmosphereLevel">Range [0, 1], 0 = no atmosphere, 1 = dense atmosphere.</param>
        /// <param name="overallLevel">Range [0, 1], 0 = no life, 1 = full life.</param>
        public void UpdatePlanetAppearance(
                PlanetSettings settings,
                float seaLevel,
                float temperatureLevel,
                float atmosphereLevel,
                float overallLevel
            )
        {
            UpdateSurface(settings, seaLevel, temperatureLevel, atmosphereLevel, overallLevel);
            UpdateSea(settings, seaLevel, temperatureLevel, atmosphereLevel, overallLevel);
            UpdateAtmosphere(settings, seaLevel, temperatureLevel, atmosphereLevel, overallLevel);
            UpdateClouds(settings, seaLevel, temperatureLevel, atmosphereLevel, overallLevel);
            UpdateRings(settings);
        }

        private void UpdateSurface(
                PlanetSettings settings,
                float seaLevel,
                float temperatureLevel,
                float atmosphereLevel,
                float overallLevel
            )
        {
            foreach (PlanetFace face in surfaceFaces)
            {
                var mat = face.Renderer.sharedMaterial;
                mat.SetColor("_RimColor", settings.Appearance.rimColor);
                mat.SetColor("_GrassColor", settings.Appearance.grassColor);
                mat.SetColor("_DryColor", settings.Appearance.dryColor);
                mat.SetColor("_RockColor", settings.Appearance.rockColor);

                Color tint = GetTemperatureTint(settings, temperatureLevel);
                mat.SetColor("_TempTint", tint);
                
                mat.SetFloat("_OverallLevel", overallLevel);
            }
        }

        private Color GetTemperatureTint(PlanetSettings settings, float temperatureLevel)
        {
            Color fromTint = temperatureLevel < 0.5f ? settings.Appearance.coldTint : Color.white;
            Color toTint = temperatureLevel < 0.5f ? Color.white : settings.Appearance.warmTint;

            float temperatureT = temperatureLevel < 0.5f ? temperatureLevel * 2f : (temperatureLevel - 0.5f) * 2f;
            return Color.Lerp(fromTint, toTint, temperatureT);
        }

        private void UpdateSea(
                PlanetSettings settings,
                float seaLevel,
                float temperatureLevel,
                float atmosphereLevel,
                float overallLevel
            )
        {
            var mat = _sea.sharedMaterial;
            mat.SetColor("_RimColor", settings.Appearance.rimColor);
            mat.SetColor("_ShoreColor", settings.Appearance.waterShoreColor);
            mat.SetColor("_DeepColor", settings.Appearance.waterDeepColor);
            mat.SetFloat("_DeepDistance", settings.Appearance.deepDistance);
            mat.SetFloat("_Temperature", temperatureLevel);

            Color tint = GetTemperatureTint(settings, temperatureLevel);
            mat.SetColor("_TempTint", tint);
        }

        public void UpdateAtmosphere(
                PlanetSettings settings,
                float seaLevel,
                float temperatureLevel,
                float atmosphereLevel,
                float overallLevel
            )
        {
            _atmosphere.gameObject.SetActive(settings.Appearance.hasAtmosphere);
            _atmosphere.transform.localScale = Vector3.one * 2f * (settings.Physical.radius + settings.Appearance.atmosphereHeight);

            var mat = _atmosphere.sharedMaterial;
            
            mat.SetColor("_Color", settings.Appearance.atmosphereColor);
            mat.SetFloat("_Thickness", settings.Appearance.maxAtmosphereThickness * atmosphereLevel);
            
            Color tint = GetTemperatureTint(settings, temperatureLevel);
            mat.SetColor("_TempTint", tint);
        }

        public void UpdateClouds(
                PlanetSettings settings,
                float seaLevel,
                float temperatureLevel,
                float atmosphereLevel,
                float overallLevel
            )
        {
            _clouds.gameObject.SetActive(settings.Appearance.hasClouds);
            _clouds.transform.localScale = Vector3.one * 2f * (settings.Physical.radius + settings.Appearance.cloudsHeight);

            var mat = _clouds.sharedMaterial;
            
            Color tint = GetTemperatureTint(settings, temperatureLevel);
            mat.SetColor("_BaseColor", settings.Appearance.baseCloudsColor);
            mat.SetColor("_OvercastColor", settings.Appearance.overcastCloudsColor);
            mat.SetColor("_TempTint", tint);
            mat.SetFloat("_Thickness", settings.Appearance.maxCloudThickness * atmosphereLevel);
        }

        public void UpdateRings(PlanetSettings settings)
        {
            _rings.gameObject.SetActive(settings.Appearance.hasRings);
            _rings.transform.localScale = Vector3.one * 2f * (settings.Physical.radius + settings.Appearance.ringOuterRadius);

            var mat = _rings.sharedMaterial;
            mat.SetFloat("_InnerRadius", settings.Appearance.ringInnerRadius);
        }
    }
}
