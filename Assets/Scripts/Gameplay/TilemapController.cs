using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System.Linq;

namespace Miner.Gameplay
{
    public class TilemapController
    {
        private Tilemap _alwaysActiveTilemap = null;
        private List<Tuple<int, Tilemap>> _tilemaps = new List<Tuple<int, Tilemap>>();
        private int _layersCount = 0;

        public Tilemap AlwaysActiveTilemap => _alwaysActiveTilemap;

        public void AddTilemap(Tilemap tilemap, int maximumLayerDepth)
        {
#warning zmien tilemap na ground layer controller
            _tilemaps.Add(new Tuple<int, Tilemap>(maximumLayerDepth, tilemap));
        }

        public Tile GetTile(Vector2Int pos)
        {
            return _tilemaps.FirstOrDefault(x => x.Item1 <= pos.y).Item2.GetTile<Tile>((Vector3Int)pos);
        }

        public void SetTile(Vector2Int pos, Tile tile)
        {
            _tilemaps.First(x => x.Item1 < pos.y).Item2.SetTile((Vector3Int)pos, tile);
        }

        public void SetTileToDefaultTilemap(Vector2Int pos, Tile tile)
        {
            _alwaysActiveTilemap.SetTile((Vector3Int)pos, tile);
        }

        public void OnTriggerUpperLayer(EventArgs args)
        {
            if(args is LayerTriggerEA lt)
            {
                if (lt.LayerNumber > 1 && lt.LayerNumber < _layersCount)
                    _tilemaps[lt.LayerNumber - 1].Item2.gameObject.SetActive(lt.LayerActivation);
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnTriggerBottomLayer(EventArgs args)
        {
            if (args is LayerTriggerEA lt)
            {
                if (lt.LayerNumber > 0 && lt.LayerNumber < (_layersCount - 1) )
                    _tilemaps[lt.LayerNumber + 1].Item2.gameObject.SetActive(lt.LayerActivation);
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public TilemapController(int layersCount, Tilemap alwaysActiveTilemap)
        {
            _alwaysActiveTilemap = alwaysActiveTilemap;
            _layersCount = layersCount;
        }
    }
}