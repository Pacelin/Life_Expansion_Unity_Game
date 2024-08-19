using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Runtime.Gameplay.Buildings.Builder
{
    public class BuildingBuilderView : MonoBehaviour
    {
        [SerializeField] private Transform _meshContainer;
        [SerializeField] private DecalProjector _projector;
        [SerializeField] private Material _badProjectorMaterial;
        [SerializeField] private Material _goodProjectorMaterial;
        
        private GameObject _mesh;
        
        public void SetBuildingRadius(float radius) =>
            _projector.transform.localScale = Vector3.one * radius * 2;

        public void SetProjector(bool good) =>
            _projector.material = good ? _goodProjectorMaterial : _badProjectorMaterial;
        
        public void SetBuildingMesh(BuildingMesh meshPrefab)
        {
            var mesh = Instantiate(meshPrefab, _meshContainer);
            mesh.transform.localPosition = meshPrefab.transform.localPosition;
            _mesh = mesh.gameObject;
        }

        public void ClearMesh() =>
            Destroy(_mesh);
    }
}