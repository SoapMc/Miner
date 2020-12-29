using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public interface  IEquipmentOwner
    {
        //battery
        float MaxPower { get; set; }
        float Power { get; set; }
        float PowerUsage { get; set; }

        //hull
        int MaxHull { get; set; }
        int Hull { get; set; }
        int ResistanceToHit { get; set; }
        int ThermalInsulation { get; set; }
        float HullPermaDamageThreshold { get; set; }

        //fuel tank
        float MaxFuel { get; set; }
        float Fuel { get; set; }

        //cooling
        float EffectiveCooling { get; set; }

        //engine
        int EnginePower { get; set; }
        float FuelUsage { get; set; }

        //drill
        float DrillSharpness { get; set; }

        //cargo
        int CargoMaxMass { get; set; }
        float ChanceForLoseResource { get; set; }
        int RadiationTolerance { get; set; }
    }
}