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
        public float _MxLeaksDueToDamage => _maxLeaksDueToDamage;

        public override string[] GetSpecificDescription()
        {
            return new string[1] { "Fuel tank volume: " + _volume.ToString() + " l" };
        }

        public override string[] GetPerformanceDescription()
        {
            return new string[2] {  "Total performance: " + ((int)(_durability * 100)).ToString() + " %",
                                    "Additional usage of fuel due to leaks: " + (_maxLeaksDueToDamage * (1 - _durability)).ToString("0.00") + " l/s",
                                 };
        }
    }
}