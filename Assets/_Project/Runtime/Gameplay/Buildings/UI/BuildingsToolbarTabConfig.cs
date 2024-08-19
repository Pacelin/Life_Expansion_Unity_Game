using UnityEngine;
using UnityEngine.Localization;

namespace Runtime.Gameplay.Buildings.UI
{
    [CreateAssetMenu(menuName = "Gameplay/Building Toolbar Tab")]
    public class BuildingsToolbarTabConfig : ScriptableObject
    {
        public string Caption => _caption.GetLocalizedString();
        
        [SerializeField] private LocalizedString _caption;
    }
}