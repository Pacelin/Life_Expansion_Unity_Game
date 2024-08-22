using System.Collections.Generic;
using Runtime.Gameplay.Buildings.Builder;
using Runtime.Gameplay.Colonizers;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Buildings.General
{
    public class BuildingsValidator : ITickable
    {
        private readonly BuildingFactory _factory;
        private readonly ColonizersModel _colonizers;
        
        public BuildingsValidator(BuildingFactory factory, ColonizersModel colonizers)
        {
            _factory = factory;
            _colonizers = colonizers;
        }

        public void Tick()
        {
            var freeEnergy = _colonizers.Energy.Energy.CurrentValue - 
                             _colonizers.Energy.EnergyUsage.CurrentValue;
            var freeColonizers = (int) _colonizers.Population.CurrentPopulation.CurrentValue -
                                 _colonizers.Population.BusyPopulation.CurrentValue;

            foreach (var building in _factory.Buildings)
                if (building.IsNew || !building.Enabled)
                    CheckActivation(building, ref freeEnergy, ref freeColonizers);
            for (int i = _factory.Buildings.Count - 1; i >= 0; i--)
                if (_factory.Buildings[i].IsNew || _factory.Buildings[i].Enabled)
                    CheckDeactivation(_factory.Buildings[i], ref freeEnergy, ref freeColonizers);
        }

        private void CheckActivation(IBuildingModel building, ref int freeEnergy, ref int freeColonizers)
        {
            if (freeEnergy >= building.EnergyCost || building.EnergyCost == 0)
                building.CancelState(EBuildingState.NoEnergy);
            if (freeColonizers >= building.ColonizersCost || building.ColonizersCost == 0)
                building.CancelState(EBuildingState.NoColonists);

            if (building.IsNew)
            {
                building.IsNew = false;
                building.SetState(EBuildingState.Active);
            }
            if (building.Enabled)
            {
                freeEnergy -= building.EnergyCost;
                freeColonizers -= building.ColonizersCost;
            }
        }
        private void CheckDeactivation(IBuildingModel building, ref int freeEnergy, ref int freeColonizers)
        {
            var deactivationStates = new List<EBuildingState>();
            if (building.IsNew)
            {
                if (freeEnergy < building.EnergyCost && building.EnergyCost > 0)
                    deactivationStates.Add(EBuildingState.NoEnergy);
                if (freeColonizers < building.ColonizersCost && building.ColonizersCost > 0)
                    deactivationStates.Add(EBuildingState.NoColonists);
                deactivationStates.Add(EBuildingState.Active);
                building.IsNew = false;
                building.SetState(deactivationStates.ToArray());
            }                
            else
            {
                if (freeEnergy < 0 && building.EnergyCost > 0)
                    deactivationStates.Add(EBuildingState.NoEnergy);
                if (freeColonizers < 0 && building.ColonizersCost > 0)
                    deactivationStates.Add(EBuildingState.NoColonists); 
                building.SetState(deactivationStates.ToArray());
                if (!building.Enabled)
                {
                    freeEnergy += building.EnergyCost;
                    freeColonizers += building.ColonizersCost;
                }
            }
        }
    }
}