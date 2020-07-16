using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Hull Reference Part")]
    public class HullReferencePart : ReferencePart
    {
        [SerializeField] private int _maxHull = 0;
        [SerializeField] private int _resistanceToHit = 0;
        [SerializeField, Range(0, 50)] private int _thermalInsulation = 0;
        [Tooltip("Damaged permanently if hull (in %) is below")]
        [SerializeField, Range(0f, 1f)] private float _permaDamageThreshold = 0.5f;

        public int MaxHull => _maxHull;
        public int ResistanceToHit => _resistanceToHit;
        public int ThermalInsulation => _thermalInsulation;
        public float PermaDamageThreshold => _permaDamageThreshold;

        public override string[] GetSpecificDescription()
        {
            return new string[3] {  "Max hull: " + _maxHull.ToString(),
                                    "Thermal insulation: " + _thermalInsulation.ToString(),
                                    "Resistance to hit: " + _resistanceToHit.ToString() };
        }

        public override string[] GetPerformanceDescription()
        {
            return new string[5] {  "Total performance: " + ((int)(_durability * 100)).ToString() + " %",
                                    "Max hull: " + _maxHull.ToString() + " (" + ((int)(_maxHull *_durability)).ToString() + ")",
                                    "Resistance to hit: " + _resistanceToHit.ToString() + " (" + ((int)(_resistanceToHit *_durability)).ToString() + ")",
                                    "Thermal insulation: " + _thermalInsulation.ToString(),
                                    "Damaged permenently when hit above " + ((int)(_maxHull * _permaDamageThreshold)).ToString() + " damage" };
        }
    }
}