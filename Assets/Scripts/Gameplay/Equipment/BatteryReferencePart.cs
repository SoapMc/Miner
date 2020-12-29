using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Battery Reference Part")]
    public class BatteryReferencePart : ReferencePart
    {
        [SerializeField] private int _maxPower = 0;
        [Tooltip("Damaged permanently if engine worked in temperature above")]
        [SerializeField, Range(100, 600)] private int _thermalVulnerability = 100;

        public int ThermalVulnerability => _thermalVulnerability;
        public int MaxPower => _maxPower;

        public override string GetOfferDescription()
        {
            return "Maximum power: " + _maxPower.ToString() + "\nOperating temp. up to " + _thermalVulnerability.ToString() + " C deg";
        }

        public override string GetPerformanceDescription(float durability)
        {
            return   "Total performance: " + ((int)(durability * 100)).ToString() + "%" +
                     "\nMaximum power: " + _maxPower.ToString() + " (" + (_maxPower * durability).ToString() + ")" +
                     "\nOperating temp. up to " + _thermalVulnerability.ToString() + " C deg";
        }

        public override void Equip(IEquipmentOwner equipmentOwner, float durability)
        {
            equipmentOwner.MaxPower += (int)(MaxPower * durability);
            if (durability == 1f)   //brand new
                equipmentOwner.Power = equipmentOwner.MaxPower;
            else
                equipmentOwner.Power = Mathf.Clamp(equipmentOwner.Power, 0, equipmentOwner.MaxPower);
        }

        public override void Unequip(IEquipmentOwner playerStats, float durability)
        {
            playerStats.MaxPower -= MaxPower * durability;
        }
    }
}