using UnityEngine;

namespace Runtime.Gameplay.Planets
{
    public class PlanetRotator : MonoBehaviour
    {
        [SerializeField] private float _angularSpeed;

        private void Update()
        {
            transform.Rotate(transform.up, _angularSpeed * Time.deltaTime);
        }
    }
}