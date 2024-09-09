using System;
using UnityEngine;

namespace UniOwl.Components
{
    [Serializable]
    public abstract class ScriptableComponentWithPrefab : ScriptableComponent
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private GameObject _variant;
        
        public GameObject Prefab => _prefab;

        public GameObject Variant
        {
            get => _variant;
            set => _variant = value;
        }
    }
}