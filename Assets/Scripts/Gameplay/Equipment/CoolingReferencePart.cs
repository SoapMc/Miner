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
    }
}