using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Tile Type")]
    public class TileType : ScriptableObject
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField] private int _id = 0;
        [SerializeField] private List<Tile> _tiles = new List<Tile>();
        [SerializeField, Range(0.01f, 10f)] private float _hardiness = 1f;
        [SerializeField] private int _value = 0;
        [SerializeField] private bool _isFuel = false;
        [SerializeField] private bool _isDestroyable = true;
        [SerializeField] private int _mass = 0;
        [SerializeField] private bool _showBriefInfoOnDig = true;

        public string Name => _name;
        public int Id => _id;
        public List<Tile> ClasifiedTiles => _tiles;
        public float Hardiness => _hardiness;
        public int Value => _value;
        public bool IsDestroyable => _isDestroyable;
        public int Mass => _mass;
        public bool IsFuel => _isFuel;
        public bool ShowBriefInfoOnDig => _showBriefInfoOnDig;

    }
}