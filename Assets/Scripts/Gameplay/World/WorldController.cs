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
        [SerializeField] private Grid _grid = null;
        [SerializeField] private Transform _playerSpawnPoint = null;
        [SerializeField] private TilemapController _tilemapController = null;
        [SerializeField] private Vector2Reference _playerSpawnPointReference = null;

        [Header("Events")]
        [SerializeField] private GameEvent _worldLoaded = null;
        [SerializeField] private GameEvent _leadToDigPlace = null;
        [SerializeField] private GameEvent _updatePlayerData = null;
        [SerializeField] private GameEvent _restoreGameAfterPlayerDestroyed = null;
        [SerializeField] private GameEvent _tryAddResourcesToPlayerCargo = null;

        private TileIdentifier _tileIdentifier = null;
        private Vector2Int _dugCoords;
        private TileType _dugTile = null;

        public Grid Grid => _grid;

        public void OnDigRequest(EventArgs args)
        {
            if(args is TryDigEA dr)
            {
                _dugCoords = dr.GridCoordinates;
                if(_tilemapController.GetTile(_dugCoords) is Tile tile)
                {
                    _dugTile = _tileIdentifier.Identify(tile.sprite);
                    if (_dugTile != null && dr.DrillSharpness > 0.001f && _dugTile.IsDestroyable)
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
            _tryAddResourcesToPlayerCargo.Raise(new TryChangeResourcesInPlayerCargoEA() { ResourcesToAdd = new List<CargoTable.Element>() { new CargoTable.Element() { Type = _dugTile, Amount = 1 } } });
            DestroyTile(_dugCoords);
            _dugTile = null;
        }

        public void OnPlayerDead()
        {
            StartCoroutine(PlayerDead(5f));
        }

        public void OnDestroyTiles(EventArgs args)
        {
            if(args is DestroyTilesEA dt)
            {
                if (dt.Source == DestroyTilesEA.ESource.Explosion)
                {
                    foreach (var coord in dt.Coordinates)
                    {
                        if (_tilemapController.GetTile(coord) is Tile tile)
                        {
                            TileType tt = _tileIdentifier.Identify(tile.sprite);
                            if (tt != null)
                            {
                                if (!tt.ExplosionProofness)
                                    DestroyTile(coord);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var coord in dt.Coordinates)
                    {
                        DestroyTile(coord);
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
                _tilemapController.SetTile(tileToOverride.Key, tileToOverride.Value.ClasifiedTiles[0]);
            }
        }

        private IEnumerator PlayerDead(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _restoreGameAfterPlayerDestroyed.Raise(new RestoreGameAfterPlayerDestroyedEA(_playerSpawnPoint));
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

        public void OverrideTile(Vector2Int gridPos, TileType tile)
        {
            _tilemapController.SetTile(gridPos, tile.ClasifiedTiles[0]);
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
            if (_tilemapController.GetTile(gridPos) == null) return;

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

    }
}