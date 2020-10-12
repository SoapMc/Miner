using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using UnityEngine.Tilemaps;
using Miner.Management.Exceptions;
using System;
using Miner.Management;

namespace Miner.Gameplay
{

    public class WorldController : MonoBehaviour
    {
        [SerializeField] private TileTypes _tiles = null;
        [SerializeField] private TileType _tileEdges = null;
        [SerializeField] private Grid _grid = null;
        [SerializeField] private Transform _playerSpawnPoint = null;
        [SerializeField] private TilemapController _tilemapController = null;
        [SerializeField] private Vector2Reference _playerSpawnPointReference = null;

        [Header("Events")]
        [SerializeField] private GameEvent _worldLoaded = null;
        [SerializeField] private GameEvent _worldUnloaded = null;
        [SerializeField] private GameEvent _leadToDigPlace = null;
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _restoreGameAfterPlayerDestroyed = null;
        [SerializeField] private GameEvent _tryAddResourcesToPlayerCargo = null;

        
        private TileIdentifier _tileIdentifier = null;
        private Vector2Int _dugCoords;
        private TileType _dugTile = null;

        public Grid Grid => _grid;

        public void Initialize(TilemapData data)
        {
            _tilemapController.Initialize(data, _tileIdentifier);
        }

        public TilemapData RetrieveSerializableData()
        {
            return _tilemapController.RetrieveSerializableData();
        }

        public void OnDigRequest(EventArgs args)
        {
            if(args is TryDigEA dr)
            {
                _dugCoords = dr.GridCoordinates;
                if(_tilemapController.GetTile(_dugCoords) is Tile tile)
                {
                    _dugTile = _tileIdentifier.Identify(tile.sprite);
                    if (_dugTile != null && dr.DrillSharpness > 0.001f && dr.DrillPower >= _dugTile.Hardiness && _dugTile.IsDestroyable)
                    {
                        _leadToDigPlace.Raise(new AllowDigEA(_dugTile, _dugCoords, dr.DrillSharpness * GameRules.Instance.GetDrillSharpnessCoefficient(_dugCoords.y), 1f, dr.StartPosition, dr.Direction));
                    }
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnDigCompleted()
        {
            if(_dugTile.IsCollectible)
                _tryAddResourcesToPlayerCargo.Raise(new TryChangeResourcesInPlayerCargoEA() { ResourcesToAdd = new List<CargoTable.Element>() { new CargoTable.Element() { Type = _dugTile, Amount = 1 } } });
            Tile tile = _tilemapController.GetTile(_dugCoords);
            if(tile != null)
            {
                if (tile.gameObject != null)
                {
                    if(tile.gameObject.TryGetComponent(out IDigCompletedHandler digCompletedHandler))
                    {
                        digCompletedHandler.OnDigCompleted();
                    }
                }
            }
            DestroyTile(_dugCoords);
            _dugTile = null;
        }

        public void OnDestroyTiles(EventArgs args)
        {
            if(args is DestroyTilesEA dt)
            {
                foreach (var coord in dt.Coordinates)
                {
                    if (_tilemapController.GetTile(coord) is Tile tile)
                    {
                        TileType tt = _tileIdentifier.Identify(tile.sprite);
                        if (tt != null)
                        {
                            if (dt.DestructionPower >= tt.Hardiness)
                                DestroyTile(coord);
                        }
                    }
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        public void OnOverrideTiles(EventArgs args)
        {
            if(args is OverrideTilesEA ot)
            {
                StartCoroutine(DelayedTilesOverriding(ot));
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }

        //IMPORTANT NOTE: Scriptable tile overriding must be done at the end of frame
        private IEnumerator DelayedTilesOverriding(OverrideTilesEA ot)
        {
            yield return new WaitForEndOfFrame();
            foreach (var tileToOverride in ot.Coordinates)
            {
                _tilemapController.SetTile(tileToOverride.Key, tileToOverride.Value.Id);
            }
        }

        public void DestroyTile(Vector2Int gridPos)
        {
            _tilemapController.SetTile(gridPos, -1);
        }

        public void OverrideTile(Vector2Int gridPos, TileType tile)
        {
            _tilemapController.SetTile(gridPos, tile.Id);
        }

        private bool IsNotCollidableTile(Vector2Int gridPos)
        {
            Tile t = _tilemapController.GetTile(gridPos);
            if (t == null) return true;
            if (t.colliderType == Tile.ColliderType.None)
            {
                return true;
            }
            return false;
        }

        private void Awake()
        {
            _tileIdentifier = new TileIdentifier(_tiles);
        }

        private void Start()
        {
            _playerSpawnPointReference.Value = _playerSpawnPoint.position;
            _worldLoaded.Raise(new WorldLoadedEA(_grid, _playerSpawnPoint));
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnDestroy()
        {
            _worldUnloaded.Raise();
        }
    }
}