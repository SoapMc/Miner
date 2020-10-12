using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public class TileIdentifier
    {
        private TileTypes _tiles = null;
        private Dictionary<int, TileType> _tilesById;

        public TileIdentifier(TileTypes tiles)
        {
            _tiles = tiles;
            _tilesById = new Dictionary<int, TileType>(tiles.Count);
            foreach(var tile in _tiles)
            {
                if (!_tilesById.ContainsKey(tile.Id))
                    _tilesById[tile.Id] = tile;
            }
        }

        public TileType Identify(Sprite sprite)
        {
            foreach(var tileType in _tiles)
            {
                foreach (var tile in tileType.ClasifiedTiles)
                {
                    if (sprite == tile.sprite)
                        return tileType;
                }
            }
            return null;
        }

        public TileType Identify(int id)
        {
            try
            {
                return _tilesById[id];
            }
            catch(KeyNotFoundException)
            {
                Management.GameManager.Instance.Log.Write(GetType().Name + " : Requested tile id is not present in collection [tileid:" + id.ToString() + "]");
                return _tilesById[0];
            }
        }
    }
}