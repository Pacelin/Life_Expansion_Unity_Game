using System;

namespace Runtime.Gameplay.Buildings.General
{
    public interface IBuildingModel : IDisposable
    {
        bool Enabled { get; }
        BuildingView View { get; }
        void Build();
        void SetState(params EBuildingState[] states);
        void CancelState(EBuildingState state);
        void Delete();
        bool EnoughEnergy();
        bool EnoughColonizers();
    }
}