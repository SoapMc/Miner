using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Miner.Gameplay
{
    public class Part
    {
        private ReferencePart _referencePart;
        private float _durability;
        private IEquipmentOwner _owner;
        private bool _enabled = false;

        public string Name => _referencePart.Name;
        public Sprite Sprite => _referencePart.Sprite;
        public EPartType Type => _referencePart.Type;
        public int Id => _referencePart.Id;
        public int Cost => _referencePart.Cost;
        public string ShortDescription => _referencePart.ShortDescription;
        public float Durability
        {
            get => _durability;
            set
            {
                IConfigurablePart configurable = _referencePart as IConfigurablePart;
                Unequip();
                if (configurable != null)
                    configurable.Disable(_owner, _durability);
                _durability = Mathf.Clamp(value, 0.01f, 1f);
                Equip(_owner);
                if (configurable != null)
                    configurable.Enable(_owner, _durability);

            }
        }
        
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_referencePart is IConfigurablePart configurable)
                {
                    if (_enabled != value)
                    {
                        if (value == true)
                        {
                            configurable.Enable(_owner, _durability);
                        }
                        else
                        {
                            configurable.Disable(_owner, _durability);
                        }
                        _enabled = value;
                    }
                }
            }
        }

        public Part(ReferencePart part, float durability = 1f)
        {
            _referencePart = part;
            _durability = durability;
        }

        public string[] GetSpecificDescription() { return _referencePart.GetSpecificDescription(); }
        public string[] GetPerformanceDescription() { return _referencePart.GetPerformanceDescription(_durability); }
        public void Equip(IEquipmentOwner equipmentOwner)
        {
            _owner = equipmentOwner;
            _referencePart.Equip(equipmentOwner, _durability);
        }

        public void Unequip()
        {
            if(_referencePart is IConfigurablePart configurable)
            {
                if(Enabled == true)
                    configurable.Disable(_owner, _durability);
            }

            _referencePart.Unequip(_owner, _durability);
        }

        public bool IsConfigurable()
        {
            if (_referencePart as IConfigurablePart != null)
                return true;
            else
                return false;
        }

        public IConfigurablePart GetConfigurableComponent()
        {
            return _referencePart as IConfigurablePart;
        }

        public static explicit operator HullReferencePart(Part part)
        {
            if (part._referencePart is HullReferencePart hull)
                return hull;
            return null;
        }

        public static explicit operator EngineReferencePart(Part part)
        {
            if (part._referencePart is EngineReferencePart engine)
                return engine;
            return null;
        }

        public static explicit operator CoolingReferencePart(Part part)
        {
            if (part._referencePart is CoolingReferencePart cooling)
                return cooling;
            return null;
        }

        public static explicit operator BatteryReferencePart(Part part)
        {
            if (part._referencePart is BatteryReferencePart battery)
                return battery;
            return null;
        }

        public static explicit operator DrillReferencePart(Part part)
        {
            if (part._referencePart is DrillReferencePart drill)
                return drill;
            return null;
        }

        public static explicit operator CargoReferencePart(Part part)
        {
            if (part._referencePart is CargoReferencePart cargo)
                return cargo;
            return null;
        }
    }
}