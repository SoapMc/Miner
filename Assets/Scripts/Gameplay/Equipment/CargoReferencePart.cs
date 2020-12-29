using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Cargo Reference Part")]
    public class CargoReferencePart : ReferencePart
    {
        [SerializeField] private int _maxMass = 0;
        [SerializeField] private int _radiationTolerance = 0;

        public int MaxMass => _maxMass;
        public int RadiationTolerance => _radiationTolerance;


        public float ChanceForLoseResource(float durability)
        {
            return 1f - durability;
        }

        public override string GetOfferDescription()
        {
            return  "Capacity: " + _maxMass.ToString() + " kg" + 
                    "\nRadiation Tolerance: " + GetRadiationToleranceText(RadiationTolerance);
        }

        public override string GetPerformanceDescription(float durability)
        {
            return "Capacity: " + _maxMass.ToString() + " kg" +
                   "\nChance for lose resource when gathered: " + ((int)(ChanceForLoseResource(durability) * 100)).ToString() + " %" +
                   "\nRadiation Tolerance: " + GetRadiationToleranceText((int)(RadiationTolerance * durability));
                                 
        }

        public override void Equip(IEquipmentOwner playerStats, float durability)
        {
            playerStats.CargoMaxMass += MaxMass;
            playerStats.ChanceForLoseResource += ChanceForLoseResource(durability);
        }

        public override void Unequip(IEquipmentOwner playerStats, float durability)
        {
            playerStats.CargoMaxMass -= MaxMass;
            playerStats.ChanceForLoseResource -= ChanceForLoseResource(durability);
        }

        private string GetRadiationToleranceText(int radiationTolerance)
        {
            if (radiationTolerance < 1)
                return "None";
            else if (radiationTolerance >= 1 && radiationTolerance < 3)
                return "Low";
            else if (radiationTolerance >= 3 && radiationTolerance < 6)
                return "Moderate";
            else if (radiationTolerance >= 6 && radiationTolerance < 8)
                return "High";
            else
                return "Very High";
        }
    }
}