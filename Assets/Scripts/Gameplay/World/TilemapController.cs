using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace Miner.Gameplay
{
    public class TilemapController : MonoBehaviour
    {
        [SerializeField] private TilemapLoader _tilemapLoader = null;
        [SerializeField] private TileType _emptyTile = null;
        private List<Tuple<int, Tilemap>> _tilemaps = new List<Tuple<int, Tilemap>>();
        private TilemapData _data = null;
        private TileIdentifier _tileIdentifier = null;

        public void Initialize(TilemapData data, TileIdentifier identifier)
        {
            _data = data;
            _tileIdentifier = identifier;
            _tilemapLoader.Initialize(this);
        }

        public TilemapData RetrieveSerializableData()
        {
            return _data;
        }

        public void Load(TilemapData tilemapData)
        {
            _data = tilemapData;
        }

        public void AddTilemap(Tilemap tilemap, int maximumLayerDepth)
        {
            _tilemaps.Add(new Tuple<int, Tilemap>(maximumLayerDepth, tilemap));
        }

        public Tile GetTile(Vector2Int pos)
        {
            return _tilemaps.FirstOrDefault(x => x.Item1 <= pos.y)?.Item2.GetTile<Tile>((Vector3Int)pos);
        }

        public void SetTile(Vector2Int pos, Tile tile)
        {
            _tilemaps.First(x => x.Item1 <= pos.y).Item2.SetTile((Vector3Int)pos, tile);
        }

        public void SetTile(Vector2Int pos, int tileId)
        {
            _data.SetTileId(pos, tileId);
            if (_tilemapLoader.IsTileLoaded(pos))
            {
                Tile tile;
                if (tileId != -1)
                {
                    TileType tt = _tileIdentifier.Identify(tileId);
                    if (tt != null)
                        tile = _tileIdentifier.Identify(tileId).ClasifiedTiles.FirstOrDefault();
                    else
                        tile = null;
                }
                else
                {
                    tile = GetTileEdge(pos);
                    if (_data.GetTileId(pos + Vector2Int.up) == -1)
                        SetEdge(pos + Vector2Int.up);
                    if (_data.GetTileId(pos + Vector2Int.right) == -1)
                        SetEdge(pos + Vector2Int.right);
                    if (_data.GetTileId(pos + Vector2Int.down) == -1)
                        SetEdge(pos + Vector2Int.down);
                    if (_data.GetTileId(pos + Vector2Int.left) == -1)
                        SetEdge(pos + Vector2Int.left);
                }
                _tilemaps.First(x => x.Item1 <= pos.y).Item2.SetTile((Vector3Int)pos, tile);
            }
        }

        private void SetEdge(Vector2Int pos)
        {
            _tilemaps.First(x => x.Item1 <= pos.y).Item2.SetTile((Vector3Int)pos, GetTileEdge(pos));
        }

        public void LoadTile(Vector2Int pos)
        {
            int tileId = _data.GetTileId(pos);

            Tile tile;
            if (tileId != -1)
            {
                TileType tt = _tileIdentifier.Identify(tileId);
                if (tt != null)
                    tile = _tileIdentifier.Identify(tileId).ClasifiedTiles.FirstOrDefault();
                else
                    tile = null;
            }
            else
            {
                tile = GetTileEdge(pos);
            }
            _tilemaps.First(x => x.Item1 <= pos.y).Item2.SetTile((Vector3Int)pos, tile);
        }

        public void UnloadTile(Vector2Int pos)
        {
            _tilemaps.First(x => x.Item1 <= pos.y).Item2.SetTile((Vector3Int)pos, null);
        }

        private Tile GetTileEdge(Vector2Int pos)
        {
            bool up = IsNotCollidableTile(pos + Vector2Int.up);
            bool down = IsNotCollidableTile(pos + Vector2Int.down);
            bool left = IsNotCollidableTile(pos + Vector2Int.left);
            bool right = IsNotCollidableTile(pos + Vector2Int.right);

            if (up && down && left && right)
            {
                return null;
            }
            else if (up && down && right)
            {
                return _emptyTile.ClasifiedTiles[10];
            }
            else if (up && down && left)
            {
                return _emptyTile.ClasifiedTiles[12]; ;
            }
            else if (down && left && right)
            {
                return _emptyTile.ClasifiedTiles[11]; ;
            }
            else if (up && left && right)
            {
                return _emptyTile.ClasifiedTiles[9];
            }
            else if (down && left)
            {
                return _emptyTile.ClasifiedTiles[8];
            }
            else if (down && right)
            {
                return _emptyTile.ClasifiedTiles[7];
            }
            else if (up && right)
            {
                return _emptyTile.ClasifiedTiles[6];
            }
            else if (up && left)
            {
                return _emptyTile.ClasifiedTiles[5];
            }
            else if (up && down)
            {
                return _emptyTile.ClasifiedTiles[14];
            }
            else if (right && left)
            {
                return _emptyTile.ClasifiedTiles[15];
            }
            else if (up)
            {
                return _emptyTile.ClasifiedTiles[4];
            }
            else if (right)
            {
                return _emptyTile.ClasifiedTiles[3];
            }
            else if (down)
            {
                return _emptyTile.ClasifiedTiles[2];
            }
            else if (left)
            {
                return _emptyTile.ClasifiedTiles[1];
            }
            else
            {
                return _emptyTile.ClasifiedTiles[0];
            }
        }

        private bool IsNotCollidableTile(Vector2Int gridPos)
        {
            if (_data.GetTileId(gridPos) == -1)
                return true;
            return false;
        }
    }
}