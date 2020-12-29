using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Configurable Modules/Cooling")]
    public class CoolingConfigurableModule : ConfigurableModule
    {
        [SerializeField] private float _effectiveCooling = 0f;

        public override string Name => "Cooling";

        public override string Description
        {
            get
            {
                return "Effective cooling: " + _effectiveCooling.ToString("0.0") + " for additional power usage: " + _powerFlowOnEnable.ToString("0.0") + " MJ/s";
            }
        }

        public override void Disable(IEquipmentOwner equipmentOwner, float durability)
        {
            base.Disable(equipmentOwner, durability);
            equipmentOwner.EffectiveCooling -= (_effectiveCooling * durability);
            equipmentOwner.PowerUsage -= _powerFlowOnEnable;
        }

        public override void Enable(IEquipmentOwner equipmentOwner, float durability)
        {
            base.Enable(equipmentOwner, durability);
            equipmentOwner.EffectiveCooling += (_effectiveCooling * durability);
            equipmentOwner.PowerUsage += _powerFlowOnEnable;
        }

        public override string GetPerformanceDescription(float durability)
        {
            return "Effective cooling: " + (_effectiveCooling * durability).ToString("0.0") + " for additional power usage: " + _powerFlowOnEnable.ToString("0.0") + " MJ/s";
        }
    }
}