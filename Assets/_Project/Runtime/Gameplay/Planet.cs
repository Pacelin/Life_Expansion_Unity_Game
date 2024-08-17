using UnityEngine;

namespace Runtime.Gameplay
{
    public class Planet : MonoBehaviour
    {
        public float Radius => _radius;
        public Vector3 Center => transform.position;
        
        [SerializeField] private float _radius;
    }
}