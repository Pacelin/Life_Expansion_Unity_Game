using Zenject;

namespace Runtime.Gameplay.Buildings.General
{
    public interface IBuildingModel
    {
        void Build();
        void Delete();
        void Flood();
    }
    public abstract class BuildingModel<T> : IBuildingModel
        where T : BuildingConditionalConfig
    {
        [Inject] protected BuildingView _view;
        [Inject] protected T _config;

        public void Build()
        {
            EnableBuilding();
        }

        public void Delete()
        {
            DisableBuilding();
        }

        public void Flood()
        {
            DisableBuilding();
        }
        
        protected abstract void EnableBuilding();
        protected abstract void DisableBuilding();
    }
}