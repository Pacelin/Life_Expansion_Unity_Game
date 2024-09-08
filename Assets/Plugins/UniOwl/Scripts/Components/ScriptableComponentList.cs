using System;
using System.Linq;
using UnityEngine;

namespace UniOwl.Components
{
    [Serializable]
    public abstract class ScriptableComponentList : ScriptableObject
    {
        [SerializeField]
        protected ScriptableComponent[] _components = Array.Empty<ScriptableComponent>();

        public ScriptableComponent[] Components => _components;
        
        public TComp GetComponent<TComp>() where TComp : ScriptableComponent
        {
            foreach (ScriptableComponent component in Components)
                if (component is TComp correctComponent)
                    return correctComponent;

            return null;
        }
        
        public bool TryGetComponent<TComp>(out TComp value) where TComp : ScriptableComponent
        {
            value = GetComponent<TComp>();
            return value != null;
        }
        
        private void Reset()
        {
            _components = Array.Empty<ScriptableComponent>();
        }
    }

    public abstract class ScriptableComponentList<T> : ScriptableComponentList where T : ScriptableComponent
    {
        public new T[] Components => _components.Cast<T>().ToArray();
        
        public new TComp GetComponent<TComp>() where TComp : T
        {
            foreach (T component in Components)
                if (component is TComp correctComponent)
                    return correctComponent;

            return null;
        }

        public new bool TryGetComponent<TComp>(out TComp value) where TComp : T
        {
            value = GetComponent<TComp>();
            return value != null;
        }
    }
}