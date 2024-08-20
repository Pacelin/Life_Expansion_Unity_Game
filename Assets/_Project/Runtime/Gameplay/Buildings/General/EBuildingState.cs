using System;

namespace Runtime.Gameplay.Buildings.General
{
    [Flags]
    public enum EBuildingState
    {
        None = 0,
        Active = 1,
        Flooded = 2,
        NoWater = 4,
        NoEnergy = 8,
        NoColonists = 16
    }
}