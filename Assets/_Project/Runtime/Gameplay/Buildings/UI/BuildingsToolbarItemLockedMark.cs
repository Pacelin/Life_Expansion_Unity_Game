using UnityEngine;

namespace Runtime.Gameplay.Buildings.UI
{
    [ExecuteInEditMode]
    public class BuildingsToolbarItemLockedMark : MonoBehaviour
    {
        [SerializeField] private GameObject[] _disableWhenLocked;
        private void OnEnable()
        {
            foreach (var go in _disableWhenLocked)
                go.SetActive(false);
        }

        private void OnDisable()
        {
            foreach (var go in _disableWhenLocked)
                go.SetActive(true);
        }
    }
}