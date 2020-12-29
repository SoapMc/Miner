using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Configurable Modules/Engine As Generator")]
    public class EngineAsGeneratorConfigurableModule : ConfigurableModule
    {
        [SerializeField] private float _fuelUsage = 0f;

        public override string Name => "Engine as power generator";

        public override string Description
        {
            get
            {
                return "Generates power: " + _powerFlowOnEnable.ToString("0.0") + " MJ/s for additional " + _fuelUsage.ToString() + " l/s";
            }
        }

        public override void Disable(IEquipmentOwner equipmentOwner, float durability)
        {
            base.Disable(equipmentOwner, durability);
            equipmentOwner.FuelUsage -= _fuelUsage;
            equipmentOwner.PowerUsage += (_powerFlowOnEnable * durability);
        }

        public override void Enable(IEquipmentOwner equipmentOwner, float durability)
        {
            base.Enable(equipmentOwner, durability);
            equipmentOwner.FuelUsage += _fuelUsage;
            equipmentOwner.PowerUsage -= (_powerFlowOnEnable * durability);
        }

        public override string GetPerformanceDescription(float durability)
        {
            return "Generates power: " + (_powerFlowOnEnable * durability).ToString("0.0") + " MJ/s for additional " + _fuelUsage.ToString() + " l/s";
        }
    }
}