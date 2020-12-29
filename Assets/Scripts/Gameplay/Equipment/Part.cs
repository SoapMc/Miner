using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Miner.Gameplay
{
    public class Part
    {
        private ReferencePart _referencePart;
        private List<ConfigurableModule> _configurableModules = new List<ConfigurableModule>();
        private float _durability;
        private IEquipmentOwner _owner;

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
                bool[] enabledModules = GetWhichModulesAreEnabled();
                Unequip();
                _durability = Mathf.Clamp(value, 0.01f, 1f);
                Equip(_owner);
                RestoreEnabledModules(enabledModules);
            }
        }
        public List<ConfigurableModule> ConfigurableModules => _configurableModules;

        public Part(ReferencePart part, float durability = 1f)
        {
            _referencePart = part;
            _durability = durability;
            foreach(ConfigurableModule module in _referencePart.ConfigurableModules)
            {
                _configurableModules.Add(GameObject.Instantiate(module));
            }
        }

        public string GetSpecificDescription() => _referencePart.GetOfferDescription();
        public string GetPerformanceDescription() => _referencePart.GetPerformanceDescription(_durability);
        public void Equip(IEquipmentOwner equipmentOwner)
        {
            _owner = equipmentOwner;
            _referencePart.Equip(equipmentOwner, _durability);
        }
        public void Unequip()
        {
            DisableAllConfigurableModules();
            _referencePart.Unequip(_owner, _durability);
        }
        public bool IsOverheatable()
        {
            if (_referencePart as IOverheatable != null)
                return true;
            else
                return false;
        }
        public IOverheatable AsOverheatable()
        {
            return _referencePart as IOverheatable;
        }
        public void EnableConfigurableModule(ConfigurableModule module)
        {
            if (_configurableModules.Exists(x => x == module))
            {
                _configurableModules.Find(x => x == module).Enable(_owner, _durability);
            }
        }
        public void DisableConfigurableModule(ConfigurableModule module)
        {
            if (_configurableModules.Exists(x => x == module))
            {
                ConfigurableModule cm = _configurableModules.Find(x => x == module);
                if(cm.Enabled)
                    cm.Disable(_owner, _durability);
            }
        }
        public void EnableAllConfigurableModules()
        {
            foreach (var module in _configurableModules)
            {
                module.Enable(_owner, _durability);
            }
        }
        public void DisableAllConfigurableModules()
        {
            foreach (var module in _configurableModules)
            {
                if(module.Enabled)
                    module.Disable(_owner, _durability);
            }
        }
        public void DisableAllConfigurableLoads()
        {
            foreach (var module in _configurableModules)
            {
                if (module.PowerFlowOnEnable < 0f && module.Enabled)
                    module.Disable(_owner, _durability);
            }
        }
        private bool[] GetWhichModulesAreEnabled()
        {
            bool[] enabledModules = new bool[_configurableModules.Count];
            for(int i = 0; i < _configurableModules.Count; ++i)
            {
                enabledModules[i] = _configurableModules[i].Enabled;
            }
            return enabledModules;
        }
        private void RestoreEnabledModules(bool[] enabledModules)
        {
            for (int i = 0; i < _configurableModules.Count; ++i)
            {
                if (enabledModules[i] == true)
                    _configurableModules[i].Enable(_owner, _durability);
                else if (_configurableModules[i].Enabled)
                    _configurableModules[i].Disable(_owner, _durability);
            }
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