using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using UnityEngine.Tilemaps;
using Miner.Management.Exceptions;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using Miner.Management;

namespace Miner.Gameplay
{
    public class WorldController : MonoBehaviour
    {
        [SerializeField] private TileTypes _tiles = null;
        [SerializeField] private TileType _tileEdges = null;
        [SerializeField] private Tilemap _tilemap = null;
        [SerializeField] private Transform _playerSpawnPoint = null;
        [SerializeField] private Vector2IntReference _playerGridPosition = null;
        [SerializeField] private FloatReference _playerExternalTemperature = null;
        [SerializeField] private FloatReference _surfaceTemperature = null;
        [SerializeField, Range(0.02f, 1f)] private float _temperatureGradient = 0.25f;  //per tile

        [Header("Events")]
        [SerializeField] private GameEvent _worldLoaded = null;
        [SerializeField] private GameEvent _leadToDigPlace = null;
        [SerializeField] private GameEvent _updatePlayerData = null;

        [Header("World Generation")]
        [SerializeField] private Vector2IntReference _horizontalWorldBorders = null;
        [SerializeField] private Vector2IntReference _vecticalWorldBorders = null;
        [SerializeField] private List<GroundLayer> _layers = new List<GroundLayer>();
        private int _minimumLayerDepth = -2;

        private TileIdentifier _tileIdentifier = null;
        private Vector2Int _dugCoords;
        private TileType _dugTile = null;

