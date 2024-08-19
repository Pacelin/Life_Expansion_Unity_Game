using System;

namespace Runtime.Gameplay.Buildings.General
{
    public interface IBuildingModel : IDisposable
    {
        bool Enabled { get; }
        bool Broken { get; }
        void Build();
        void Delete();
        void Flood();
        void Repair();
    }
}