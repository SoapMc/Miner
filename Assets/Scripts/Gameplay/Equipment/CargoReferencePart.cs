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

        public int MaxMass => _maxMass;

        public float ChanceForLoseResource()
        {
            return 1f - Durability;
        }

        public override string[] GetSpecificDescription()
        {
            return new string[1] { "Capacity: " + _maxMass.ToString() + " kg" };
        }

        public override string[] GetPerformanceDescription()
        {
            return new string[2] {  "Capacity: " + _maxMass.ToString() + " kg",
                                    "Chance for lose resource when gathered: " + ((int)(ChanceForLoseResource() * 100)).ToString() + " %",
                                 };
        }
    }
}