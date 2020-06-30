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
        [SerializeField] private GroundLayerController _surfaceTilemap = null;
        [SerializeField] private GroundLayerController _groundLayerPrefab = null;
        [SerializeField] private Grid _grid = null;
        [SerializeField] private Transform _playerSpawnPoint = null;
        [SerializeField] private Vector2IntReference _playerGridPosition = null;
        [SerializeField] private FloatReference _playerExternalTemperature = null;
        [SerializeField] private FloatReference _surfaceTemperature = null;
        [SerializeField, Range(0.02f, 1f)] private float _temperatureGradient = 0.25f;  //per tile
        [SerializeField] private TilemapController _tilemapController = null;
        [SerializeField] private UndergroundTrigger _undergroundTrigger = null;

        [Header("Events")]
        [SerializeField] private GameEvent _worldLoaded = null;
        [SerializeField] private GameEvent _leadToDigPlace = null;
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _updateInfrastructureData = null;
        [SerializeField] private GameEvent _restoreGameAfterPlayerDestroyed = null;

        [Header("World Generation")]
        [SerializeField] private Vector2IntReference _horizontalWorldBorders = null;
        [SerializeField] private Vector2IntReference _vecticalWorldBorders = null;
        [SerializeField] private IntReference _undergroundTriggerDepth;
        [SerializeField] private List<GroundLayer> _layers = new List<GroundLayer>();
        private int _surfaceDepth = -2;

        private TileIdentifier _tileIdentifier = null;
        private Vector2Int _dugCoords;
        private TileType _dugTile = null;

        public void OnDigRequest(EventArgs args)
        {
            if(args is DigRequestEA dr)
            {
                _dugCoords = dr.Coordinates;
                if(_tilemapController.GetTile(_dugCoords) is Tile tile)
                {
                    _dugTile = _tileIdentifier.Identify(tile.sprite);
                    if (_dugTile != null && dr.DrillSharpness > 0.001f)
                    {
                        _leadToDigPlace.Raise(new LeadToDigPlaceEA(_dugTile, _dugCoords, dr.DrillSharpness * GameRules.Instance.GetDrillSharpnessCoefficient(_dugCoords.y), 1f));
                    }
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

        public void OnPlayerDead()
        {
            StartCoroutine(PlayerDead(5f));
        }

        private IEnumerator PlayerDead(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _restoreGameAfterPlayerDestroyed.Raise(new RestoreGameAfterPlayerDestroyedEA(_playerSpawnPoint));
            _tilemapController.ActivateSurface();
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
            Tile t = _tilemapController.GetTile(gridPos);
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
                bool up = IsNotCollidableTile(gridPos + Vector2Int.up);
                bool down = IsNotCollidableTile(gridPos + Vector2Int.down);
                bool left = IsNotCollidableTile(gridPos + Vector2Int.left);
                bool right = IsNotCollidableTile(gridPos + Vector2Int.right);

                if (up && down && left && right)
                {
                    _tilemapController.SetTile(gridPos, null);
                    return;
                }
                else if (up && down && right)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[10]);
                    return;
                }
                else if (up && down && left)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[12]);
                    return;
                }
                else if (down && left && right)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[11]);
                    return;
                }
                else if (up && left && right)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[9]);
                    return;
                }
                else if (down && left)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[8]);
                    return;
                }
                else if (down && right)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[7]);
                    return;
                }
                else if (up && right)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[6]);
                    return;
                }
                else if (up && left)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[5]);
                    return;
                }
                else if(up && down)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[14]);
                    return;
                }
                else if (right && left)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[15]);
                    return;
                }
                else if (up)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[4]);
                    return;
                }
                else if (right)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[3]);
                    return;
                }
                else if (down)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[2]);
                    return;
                }
                else if (left)
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[1]);
                    return;
                }
                else
                {
                    _tilemapController.SetTile(gridPos, _tileEdges.ClasifiedTiles[0]);
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
            if (seed != -1)
                Random.InitState(seed);

            float prob;
            bool tileSet;
            int minimumDepthForCurrentLayer = 0;
            int maximumDepthForCurrentLayer = _surfaceDepth;
            int worldWidth = Mathf.Abs(_horizontalWorldBorders.Value.x - _horizontalWorldBorders.Value.y);
            _undergroundTrigger.Initialize(_undergroundTriggerDepth, worldWidth);
            int totalDepth = -_layers.Sum(x => x.Depth) + maximumDepthForCurrentLayer;
            Camera mainCamera = Camera.main;

            Tilemap surface = _surfaceTilemap.Tilemap;
            for (int x = _horizontalWorldBorders.Value.x; x < _horizontalWorldBorders.Value.y; ++x)
            {
                _surfaceTilemap.Tilemap.SetTile(new Vector3Int(x, 20, 0), _tileEdges.ClasifiedTiles[13]);
                _surfaceTilemap.Tilemap.SetTile(new Vector3Int(x, totalDepth, 0), _tileEdges.ClasifiedTiles[13]);
            }

            for (int y = 20; y > totalDepth - 1; --y)
            {
                _surfaceTilemap.Tilemap.SetTile(new Vector3Int(_horizontalWorldBorders.Value.x - 1, y, 0), _tileEdges.ClasifiedTiles[13]);
                _surfaceTilemap.Tilemap.SetTile(new Vector3Int(_horizontalWorldBorders.Value.y, y, 0), _tileEdges.ClasifiedTiles[13]);
            }
            _tilemapController.AddTilemap(_surfaceTilemap, _surfaceDepth);

            for (int i = 0; i < _layers.Count; ++i)
            {
                minimumDepthForCurrentLayer = maximumDepthForCurrentLayer;  //minimum depth is maximum depth from previous layer 
                maximumDepthForCurrentLayer -= _layers[i].Depth;
                GroundLayerController groundLayer = Instantiate(_groundLayerPrefab, _grid.transform);
                groundLayer.Initialize(minimumDepthForCurrentLayer, maximumDepthForCurrentLayer, Mathf.Abs(_horizontalWorldBorders.Value.x - _horizontalWorldBorders.Value.y), i + 1, mainCamera.orthographicSize);
                _tilemapController.AddTilemap(groundLayer, maximumDepthForCurrentLayer);

                for (int x = _horizontalWorldBorders.Value.x; x < _horizontalWorldBorders.Value.y; ++x)
                {
                    for (int y = minimumDepthForCurrentLayer; y > maximumDepthForCurrentLayer; --y)
                    {
                        tileSet = false;
                        prob = Random.Range(0f, 1f);
                        for (int j = 0; j < _layers[i].Resources.Count; ++j)
                        {
                            if (prob <= _layers[i].Resources[j].Probability)
                            {
                                groundLayer.Tilemap.SetTile(new Vector3Int(x, y, 0), _layers[i].Resources[j].Type.ClasifiedTiles[0]);
                                tileSet = true;
                            }
                        }
                        if (!tileSet)
                        {
                            groundLayer.Tilemap.SetTile(new Vector3Int(x, y, 0), _layers[i].DefaultTiles[0].ClasifiedTiles[0]);
                        }

                    }
                }
                
                for (int y = minimumDepthForCurrentLayer; y > (maximumDepthForCurrentLayer + 1); --y)
                {
                    for (int x = _horizontalWorldBorders.Value.x; x < _horizontalWorldBorders.Value.y; ++x)
                    {

                        prob = Random.Range(0f, 1f);
                        if (prob <= _layers[i].ProbabilityOfEmptySpaces)
                        {
                            DestroyTile(new Vector2Int(x, y));
                        }
                    }
                }
                    
            }

            _tilemapController.ActivateSurface();
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
            _surfaceTilemap.Initialize(0, _surfaceDepth, Mathf.Abs(_horizontalWorldBorders.Value.x - _horizontalWorldBorders.Value.y), 0);
        }

        private void Start()
        {
            int totalDepth = -_layers.Sum(x => x.Depth) + _surfaceDepth;
            _vecticalWorldBorders.Value = new Vector2Int(_vecticalWorldBorders.Value.x, totalDepth);
            int worldWidth = Mathf.Abs(_horizontalWorldBorders.Value.x - _horizontalWorldBorders.Value.y);
            _undergroundTrigger.Initialize(_undergroundTriggerDepth, worldWidth);

            _worldLoaded.Raise(new WorldLoadedEA(_grid, _playerSpawnPoint));
            StartCoroutine(UpdateExternalTemperature());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

    }
}