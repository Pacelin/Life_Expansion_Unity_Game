using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Planets
{
    public class PlanetComponent : MonoBehaviour
    {
        [SerializeField] private Transform _water;

        public void SetWaterRadius(float radius) => _water.localScale = Vector3.one * radius * 2;
    }
}