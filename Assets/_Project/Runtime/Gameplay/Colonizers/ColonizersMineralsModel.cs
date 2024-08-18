using System;
using R3;
using UnityEngine;

namespace Runtime.Gameplay.Colonizers
{
    public class ColonizersMineralsModel
    {
        public ReadOnlyReactiveProperty<int> CurrentMinerals => _currentMinerals;
        public ReadOnlyReactiveProperty<int> MaxMinerals => _maxMinerals;

        private readonly ReactiveProperty<int> _currentMinerals;
        private readonly ReactiveProperty<int> _maxMinerals;

        public ColonizersMineralsModel(int initial, int initialMax)
        {
            _currentMinerals = new(initial);
            _maxMinerals = new(initialMax);
        }

        public bool HasMinerals(int count) => _currentMinerals.Value >= count;

        public void ApplyPurchase(int cost)
        {
            if (!HasMinerals(cost))
                throw new OperationCanceledException();
            _currentMinerals.Value -= cost;
        }

        public void AddMinerals(int value, out int remaining)
        {
            if (_currentMinerals.Value >= _maxMinerals.Value)
            {
                remaining = value;
                return;
            }
            remaining = Mathf.Max(0, _currentMinerals.Value + value - _maxMinerals.Value);
            _currentMinerals.Value += value - remaining;
        }

        public void ApplyDeltaToMax(int delta)
        {
            _maxMinerals.Value = Mathf.Max(0, _maxMinerals.Value + delta);
        }
    }
}