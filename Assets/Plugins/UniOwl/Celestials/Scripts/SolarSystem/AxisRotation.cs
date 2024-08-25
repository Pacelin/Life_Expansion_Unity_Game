using UnityEngine;

namespace UniOwl.Celestials
{
    public class AxisRotation : MonoBehaviour
    {
        [SerializeField]
        private float _rotationSpeed;

        private void Update()
        {
            transform.Rotate(transform.up, _rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}