        public void OnDigRequest(EventArgs args)
        {
            if(args is DigRequestEA dr)
            {
                _dugCoords = dr.Coordinates;
                if(_tilemap.GetTile<Tile>((Vector3Int)_dugCoords) is Tile tile)
                {
                    _dugTile = _tileIdentifier.Identify(tile.sprite);
                    if(_dugTile != null && dr.DrillSharpness > 0.0001f)
                        _leadToDigPlace.Raise(new LeadToDigPlaceEA(_dugTile, _dugCoords, dr.DrillSharpness * GameRules.Instance.GetDrillSharpnessCoefficient(_dugCoords.y), 1f));
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnDigComplete()
        {
            UpdatePlayerDataEA upd = new UpdatePlayerDataEA();
            upd.AddCargoChange.Add(new CargoTable.Element() { Type = _dugTile, Amount = 1 });
            _updatePlayerData.Raise(upd);
            DestroyTile(_dugCoords);
            _dugTile = null;
        }

        public void DestroyTile(Vector2Int gridPos)
        {
            SetTileEdges(gridPos);
            
            if (IsNotCollidableTile(gridPos + Vector2Int.up))
                SetTileEdges(gridPos + Vector2Int.up);
            if (IsNotCollidableTile(gridPos + Vector2Int.down))
                SetTileEdges(gridPos + Vector2Int.down);
            if (IsNotCollidableTile(gridPos + Vector2Int.left))
                SetTileEdges(gridPos + Vector2Int.left);
            if (IsNotCollidableTile(gridPos + Vector2Int.right))
                SetTileEdges(gridPos + Vector2Int.right);
        }

        public bool IsNotCollidableTile(Vector2Int gridPos)
        {
            Tile t = _tilemap.GetTile<Tile>((Vector3Int)gridPos);
            if (t == null) return true;
            if (t.colliderType == Tile.ColliderType.None)
            {
                return true;
            }
            return false;
        }

        public void SetTileEdges(Vector2Int gridPos)
        {
            try
            {
                Vector3Int coords = (Vector3Int)gridPos;
                bool up = IsNotCollidableTile(gridPos + Vector2Int.up);
                bool down = IsNotCollidableTile(gridPos + Vector2Int.down);
                bool left = IsNotCollidableTile(gridPos + Vector2Int.left);
                bool right = IsNotCollidableTile(gridPos + Vector2Int.right);
                //Debug.Log("Pos: " + gridPos + " u: " + up + " d:" + down + " l:" + left + " r:" + right);
                if (up && down && left && right)
                {
                    _tilemap.SetTile(coords, null);
                    return;
                }
                else if (up && down && right)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[10]);
                    return;
                }
                else if (up && down && left)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[12]);
                    return;
                }
                else if (down && left && right)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[11]);
                    return;
                }
                else if (up && left && right)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[9]);
                    return;
                }
                else if (down && left)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[8]);
                    return;
                }
                else if (down && right)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[7]);
                    return;
                }
                else if (up && right)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[6]);
                    return;
                }
                else if (up && left)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[5]);
                    return;
                }
                else if(up && down)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[14]);
                    return;
                }
                else if (right && left)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[15]);
                    return;
                }
                else if (up)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[4]);
                    return;
                }
                else if (right)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[3]);
                    return;
                }
                else if (down)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[2]);
                    return;
                }
                else if (left)
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[1]);
                    return;
                }
                else
                {
                    _tilemap.SetTile(coords, _tileEdges.ClasifiedTiles[0]);
                    return;
                }
            }
            catch
            {
                Debug.Log(gridPos);
            }
        }


        public void GenerateWorld(int seed = -1)
        {
            float prob;
            bool tileSet;
            int minimumLayerDepthForCurrentLayer = _minimumLayerDepth;

            int totalDepth = -_layers.Sum(x => x.Depth) + minimumLayerDepthForCurrentLayer;
            for (int x = _horizontalWorldBorders.Value.x; x < _horizontalWorldBorders.Value.y; ++x)
            {
                _tilemap.SetTile(new Vector3Int(x, 20, 0), _tileEdges.ClasifiedTiles[13]);
                _tilemap.SetTile(new Vector3Int(x, totalDepth, 0), _tileEdges.ClasifiedTiles[13]);
            }
            
            for (int y = 20; y > totalDepth - 1; --y)
            {
                _tilemap.SetTile(new Vector3Int(_horizontalWorldBorders.Value.x - 1, y, 0), _tileEdges.ClasifiedTiles[13]);
                _tilemap.SetTile(new Vector3Int(_horizontalWorldBorders.Value.y, y, 0), _tileEdges.ClasifiedTiles[13]);
            }

            foreach (var layer in _layers)
            {

                for(int x = _horizontalWorldBorders.Value.x; x < _horizontalWorldBorders.Value.y; ++x)
                {
                    for (int y = minimumLayerDepthForCurrentLayer; y > (minimumLayerDepthForCurrentLayer - layer.Depth); --y)
                    {
                        tileSet = false;
                        prob = Random.Range(0f, 1f);
                        for (int j = 0; j < layer.Resources.Count; ++j)
                        {
                            if (prob <= layer.Resources[j].Probability)
                            {
                                _tilemap.SetTile(new Vector3Int(x, y, 0), layer.Resources[j].Type.ClasifiedTiles[0]);
                                tileSet = true;
                            }
                        }
                        if (!tileSet)
                        {
                            _tilemap.SetTile(new Vector3Int(x, y, 0), layer.DefaultTiles[0].ClasifiedTiles[0]);
                        }

                    }
                }

                for (int y = minimumLayerDepthForCurrentLayer; y > (minimumLayerDepthForCurrentLayer - layer.Depth); --y)
                {
                    for (int x = _horizontalWorldBorders.Value.x; x < _horizontalWorldBorders.Value.y; ++x)
                    {
                    
                        prob = Random.Range(0f, 1f);
                        if(prob <= layer.ProbabilityOfEmptySpaces)
                        {
                           DestroyTile(new Vector2Int(x, y));
                        }
                    }
                }
                minimumLayerDepthForCurrentLayer -= layer.Depth;
            }
        }

        private IEnumerator UpdateExternalTemperature()
        {
            while(true)
            {
                _playerExternalTemperature.Value = (_surfaceTemperature.Value - _playerGridPosition.Value.y * _temperatureGradient) + Random.Range(-1f, 1f);
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void Awake()
        {
            _tileIdentifier = new TileIdentifier(_tiles);
            GenerateWorld();
        }

        private void Start()
        {
            int totalDepth = -_layers.Sum(x => x.Depth) + _minimumLayerDepth;
            _vecticalWorldBorders.Value = new Vector2Int(_vecticalWorldBorders.Value.x, totalDepth);

            _worldLoaded.Raise(new WorldLoadedEA(_tilemap, _playerSpawnPoint));
            StartCoroutine(UpdateExternalTemperature());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

    }
}