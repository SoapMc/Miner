using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "World/Tile Types/Default Tile Type")]
    public class TileType : ScriptableObject
    {
        [SerializeField] protected string _name = string.Empty;
        [SerializeField] protected int _id = 0;
        [SerializeField] protected Sprite _icon = null;
        [SerializeField] protected List<CustomTile> _tiles = new List<CustomTile>();
        [SerializeField, Range(1, 100)] protected int _hardiness = 1;
        [SerializeField] protected int _value = 0;
        [SerializeField] protected bool _isFuel = false;
        [SerializeField] protected bool _isCollectible = false;
        [SerializeField] protected bool _isDestroyable = true;
        [SerializeField] protected int _mass = 0;

        public string Name => _name;
        public int Id => _id;
        public Sprite Icon => _icon;
        public List<CustomTile> ClasifiedTiles => _tiles;
        public float Hardiness => _hardiness;
        public int Value => _value;
        public bool IsCollectible => _isCollectible;
        public bool IsDestroyable => _isDestroyable;
        public int Mass => _mass;
        public bool IsFuel => _isFuel;

        public CustomTile this[int index] => ClasifiedTiles[index];

        public virtual void OnResourceAddedToCargo() { }
        public virtual void OnResourceRemovedFromCargo() { }

    }
}