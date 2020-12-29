using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Miner.Management;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment Table")]
    public class EquipmentTable : ScriptableObject
    {
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
            if(_equipment.ContainsKey(partType))
                return _equipment[partType];
            else
                return null;
        }

        public void SetEquippedPart(Part part)
        {
            if (part != null)
            {
                if (!_equipment.ContainsKey(part.Type))
                    Log.Instance.WriteException(new KeyNotFoundException("Key: " + part.Type));
                _equipment[part.Type] = part;
            }
        }

        public void UnequipPart(EPartType partType)
        {
            try
            {
                _equipment[partType] = null;
            }
            catch
            {
                Log.Instance.WriteException(new KeyNotFoundException("Key: " + partType));
            }
        }
    }
}