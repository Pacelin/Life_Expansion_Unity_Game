using UnityEngine;

namespace UniOwl.Celestials
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class PlanetFace : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter _filter;

        [SerializeField]
        private MeshRenderer _renderer;

        public MeshFilter Filter => _filter;
        public MeshRenderer Renderer => _renderer;
    }
}