using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace Runtime.Gameplay.Buildings.UI
{
    public class BuildingsToolbarView : MonoBehaviour
    {
        public ReadOnlyReactiveProperty<BuildingsToolbarItemView> Selected => _selected;
        public ReadOnlyReactiveProperty<BuildingsToolbarItemView> Hovered => _hovered;

        [SerializeField] private Transform _container;
        [SerializeField] private Transform _tabsContainer;
        [SerializeField] private BuildingsToolbarItemView _itemPrefab;
        [SerializeField] private BuildingsToolbarTabView _tabPrefab;

        private ReactiveProperty<BuildingsToolbarItemView> _selected = new();
        private ReactiveProperty<BuildingsToolbarTabView> _selectedTab = new();
        private ReactiveProperty<BuildingsToolbarItemView> _hovered = new();
        private List<BuildingsToolbarItemView> _items = new();
        private Dictionary<BuildingsToolbarTabView, List<BuildingsToolbarItemView>> _tabs = new();
        private CompositeDisposable _disposables;

        private void OnEnable()
        {
            _disposables = new();
            foreach (var item in _items)
                Subscribe(item);
            foreach (var (tab, _) in _tabs)
                Subscribe(tab);
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
            itemView.gameObject.SetActive(false);
            _items.Add(itemView);
            Subscribe(itemView);
            return itemView;
        }

        public void AssignTab(BuildingsToolbarItemView item, BuildingsToolbarTabView tab) =>
            _tabs[tab].Add(item);

        public void SelectFirstTab() => _tabs.First().Key.SetSelected(true);
        public BuildingsToolbarTabView CreateTab(string tab)
        {
            var tabView = Instantiate(_tabPrefab, _tabsContainer);
            tabView.SetCaption(tab);
            _tabs.Add(tabView, new List<BuildingsToolbarItemView>());
            Subscribe(tabView);
            return tabView;
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
        
        private void Subscribe(BuildingsToolbarTabView tab)
        {
            tab.IsSelected
                .Where(isSelected => isSelected)
                .Subscribe(_ =>
                {
                    if (_selectedTab.Value != null)
                    {
                        _selectedTab.Value.SetSelected(false);
                        foreach (var item in _tabs[_selectedTab.Value])
                            item.gameObject.SetActive(false);
                    }
                    _selectedTab.Value = tab;
                    foreach (var item in _tabs[_selectedTab.Value])
                        item.gameObject.SetActive(true);
                })
                .AddTo(_disposables);
        }
    }
}