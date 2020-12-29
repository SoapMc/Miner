using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Engine Reference Part")]
    public class EngineReferencePart : ReferencePart, IOverheatable
    {
        [SerializeField] private int _power = 0;
        [SerializeField] private float _fuelUsage = 0;
        

        [Tooltip("Damaged permanently if engine worked in temperature above")]
        [SerializeField, Range(100, 600)] private int _maximumOperatingTemeprature = 100;

        public int Power => _power;
        public float FuelUsage => _fuelUsage;
        public int MaximumOperatingTemperature => _maximumOperatingTemeprature;
        public float ToleranceTime => 10f;

        public override string GetOfferDescription()
        {
            return "Engine power: " + _power.ToString() + " HP" +
                   "\nOperating temp. up to " + _maximumOperatingTemeprature.ToString() + " C deg";
        }

        public override string GetPerformanceDescription(float durability)
        {
            return "Total performance: " + ((int)(durability * 100)).ToString() + " %" +
                   "Engine power: " + _power.ToString() + " (" + (_power * durability).ToString() + ")" +
                   "Operating temp. up to " + _maximumOperatingTemeprature.ToString() + " C deg" +
                   base.GetPerformanceDescription(durability);
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
    }
}