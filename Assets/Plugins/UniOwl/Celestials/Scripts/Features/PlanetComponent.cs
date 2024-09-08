using UniOwl.Components;
using UnityEngine;

namespace UniOwl.Celestials
{
    public abstract class PlanetComponent : ScriptableComponentWithPrefab
    {
        protected PlanetObject _planetObject;

        public override void Initialize(ScriptableComponentList parent)
        {
            base.Initialize(parent);
            _planetObject = (PlanetObject)parent;
        }
        
        public override void OnActive()
        {
            
        }

        public override void OnInactive()
        {
            
        }

        public abstract void UpdateVisual(GameObject editableGO);
    }
}