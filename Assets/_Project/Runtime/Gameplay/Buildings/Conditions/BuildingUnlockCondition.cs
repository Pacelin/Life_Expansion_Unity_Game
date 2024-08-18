using System;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.Conditions
{
    public abstract class BuildingUnlockCondition : ScriptableObject
    {
        public abstract IDisposable Subscribe(DiContainer container, Action<bool> observer);
    }
}