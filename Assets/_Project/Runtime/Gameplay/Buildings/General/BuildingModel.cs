using System;
using System.Collections.Generic;
using Runtime.Gameplay.Buildings.Builder;
using Runtime.Gameplay.Colonizers;
using Runtime.Gameplay.Planets;
using Zenject;
using R3;
using Runtime.Core;

namespace Runtime.Gameplay.Buildings.General
{
    public abstract class BuildingModel<T> : IBuildingModel
        where T : BuildingConditionalConfig
    {
        public int EnergyCost => _config.EnergyCost;
        public int ColonizersCost => _config.ColonizersCost;
        public BuildingView View => _view;
        public bool Enabled => _enabled;

        public bool IsNew { get; set; } = true;
        
        [Inject] protected BuildingView _view;
        [Inject] protected T _config;

        [Inject] protected Planet _planet;
        [Inject] protected ColonizersModel _colonizers;
        [Inject] protected BuildService _buildService;
        [Inject] protected BuildingBubbleConfig _bubbleConfig;

        private EBuildingState _state = EBuildingState.None;
        private bool _enabled;
        private IDisposable _brokeSync;

        public void Build()
        {
            _colonizers.Minerals.ApplyPurchase(_config.MineralsCost);
            if (_config.BuildTerritory == EBuildTerritory.Water)
                _brokeSync = WaterSync();
            else
                _brokeSync = GroundSync();
        }

        public void Delete()
        {
            SetEnabled(false);
            _colonizers.Minerals.AddMinerals(_config.MineralsCost / 2, out _);
        }

        public void SetState(params EBuildingState[] states)
        {
            bool changed = false;
            foreach (var state in states)
            {
                if (_state.HasFlag(state))
                    continue;
                _state |= state;
                changed = true;
            }

            if (!changed)
                return;
            UpdateState();
        }

        public void CancelState(EBuildingState state)
        {
            if (!_state.HasFlag(state))
                return;
            _state &= ~state;
            UpdateState();
        }
        
        protected abstract void OnEnable();
        protected abstract void OnDisable();
        protected abstract void Dispose();

        private void SetEnabled(bool enabled)
        {
            if (_enabled == enabled)
                return;
            _enabled = enabled;
            _view.SetEnabled(_enabled);    
            
            if (_enabled)
            {
                if (_config.EnergyCost > 0)
                    _colonizers.Energy.ApplyDeltaToUsage(_config.EnergyCost);
                if (_config.ColonizersCost > 0)
                    _colonizers.Population.ApplyDeltaToBusy(_config.ColonizersCost);
                OnEnable();
            }
            else
            {
                if (_config.EnergyCost > 0)
                    _colonizers.Energy.ApplyDeltaToUsage(-_config.EnergyCost);
                if (_config.ColonizersCost > 0)
                    _colonizers.Population.ApplyDeltaToBusy(-_config.ColonizersCost);
                OnDisable();
            }
        }

        private void UpdateState()
        {
            List<string> reasons = new List<string>();
            if (_state.HasFlag(EBuildingState.NoColonists)) 
                reasons.Add(_bubbleConfig.NoColonizersText);
            if (_state.HasFlag(EBuildingState.NoEnergy))
                reasons.Add(_bubbleConfig.NoEnergyText);
            if (_state.HasFlag(EBuildingState.NoWater))
                reasons.Add(_bubbleConfig.NoWaterText);
            if (_state.HasFlag(EBuildingState.Flooded))
                reasons.Add(_bubbleConfig.FloodText);
            if (reasons.Count == 0)
            {
                _view.Bubble.Hide();
                SetEnabled(true);
            }
            else
            {
                _view.Bubble.ShowBubble(EBubbleIcon.Warning, string.Join("\n", reasons), ECursorIcon.Warning);
                SetEnabled(false);
            }
        }
        
        private IDisposable WaterSync()
        {
            return _planet.Water.Value
                .Subscribe(_ =>
                {
                    var territory = _buildService.CheckBuildTerritory(_view.transform.position,
                        out var newPosition);
                    _view.transform.position = newPosition;
                    if (territory != EBuildTerritory.Water)
                        SetState(EBuildingState.NoWater);
                    else
                        CancelState(EBuildingState.NoWater);
                });
        }
        
        private IDisposable GroundSync()
        {
            return _planet.Water.Value
                .Subscribe(_ =>
                {
                    var territory = _buildService.CheckBuildTerritory(_view.transform.position, out var _);
                    if (territory != EBuildTerritory.Ground)
                        SetState(EBuildingState.Flooded);
                    else
                        CancelState(EBuildingState.Flooded);
                });
        }

        void IDisposable.Dispose()
        {
            _brokeSync?.Dispose();
            Dispose();
        }
    }
}