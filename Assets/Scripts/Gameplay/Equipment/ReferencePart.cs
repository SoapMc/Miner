using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public abstract class ReferencePart : ScriptableObject
    {
        [SerializeField] protected string _name;
        [SerializeField] protected Sprite _sprite;
        [SerializeField] protected EPartType _type;
        [SerializeField] protected int _id;
        [SerializeField] protected int _cost;
        [SerializeField, TextArea] protected string _shortDescription;
        [SerializeField] protected List<ConfigurableModule> _configurableModules = new List<ConfigurableModule>();

        public string Name => _name;
        public Sprite Sprite => _sprite;
        public EPartType Type => _type;
        public int Id => _id;
        public int Cost => _cost;
        public string ShortDescription => _shortDescription;
        public List<ConfigurableModule> ConfigurableModules => _configurableModules;

        public virtual string GetOfferDescription()
        {
            string result = string.Empty;
            for (int i = 0; i < _configurableModules.Count; ++i)
            {
                result += "\n" + _configurableModules[i].Description;
            }
            return result;
        }
        public virtual string GetPerformanceDescription(float durability)
        {
            string result = string.Empty;
            for(int i = 0; i < _configurableModules.Count; ++i)
            {
                result += "\n" + _configurableModules[i].GetPerformanceDescription(durability);
            }
            return result;
        }
        public abstract void Equip(IEquipmentOwner playerStats, float durability);
        public abstract void Unequip(IEquipmentOwner playerStats, float durability);

        public void EnableConfigurableModule(ConfigurableModule module, IEquipmentOwner equipmentOwner, float durability)
        {
            if(_configurableModules.Exists(x => x == module))
            {
                _configurableModules.Find(x => x == module).Enable(equipmentOwner, durability);
            }
        }
        public void DisableConfigurableModule(ConfigurableModule module, IEquipmentOwner equipmentOwner, float durability)
        {
            if (_configurableModules.Exists(x => x == module))
            {
                _configurableModules.Find(x => x == module).Disable(equipmentOwner, durability);
            }
        }
        public void EnableAllConfigurableModules(IEquipmentOwner equipmentOwner, float durability)
        {
            foreach (var module in _configurableModules)
            {
                module.Enable(equipmentOwner, durability);
            }
        }
        public void DisableAllConfigurableModules(IEquipmentOwner equipmentOwner, float durability)
        {
            foreach (var module in _configurableModules)
            {
                module.Disable(equipmentOwner, durability);
            }
        }
        public void DisableAllConfigurableLoads(IEquipmentOwner equipmentOwner, float durability)
        {
            foreach (var module in _configurableModules)
            {
                if(module.PowerFlowOnEnable < 0f)
                    module.Disable(equipmentOwner, durability);
            }
        }

        public Part CreatePart(float durability = 1f)
        {
            return new Part(this, durability);
        }

        public static Part CreatePart(ReferencePart referencePart, float durability = 1f)
        {
            if (referencePart != null)
                return referencePart.CreatePart(durability);
            return null;
        }
    }
}