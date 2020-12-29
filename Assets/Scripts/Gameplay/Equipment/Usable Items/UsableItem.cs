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
        [SerializeField] private float _usedPower = 0;
        [SerializeField, TextArea] private string _shortDescription = string.Empty;
        [SerializeField] private List<UsableItemEffect> _effects = new List<UsableItemEffect>();
        
        public string Name => _name;
        public Sprite Sprite => _sprite;
        public int Id => _id;
        public int Cost => _cost;
        public float UsedPower => _usedPower;
        public string ShortDescription
        {
            get
            {
                string result = _shortDescription + " \n";
                result += "Power cost: " + _usedPower.ToString("0.0");
                return result;
            }
        }
        public List<UsableItemEffect> Effects => _effects;

        public void Execute()
        {
            foreach(var eff in Effects)
            {
                eff.Execute();
            }
        }
    }
}