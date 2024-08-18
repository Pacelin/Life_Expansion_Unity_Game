using System.Linq;
using UnityEngine;

namespace UniOwl.Celestials
{
    public class Planet : MonoBehaviour
    {
        [SerializeField]
        private PlanetFace[] _faces;

        public PlanetFace[] Faces => _faces;

        public Mesh[] SharedMeshes => _faces.Select(face => face.Filter.sharedMesh).ToArray();

        [SerializeField]
        private Transform _sea;

        public Transform Sea => _sea;
    }
}
