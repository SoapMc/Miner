using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Engine Reference Part")]
    public class EngineReferencePart : ReferencePart
    {
        [SerializeField] private int _power = 0;
        [SerializeField] private float _fuelUsage = 0;

        [Tooltip("Damaged permanently if engine worked in temperature above")]
        [SerializeField, Range(100, 600)] private int _permaDamageTemperatureThreshold = 100;
        [Tooltip("Damaged permanently if engine worked with resources load above")]
        [SerializeField] private int _permaDamageLoadThreshold = 0;

        public int Power => _power;
        public float FuelUsage => _fuelUsage;

        public override string[] GetSpecificDescription()
        {
            return new string[1] { "Engine power: " + _power.ToString() + " HP" };
        }

        public override string[] GetPerformanceDescription()
        {
            return new string[2] {  "Total performance: " + ((int)(_durability * 100)).ToString() + " %",
                                    "Engine power: " + _power.ToString() + " (" + (_power *_durability).ToString() + ")",
                                 };
        }

    }
}