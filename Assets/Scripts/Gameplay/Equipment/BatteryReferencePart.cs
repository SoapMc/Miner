using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Battery Reference Part")]
    public class BatteryReferencePart : ReferencePart
    {
        [SerializeField] private int _availableCells = 0;
        
        public int AvailableCells => _availableCells;
    }
}