using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment Table")]
    public class EquipmentTable : ScriptableObject
    {
        public HullReferencePart Hull { get; set; }
        public FuelTankReferencePart FuelTank { get; set; }
        public BatteryReferencePart Battery { get; set; }
        public CargoReferencePart Cargo { get; set; }
        public EngineReferencePart Engine { get; set; }
        public CoolingReferencePart Cooling { get; set; }
        public DrillReferencePart Drill { get; set; }

        public void Clear()
        {
            Hull = null;
            FuelTank = null;
            Battery = null;
            Cargo = null;
            Engine = null;
            Cooling = null;
            Drill = null;
        }
    }
}