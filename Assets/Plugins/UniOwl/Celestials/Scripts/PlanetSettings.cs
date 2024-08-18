using UnityEngine;
using Random = UnityEngine.Random;

namespace UniOwl.Celestials
{
    [CreateAssetMenu(menuName = "Game/Celestials/Planet Settings", fileName = "SO_Planet")]
    public class PlanetSettings : ScriptableObject
    {
        [SerializeField]
        private Planet _planet;

        [SerializeField]
        private ModelSettings _model;
        [SerializeField]
        private TextureSettings _textures;
        [SerializeField]
        private PhysicalSettings _physical;
        [SerializeField]
        private SurfaceGenerator _generation;
        
        [SerializeField]
        public Color GrassColor, RockColor;

        public ModelSettings Model => _model;
        public TextureSettings Textures => _textures;
        public PhysicalSettings Physical => _physical;
        public SurfaceGenerator Generation => _generation;
        
        public Planet Planet
        {
            get => _planet;
            set => _planet = value;
        }

        private void OnEnable()
        {
            if (_generation.seed == 0)
                _generation.seed = Random.Range(int.MinValue, int.MaxValue);
        }
    }
}
