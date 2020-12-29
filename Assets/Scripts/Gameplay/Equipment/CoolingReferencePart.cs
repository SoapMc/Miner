using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Cooling Reference Part")]
    public class CoolingReferencePart : ReferencePart
    {
        public override string GetOfferDescription()
        {
            return "It needs to be enabled first!" + base.GetOfferDescription();
        }

        public override string GetPerformanceDescription(float durability)
        {
            return "Total performance: " + ((int)(durability * 100)).ToString() + base.GetPerformanceDescription(durability);
        }

        public override void Equip(IEquipmentOwner playerStats, float durability)
        {
            
        }

        public override void Unequip(IEquipmentOwner playerStats, float durability)
        {
            
        }
    }
}