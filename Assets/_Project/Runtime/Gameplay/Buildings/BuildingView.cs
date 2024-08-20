using Runtime.Gameplay.Buildings.General;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Runtime.Gameplay.Buildings
{
    public class BuildingView : MonoBehaviour
    {
        public float BuildingRadius => _buildingRadius;
        public BuildingMesh Mesh => _buildingMesh;
        public IBuildingModel Model => _model;

        public BuildingBubbleView Bubble => _bubble;
        
        [SerializeField] private float _buildingRadius;
        [SerializeField] private BuildingBubbleView _bubble;
        [SerializeField] private BuildingMesh _buildingMesh;
        [SerializeField] private SphereCollider _buildingCollider;
        [SerializeField] private DecalProjector _projector;

        private IBuildingModel _model;
        
        private void OnValidate()
        {
            if (_buildingCollider)
            {
                _buildingCollider.radius = _buildingRadius;
                _buildingCollider.center = Vector3.zero;
            }

            if (_projector)
            {
                _projector.transform.localScale = Vector3.one * _buildingRadius * 2;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.3f);
            Gizmos.DrawSphere(transform.position, _buildingRadius);
        }

        private void Awake() => DisableProjector();
        public void SetModel(IBuildingModel model) =>
            _model = model;

        public void EnableProjector() => _projector.gameObject.SetActive(true);
        public void DisableProjector() => _projector.gameObject.SetActive(false);
    }
}