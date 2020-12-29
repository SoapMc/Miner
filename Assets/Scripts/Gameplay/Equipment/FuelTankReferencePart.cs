using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Fuel Tank Reference Part")]
    public class FuelTankReferencePart : ReferencePart
    {
        [SerializeField] private float _volume = 0f;
        [SerializeField, Range(0, 5f)] private float _maxLeaksDueToDamage = 1f;

        public float Volume => _volume;
        public float MaxLeaksDueToDamage => _maxLeaksDueToDamage;

        public override string GetOfferDescription()
        {
            return "Fuel tank volume: " + _volume.ToString() + " l";
        }

        public override string GetPerformanceDescription(float durability)
        {
            return "Total performance: " + ((int)(durability * 100)).ToString() + " %" +
                   "\nAdditional usage of fuel due to leaks: " + (_maxLeaksDueToDamage * (1 - durability)).ToString("0.00") + " l/s";
        }

        public override void Equip(IEquipmentOwner equipmentOwner, float durability)
        {
            equipmentOwner.MaxFuel += Volume;
            equipmentOwner.Fuel = Mathf.Clamp(equipmentOwner.Fuel, 0, equipmentOwner.MaxFuel);
            equipmentOwner.FuelUsage += _maxLeaksDueToDamage * (1f - durability);
        }

        public override void Unequip(IEquipmentOwner equipmentOwner, float durability)
        {
            equipmentOwner.MaxFuel -= Volume;
            equipmentOwner.FuelUsage -= _maxLeaksDueToDamage * (1f - durability);
        }
    }
}