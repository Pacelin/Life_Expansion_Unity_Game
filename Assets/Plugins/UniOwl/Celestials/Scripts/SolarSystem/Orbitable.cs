using UnityEngine;

namespace UniOwl.Celestials
{
    public class Orbitable : MonoBehaviour
    {
        [SerializeField]
        private Transform _transform;
        [Header("Orbit")]
        [SerializeField]
        private float _radius = 10f;
        [SerializeField]
        private float _rotationPeriod = 10f;
        [SerializeField]
        private float _rotationOffset;
        [SerializeField]
        private Vector3 _rotationEuler = Vector3.zero;

        private Vector3 _orbitPosition;
        public Vector3 OrbitPosition => _orbitPosition;
        
        public void UpdateOrbitPosition(Vector3 parentOffset)
        {
            float arg = 2f * Mathf.PI * Time.time / _rotationPeriod + _rotationOffset;
            _orbitPosition = parentOffset + Quaternion.Euler(_rotationEuler) * (_radius * new Vector3(Mathf.Cos(arg), 0f, Mathf.Sin(arg)));
        }

        public void UpdateTransformPosition(Orbitable target)
        {
            _transform.position = target.OrbitPosition - _orbitPosition;
        }
    }
}