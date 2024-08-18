using System;
using R3;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Conditions
{
    [CreateAssetMenu(menuName = "Gameplay/Buildings/No Unlock Condition")]
    public class NoUnlockCondition : BuildingUnlockCondition
    {
        public override IDisposable Subscribe(DiContainer container, Action<bool> observable)
        {
            observable(true);
            return Disposable.Empty;
        }
    }
}