using UnityEngine;

namespace UniOwl.Components
{
    public abstract class ScriptableComponentListWithPrefab<T> : ScriptableComponentList<T> where T : ScriptableComponentWithPrefab
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private GameObject _root;
    }
}