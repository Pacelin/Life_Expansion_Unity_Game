using System;
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

        private void Awake()
        {
            transform.gameObject.SetActive(false);
            CombineInstance[] combine = new CombineInstance[_faces.Length];
            for (int i = 0; i < _faces.Length; i++)
            {
                combine[i].mesh = _faces[i].Filter.sharedMesh;
                combine[i].transform = _faces[i].transform.localToWorldMatrix;
            }

            Mesh combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combine);
            gameObject.AddComponent<MeshFilter>().sharedMesh = combinedMesh;
            gameObject.AddComponent<MeshRenderer>();
            foreach (var face in _faces)
                face.gameObject.SetActive(false);
            transform.gameObject.SetActive(true);
            combinedMesh.bounds = new Bounds(Vector3.zero, Vector3.one * 2000);
            combinedMesh.RecalculateNormals();
            foreach (var mesh in SharedMeshes)
                mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 2000);
        }
    }
}
