using System;

namespace Runtime.Gameplay.Buildings.General
{
    public interface IBuildingModel : IDisposable
    {
        BuildingView View { get; }
        bool Enabled { get; }
        bool Broken { get; }
        bool IsWrongTerritory { get; }
        void Build();
        void Delete();
        void Repair();
    }
}