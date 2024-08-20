using System;
using R3;
using Runtime.Gameplay.Colonizers;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Conditions
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings Unlocks/Population Condition")]
    public class PopulationCondition : BuildingUnlockCondition
    {
        [SerializeField] private int _targetPopulation;
        
        public override IDisposable Subscribe(DiContainer container, Action<bool> observable)
        {
            var colonizers = container.Resolve<ColonizersModel>();
            if (colonizers.Population.CurrentPopulation.CurrentValue < _targetPopulation)
            {
                observable(false);
                return colonizers.Population.CurrentPopulation
                    .Where(v => v >= _targetPopulation)
                    .Take(1)
                    .Subscribe(_ => observable(true));
            }
            observable(true);
            return Disposable.Empty;
        }
    }
}