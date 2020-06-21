using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Battery Reference Part")]
    public class BatteryReferencePart : ReferencePart
    {
        [SerializeField] private int _availableCells = 0;
        [Tooltip("Damaged permanently if engine worked in temperature above")]
        [SerializeField, Range(100, 600)] private int _thermalVulnerability = 100;

        public int ThermalVulnerability => _thermalVulnerability;
        public int AvailableCells => _availableCells;

        public override string[] GetSpecificDescription()
        {
            return new string[2] {  "Available cells: " + _availableCells.ToString() + " HP",
                                    "Operating temp. up to " + _thermalVulnerability.ToString() + " C deg"};
        }

        public override string[] GetPerformanceDescription()
        {
            return new string[3] {  "Total performance: " + ((int)(_durability * 100)).ToString() + " %",
                                    "Available cells: " + _availableCells.ToString() + " (" + (_availableCells *_durability).ToString() + ")",
                                    "Operating temp. up to " + _thermalVulnerability.ToString() + " C deg"
                                 };
        }
    }
}