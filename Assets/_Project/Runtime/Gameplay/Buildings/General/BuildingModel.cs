using System;
using Runtime.Gameplay.Colonizers;
using Runtime.Gameplay.Planets;
using Zenject;

namespace Runtime.Gameplay.Buildings.General
{
    public abstract class BuildingModel<T> : IBuildingModel
        where T : BuildingConditionalConfig
    {
        public bool Enabled => _enabled;
        public bool Broken => _broken;
        
        [Inject] protected BuildingView _view;
        [Inject] protected T _config;

        [Inject] protected Planet _planet;
        [Inject] protected ColonizersModel _colonizers;

        private bool _enabled;
        private bool _broken;
        
        public void Build()
        {
            _colonizers.Minerals.ApplyPurchase(_config.MineralsCost);
            SetEnabled(true);
        }

        public void Delete()
        {
            SetEnabled(false);
            _colonizers.Minerals.AddMinerals(_config.MineralsCost / 2, out _);
        }

        public void Flood()
        {
            if (!_broken)
            {
                _broken = true;
                SetEnabled(false);
            }
        }

        public void Repair()
        {
            if (_broken)
            {
                _broken = false;
                SetEnabled(true);
            }
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
        
        void IDisposable.Dispose() => Dispose();
    }
}