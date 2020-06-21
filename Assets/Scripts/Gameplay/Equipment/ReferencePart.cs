using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public abstract class ReferencePart : ScriptableObject
    {
        [SerializeField] protected string _name;
        [SerializeField] protected Sprite _sprite;
        [SerializeField] protected int _id;
        [SerializeField] protected int _cost;
        [SerializeField, TextArea] protected string _shortDescription;
        [System.NonSerialized] protected float _durability = 1f;

        public string Name => _name;
        public Sprite Sprite => _sprite;
        public int Id => _id;
        public int Cost => _cost;
        public string ShortDescription => _shortDescription;
        public float Durability
        {
            get => _durability;
            set => _durability = Mathf.Clamp(value, 0f, 1f);
        }

        public virtual string[] GetSpecificDescription() { return new string[0]; }
        public virtual string[] GetPerformanceDescription() { return new string[0]; }
    }
}