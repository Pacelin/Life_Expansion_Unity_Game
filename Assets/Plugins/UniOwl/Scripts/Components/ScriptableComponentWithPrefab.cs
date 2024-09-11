using System;
using UnityEngine;

namespace UniOwl.Components
{
    [Serializable]
    public abstract class ScriptableComponentWithPrefab : ScriptableComponent
    {
        [SerializeField] private GameObject _prefab;
        public GameObject Prefab => _prefab;
        
        public abstract void UpdateVisual(GameObject editableGO);
    }
}