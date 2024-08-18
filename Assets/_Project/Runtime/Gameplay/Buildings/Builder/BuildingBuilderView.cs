using UnityEngine;

namespace Runtime.Gameplay.Buildings.Builder
{
    public class BuildingBuilderView : MonoBehaviour
    {
        [SerializeField] private Transform _unitBuildingSphere;
        [SerializeField] private Transform _meshContainer;

        private GameObject _mesh;
        
        public void SetBuildingRadius(float radius) =>
            _unitBuildingSphere.localScale = Vector3.one * radius * 2;

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