using System;
using R3;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings
{
    public abstract class BuildingUnlockCondition : ScriptableObject
    {
        public abstract IDisposable Subscribe(DiContainer container, Action<bool> observer);
    }

    [CreateAssetMenu(menuName = "Gameplay/Buildings/No Unlock Condition")]
    public class NoUnlockCondition : BuildingUnlockCondition
    {
        public override IDisposable Subscribe(DiContainer container, Action<bool> observable)
        {
            observable(true);
            return Disposable.Empty;
        }
    }
    
    [CreateAssetMenu(menuName = "Gameplay/Buildings/Population Condition")]
    public class PopulationCondition : BuildingUnlockCondition
    {
        [SerializeField] private int _targetPopulation;
        
        public override IDisposable Subscribe(DiContainer container, Action<bool> observable)
        {
            observable(true);
            return Disposable.Empty;
        }
    }
}