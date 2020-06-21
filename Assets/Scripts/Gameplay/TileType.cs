﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Tile Type")]
    public class TileType : ScriptableObject
    {
        [SerializeField] private string _name = string.Empty;
        [SerializeField] private List<Tile> _tiles;
        [SerializeField, Range(0.01f, 10f)] private float _hardiness = 1f;
        [SerializeField] private int _value = 0;
        [SerializeField] private bool _isFuel = false;

        public string Name => _name;
        public List<Tile> ClasifiedTiles => _tiles;
        public float Hardiness => _hardiness;
        public int Value => _value;
    }
}