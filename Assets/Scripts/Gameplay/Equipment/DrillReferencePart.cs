﻿using System.Collections;
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

        public override string[] GetPerformanceDescription(float durability)
        {
            return new string[2] {  "Total performance: " + ((int)(durability * 100)).ToString() + " %",
                                    "Drill sharpness: " + _sharpness.ToString() + " (" + (_sharpness * durability).ToString() + ")"
                                 };
        }

        public override void Equip(IEquipmentOwner playerStats, float durability)
        {
            playerStats.DrillSharpness += (Sharpness * durability);
        }

        public override void Unequip(IEquipmentOwner playerStats, float durability)
        {
            playerStats.DrillSharpness -= (Sharpness * durability);
        }
    }
}