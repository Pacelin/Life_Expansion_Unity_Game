using Runtime.Gameplay.Buildings.General;
using UnityEngine;

namespace Runtime.Gameplay.Buildings
{
    public class BuildingView : MonoBehaviour
    {
        public float BuildingRadius => _buildingRadius;
        public BuildingMesh Mesh => _buildingMesh;
        public IBuildingModel Model => _model;
        
        [SerializeField] private float _buildingRadius;
        [SerializeField] private BuildingMesh _buildingMesh;
        [SerializeField] private SphereCollider _buildingCollider;

        private IBuildingModel _model;
        
        private void OnValidate()
        {
            if (_buildingCollider)
            {
                _buildingCollider.radius = _buildingRadius;
                _buildingCollider.center = Vector3.zero;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.3f);
            Gizmos.DrawSphere(transform.position, _buildingRadius);
        }

        public void SetModel(IBuildingModel model) =>
            _model = model;
    }
}