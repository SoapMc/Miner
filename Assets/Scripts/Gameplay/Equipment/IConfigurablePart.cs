using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public interface IConfigurablePart
    {
        string ConfigurationName { get; }
        string ConfigurationDescription { get; }
        bool UsesPower { get; }

        void Enable(IEquipmentOwner equipmentOwner, float durability);
        void Disable(IEquipmentOwner equipmentOwner, float durability);
    }
}