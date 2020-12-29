using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public abstract class ConfigurableModule : ScriptableObject
    {
        [SerializeField] protected float _powerFlowOnEnable = 0f;
        [System.NonSerialized] protected bool _enabled = false;

        public abstract string Name { get; }
        public abstract string Description { get; }
        public float PowerFlowOnEnable => _powerFlowOnEnable;
        public bool Enabled => _enabled;

        public abstract string GetPerformanceDescription(float durability);
        public virtual void Enable(IEquipmentOwner equipmentOwner, float durability) { _enabled = true; }
        public virtual void Disable(IEquipmentOwner equipmentOwner, float durability) { _enabled = false; }

        private void OnEnable()
        {
            _enabled = false;
        }
    }
}