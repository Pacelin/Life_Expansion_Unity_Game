using UnityEngine;

namespace Runtime.Gameplay.Buildings.UI
{
    public class CongratulationView : MonoBehaviour
    {
        public static CongratulationView Instance;

        private void Awake()
        {
            Instance = this;
            gameObject.SetActive(false);
        }
    }
}