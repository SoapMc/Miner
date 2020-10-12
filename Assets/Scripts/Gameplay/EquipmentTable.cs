using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment Table")]
    public class EquipmentTable : ScriptableObject
    {
        private IEquipmentOwner _owner = null;
        private Dictionary<EPartType, Part> _equipment = new Dictionary<EPartType, Part>();

        private void OnEnable()
        {
            Clear();
        }

        public void Clear()
        {
            foreach (EPartType partType in Enum.GetValues(typeof(EPartType)))
            {
                _equipment[partType] = null;
            }
        }

        public Part GetEquippedPart(EPartType partType)
        {
            return _equipment[partType];
        }

        public void SetEquippedPart(Part part)
        {
            _equipment[part.Type] = part;
        }
    }
}