using System;
using UnityEngine;

namespace UniOwl.Components
{
    [Serializable]
    public abstract class ScriptableComponentWithPrefab : ScriptableComponent
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private GameObject _instance;
        
        public GameObject Prefab => _prefab;

        public GameObject Instance
        {
            get => _instance;
            set => _instance = value;
        }
    }
}