using UnityEngine;

namespace Runtime.Gameplay.Misc
{
    public class RotationAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _axis;
        [SerializeField] private float _angularSpeed;

        private void Update() =>
            _target.Rotate(_axis, _angularSpeed * Time.deltaTime);
    }
}