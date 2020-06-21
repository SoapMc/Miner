using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Usable Item")]
    public class UsableItem : ScriptableObject
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField] private Sprite _sprite = null;
        [SerializeField] private int _id = 0;
        [SerializeField] private int _cost = 0;
        [SerializeField] private string _shortDescription = string.Empty;
        [SerializeField] private List<UsableItemEffect> _effects = new List<UsableItemEffect>();
        
        public string Name => _name;
        public Sprite Sprite => _sprite;
        public int Id => _id;
        public int Cost => _cost;
        public string ShortDescription => _shortDescription;
        public List<UsableItemEffect> Effects => _effects;

    }
}