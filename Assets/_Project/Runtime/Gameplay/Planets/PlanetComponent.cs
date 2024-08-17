using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Planets
{
    public class PlanetComponent : MonoBehaviour
    {
        [Inject] private PlanetConfig _config;
    }
}