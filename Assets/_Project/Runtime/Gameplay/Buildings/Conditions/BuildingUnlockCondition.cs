using System;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace Runtime.Gameplay.Buildings.Conditions
{
    public abstract class BuildingUnlockCondition : ScriptableObject
    {
        public string LockingDescription => _lockingDescription.GetLocalizedString();
        
        [SerializeField] private LocalizedString _lockingDescription;
        
        public abstract IDisposable Subscribe(DiContainer container, Action<bool> observer);
    }
}