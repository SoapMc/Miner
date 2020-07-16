using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Cooling Reference Part")]
    public class CoolingReferencePart : ReferencePart
    {
        [SerializeField] private float _effectiveCooling = 0f;

        public float EffectiveCooling => _effectiveCooling;

        public override string[] GetSpecificDescription()
        {
            return new string[1] { "Effective cooling: " + _effectiveCooling.ToString("0.0") };
        }

        public override string[] GetPerformanceDescription()
        {
            return new string[2] {  "Total performance: " + ((int)(_durability * 100)).ToString(),
                                    "Effective cooling: " + _effectiveCooling.ToString("0.0") + " (" + (_effectiveCooling *_durability).ToString("0.0") + ")"
                                 };
        }
    }
}