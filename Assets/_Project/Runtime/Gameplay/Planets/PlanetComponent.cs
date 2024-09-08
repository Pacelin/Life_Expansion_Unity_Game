using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Planets
{
    public class PlanetComponent : MonoBehaviour
    {
        public UniOwl.Celestials.Planet_Old UniPlanet => _uniPlanet;
        [SerializeField] private Transform _water;
        [SerializeField] private UniOwl.Celestials.Planet_Old _uniPlanet;

        public void SetWaterRadius(float radius) => _water.localScale = Vector3.one * radius * 2;
    }
}