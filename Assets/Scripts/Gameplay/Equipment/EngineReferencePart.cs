using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Engine Reference Part")]
    public class EngineReferencePart : ReferencePart, IConfigurablePart, IOverheatable
    {
        [SerializeField] private int _power = 0;
        [SerializeField] private float _fuelUsage = 0;
        [SerializeField] private float _powerCharging = 0;
        [SerializeField] private float _additionalFuelUsageWhenCharging = 0f;

        [Tooltip("Damaged permanently if engine worked in temperature above")]
        [SerializeField, Range(100, 600)] private int _maximumOperatingTemeprature = 100;
        [Tooltip("Damaged permanently if engine worked with resources load above")]
        [SerializeField] private int _permaDamageLoadThreshold = 0;

        public int Power => _power;
        public float FuelUsage => _fuelUsage;
        public float PowerCharging => _powerCharging;
        public int MaximumOperatingTemperature => _maximumOperatingTemeprature;
        public float ToleranceTime => 10f;

        public string ConfigurationName => "Engine as power generator";

        public string ConfigurationDescription
        {
            get
            {
                return "Increases fuel usage: " + _additionalFuelUsageWhenCharging.ToString("0.00") + " l/s" +
                    "\nGenerates power: " + _powerCharging.ToString("0.0") + " MJ/s";
            }
        }

        public bool UsesPower => false;

        public override string[] GetSpecificDescription()
        {
            return new string[2] {  "Engine power: " + _power.ToString() + " HP",
                                    "Operating temp. up to " + _maximumOperatingTemeprature.ToString() + " C deg"};
        }

        public override string[] GetPerformanceDescription(float durability)
        {
            return new string[3] {  "Total performance: " + ((int)(durability * 100)).ToString() + " %",
                                    "Engine power: " + _power.ToString() + " (" + (_power * durability).ToString() + ")",
                                    "Operating temp. up to " + _maximumOperatingTemeprature.ToString() + " C deg"
                                 };
        }

        public override void Equip(IEquipmentOwner playerStats, float durability)
        {
            playerStats.EnginePower += (int)(Power * durability * 1000);
            playerStats.FuelUsage += FuelUsage;
        }

        public override void Unequip(IEquipmentOwner playerStats, float durability)
        {
            playerStats.EnginePower -= (int)(Power * durability * 1000);
            playerStats.FuelUsage -= FuelUsage;
        }

        public void Enable(IEquipmentOwner equipmentOwner, float durability)
        {
            equipmentOwner.FuelUsage += _additionalFuelUsageWhenCharging;
            equipmentOwner.PowerUsage -= _powerCharging;
        }

        public void Disable(IEquipmentOwner equipmentOwner, float durability)
        {
            equipmentOwner.FuelUsage -= _additionalFuelUsageWhenCharging;
            equipmentOwner.PowerUsage += _powerCharging;
        }
    }
}