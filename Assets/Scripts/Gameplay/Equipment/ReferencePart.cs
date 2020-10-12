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

        public string Name => _name;
        public Sprite Sprite => _sprite;
        public EPartType Type => _type;
        public int Id => _id;
        public int Cost => _cost;
        public string ShortDescription => _shortDescription;

        public virtual string[] GetSpecificDescription() { return new string[0]; }
        public virtual string[] GetPerformanceDescription(float durability) { return new string[0]; }
        public abstract void Equip(IEquipmentOwner playerStats, float durability);
        public abstract void Unequip(IEquipmentOwner playerStats, float durability);

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