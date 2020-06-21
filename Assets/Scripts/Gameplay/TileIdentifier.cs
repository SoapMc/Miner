using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public class TileIdentifier
    {
        private TileTypes _tiles = null;

        public TileIdentifier(TileTypes tiles)
        {
            _tiles = tiles;
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
    }
}