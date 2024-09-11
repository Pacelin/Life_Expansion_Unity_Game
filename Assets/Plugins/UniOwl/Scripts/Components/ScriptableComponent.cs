using System;
using UnityEngine;

namespace UniOwl.Components
{
    [Serializable]
    public abstract class ScriptableComponent : ScriptableObject
    {
        [SerializeField]
        private bool _active = true;
        
        public bool Active => _active;

        [SerializeField]
        private ScriptableComponentList _list;

        public ScriptableComponentList List => _list;
        
        public virtual void Initialize(ScriptableComponentList parent)
        {
            _list = parent;
        }
        
        private void OnDestroy()
        {
            SetActiveInternal(false);
        }

        public void SetActiveInternal(bool value)
        {
            _active = value;
            if (_active)
                OnActive();
            else
                OnInactive();
        }

        public virtual void OnActive() { }

        public virtual void OnInactive() { }

        public TComp GetComponent<TComp>() where TComp : ScriptableComponent
        {
            return _list.GetComponent<TComp>();
        }

        public bool TryGetComponent<TComp>(out TComp value) where TComp : ScriptableComponent
        {
            return _list.TryGetComponent(out value);
        }
    }
}