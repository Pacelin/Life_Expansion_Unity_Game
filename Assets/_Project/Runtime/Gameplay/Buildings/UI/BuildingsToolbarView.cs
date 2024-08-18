using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Runtime.Gameplay.Buildings.UI
{
    public class BuildingsToolbarView : MonoBehaviour
    {
        public ReadOnlyReactiveProperty<BuildingsToolbarItemView> Selected => _selected;
        public ReadOnlyReactiveProperty<BuildingsToolbarItemView> Hovered => _hovered;

        [SerializeField] private Transform _container;
        [SerializeField] private BuildingsToolbarItemView _itemPrefab;

        private ReactiveProperty<BuildingsToolbarItemView> _selected = new();
        private ReactiveProperty<BuildingsToolbarItemView> _hovered = new();
        private List<BuildingsToolbarItemView> _items = new();
        private CompositeDisposable _disposables;

        private void OnEnable()
        {
            _disposables = new();
            foreach (var item in _items)
                Subscribe(item);
        }

        private void OnDisable()
        {
            if (_selected.Value != null)
                _selected.Value.SetSelected(false);
            _disposables.Dispose();
        }

        public void ResetSelection()
        {
            if (_selected.Value != null)
                _selected.Value.SetSelected(false);
        }
        
        public BuildingsToolbarItemView CreateItem()
        {
            var itemView = Instantiate(_itemPrefab, _container);
            _items.Add(itemView);
            Subscribe(itemView);
            return itemView;
        }

        private void Subscribe(BuildingsToolbarItemView item)
        {
            item.IsSelected
                .Subscribe(isSelected =>
                {
                    if (isSelected)
                    {
                        if (_selected.Value != null)
                            _selected.Value.SetSelected(false);
                        _selected.Value = item;
                    }
                    else
                    {
                        _selected.Value = null;
                    }
                })
                .AddTo(_disposables);
            item.IsHover
                .Subscribe(isHover =>
                    _hovered.Value = isHover ? item : null)
                .AddTo(_disposables);
        }
    }
}