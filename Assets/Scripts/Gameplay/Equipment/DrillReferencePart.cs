using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Drill Reference Part")]
    public class DrillReferencePart : ReferencePart
    {
        [SerializeField, Range(0.1f, 4f)] private float _sharpness = 0.1f;

        public float Sharpness => _sharpness;

        public override string[] GetSpecificDescription()
        {
            return new string[1] { "Drill sharpness: " + _sharpness.ToString() };
        }

        public override string[] GetPerformanceDescription()
        {
            return new string[2] {  "Total performance: " + ((int)(_durability * 100)).ToString() + " %",
                                    "Drill sharpness: " + _sharpness.ToString() + " (" + (_sharpness *_durability).ToString() + ")"
                                 };
        }
    }
}