using System.Linq;
using UnityEngine;

namespace UniOwl.Audio
{
    [CreateAssetMenu(menuName = "Game/Audio/Footsteps Data", fileName = "SO_FootstepsData")]
    public class FootstepsData : ScriptableObject
    {
        [SerializeField]
        private FootstepsDataEntry[] entries;

        public AudioContainer GetCue(PhysicsMaterial material) => entries.FirstOrDefault(entry => entry.material == material)?.container;
    }
}