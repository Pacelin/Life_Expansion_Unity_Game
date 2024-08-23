using UnityEngine;

namespace UniOwl.Celestials
{
    public class SolarSystem : MonoBehaviour
    {
        [SerializeField]
        private Orbitable _target;

        [SerializeField]
        private Orbitable _sun;
        [SerializeField]
        private Orbitable[] _planets;
        
        private void Update()
        {
            _sun.UpdateTransformPosition(_target);

            foreach (Orbitable planet in _planets)
            {
                planet.UpdateOrbitPosition(_sun.OrbitPosition);
                planet.UpdateTransformPosition(_target);
            }
        }
    }
}
