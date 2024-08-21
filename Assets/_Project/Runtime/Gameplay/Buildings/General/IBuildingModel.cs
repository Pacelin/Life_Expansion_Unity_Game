using System;

namespace Runtime.Gameplay.Buildings.General
{
    public interface IBuildingModel : IDisposable
    {
        int EnergyCost { get; }
        int ColonizersCost { get; }
        bool Enabled { get; }
        bool IsNew { get; set; }
        BuildingView View { get; }
        void Build();
        void SetState(params EBuildingState[] states);
        void CancelState(EBuildingState state);
        void Delete();
    }
}