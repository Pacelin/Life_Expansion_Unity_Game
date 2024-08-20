using System;

namespace Runtime.Gameplay.Buildings.General
{
    [Flags]
    public enum EBuildingState
    {
        Active = 1,
        Flooded = 2,
        NoWater = 4,
        NoEnergy = 8,
        NoColonists = 16
    }
}