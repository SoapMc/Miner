using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Hull Reference Part")]
    public class HullReferencePart : ReferencePart
    {
        [SerializeField] private int _maxHull = 0;
        [Tooltip("Damaged permanently if hull (in %) is below")]
        [SerializeField, Range(0f, 1f)] private float _permaDamageThreshold = 0.5f;

        public int MaxHull => _maxHull;

        public override string[] GetSpecificDescription()
        {
            return new string[1] {"Max hull: " + _maxHull.ToString() };
        }

        public override string[] GetPerformanceDescription()
        {
            return new string[3] {  "Total performance: " + ((int)(_durability * 100)).ToString() + " %",
                                    "Max hull: " + _maxHull.ToString() + " (" + (_maxHull *_durability).ToString() + ")",
                                    "Damaged permenently when hit above " + (_maxHull * _permaDamageThreshold).ToString() + " damage" };
        }
    }
}