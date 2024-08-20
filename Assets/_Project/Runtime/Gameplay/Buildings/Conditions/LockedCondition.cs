using System;
using R3;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Conditions
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings Unlocks/Locked")]
    public class LockedCondition : BuildingUnlockCondition
    {
        public override IDisposable Subscribe(DiContainer container, Action<bool> observable)
        {
            observable(false);
            return Disposable.Empty;
        }
    }
}