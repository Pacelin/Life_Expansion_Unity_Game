using System;
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
        public BuildingView View => _view;
        public bool Enabled => _enabled;
        public bool Broken => _broken;
        public bool IsWrongTerritory => _isWrongTerritory;
        
        [Inject] protected BuildingView _view;
        [Inject] protected T _config;

        [Inject] protected Planet _planet;
        [Inject] protected ColonizersModel _colonizers;
        [Inject] protected BuildService _buildService;
        [Inject] protected BuildingBubbleConfig _bubbleConfig;

        private bool _enabled;
        private bool _broken;
        private bool _isWrongTerritory;
        private IDisposable _brokeSync;
        
        public void Build()
        {
            _colonizers.Minerals.ApplyPurchase(_config.MineralsCost);
            if (_config.BuildTerritory == EBuildTerritory.Water)
                _brokeSync = WaterSync();
            else
                _brokeSync = GroundSync();
            SetEnabled(true);
        }

        public void Delete()
        {
            SetEnabled(false);
            _colonizers.Minerals.AddMinerals(_config.MineralsCost / 2, out _);
        }

        public void SetEnabled(bool enabled)
        {
            if (_enabled == enabled)
                return;
            _enabled = enabled;

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
        
        protected abstract void OnEnable();
        protected abstract void OnDisable();
        protected abstract void Dispose();

        private void Broke()
        {
            if (!_broken)
            {
                _broken = true;
                SetEnabled(false);
                if (_config.BuildTerritory == EBuildTerritory.Water)
                    _view.Bubble.ShowBubble(EBubbleIcon.Warning, _bubbleConfig.NoWaterText, ECursorIcon.Warning);
                else
                    _view.Bubble.ShowBubble(EBubbleIcon.Warning, _bubbleConfig.FloodText, ECursorIcon.Warning);
            }
        }
        
        private void Repair()
        {
            if (_broken)
            {
                _broken = false;
                SetEnabled(true);
                _view.Bubble.Hide();
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
                    _isWrongTerritory = territory != EBuildTerritory.Water;
                    if (_isWrongTerritory && !_broken)
                        Broke();
                    else if (!_isWrongTerritory && _broken)
                        Repair();
                });
        }
        
        private IDisposable GroundSync()
        {
            return _planet.Water.Value
                .Subscribe(_ =>
                {
                    var territory = _buildService.CheckBuildTerritory(_view.transform.position, out var _);
                    _isWrongTerritory = territory != EBuildTerritory.Ground;
                    if (_isWrongTerritory && !_broken)
                        Broke();
                });
        }

        void IDisposable.Dispose()
        {
            _brokeSync?.Dispose();
            Dispose();
        }
    }
}