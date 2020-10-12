using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Cooling Reference Part")]
    public class CoolingReferencePart : ReferencePart, IConfigurablePart
    {
        [SerializeField] private float _effectiveCooling = 0f;
        [SerializeField, Range(0f, 20f)] private float _powerUsage = 0f;
        public float EffectiveCooling => _effectiveCooling;
        public float PowerUsage => _powerUsage;

        public string ConfigurationName => "Active cooling system";

        public string ConfigurationDescription
        {
            get
            {
                return "Increases power usage: " + _powerUsage.ToString("0.0") + " MJ/s" +
                    "\nIncreases effective cooling by " + _effectiveCooling.ToString("0.0");
            }
        }

        public bool UsesPower => true;

        public override string[] GetSpecificDescription()
        {
            return new string[3] { "It needs to be enabled first!",
                                   "Effective cooling: " + _effectiveCooling.ToString("0.0"),
                                   "Power usage: " + _powerUsage.ToString("0.0")
                                 };
        }

        public override string[] GetPerformanceDescription(float durability)
        {
            return new string[3] {  "Total performance: " + ((int)(durability * 100)).ToString(),
                                    "Effective cooling: " + _effectiveCooling.ToString("0.0") + " (" + (_effectiveCooling * durability).ToString("0.0") + ")",
                                    "Power usage: " + _powerUsage.ToString("0.0")
                                 };
        }

        public override void Equip(IEquipmentOwner playerStats, float durability)
        {
            //playerStats.EffectiveCooling += (EffectiveCooling * durability);
        }

        public override void Unequip(IEquipmentOwner playerStats, float durability)
        {
            //playerStats.EffectiveCooling -= (EffectiveCooling * durability);
        }

        public void Enable(IEquipmentOwner equipmentOwner, float durability)
        {
            equipmentOwner.EffectiveCooling += (EffectiveCooling * durability);
            equipmentOwner.PowerUsage += PowerUsage;
        }

        public void Disable(IEquipmentOwner equipmentOwner, float durability)
        {
            equipmentOwner.EffectiveCooling -= (EffectiveCooling * durability);
            equipmentOwner.PowerUsage -= PowerUsage;
        }
    }
}