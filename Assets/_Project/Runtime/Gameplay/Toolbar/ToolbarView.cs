using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Runtime.Gameplay.Toolbar
{
    public class ToolbarView : MonoBehaviour
    {
        public ReadOnlyReactiveProperty<ToolbarItemView> Selected => _selected;
        
        [SerializeField] private Transform _container;

        private ReactiveProperty<ToolbarItemView> _selected;
        private List<ToolbarItemView> _items;
        private CompositeDisposable _disposables;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }
        
        public void Add(ToolbarItemView item)
        {
            
        }
    }
}