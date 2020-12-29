using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System;
using Random = UnityEngine.Random;

namespace Miner.Gameplay
{
    public class NaturalExplosionsGenerator : MonoBehaviour
    {
        [SerializeField, Range(0f, 0.2f)] private float _chanceForExplosion = 0.01f;
        [SerializeField] private ParticleSystem _explosionPrefab = null;
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private GameEvent _damagePlayer = null;
        [SerializeField] private GameEvent _destroyTiles = null;
        [SerializeField] private Vector2IntReference _playerGridPosition = null;
        [SerializeField] private DamageType _damageType = null;

        public void OnDigCompleted()
        {
            if(Random.Range(0f, 1f) < _chanceForExplosion)
            {
                Instantiate(_explosionPrefab, _playerPosition.Value, Quaternion.identity);
                _damagePlayer.Raise(new DamagePlayerEA((int)(Mathf.Abs(_playerGridPosition.Value.y) / 5f), _damageType));

                List<Vector2Int> destroyedTiles = new List<Vector2Int>(9);
                for (int y = -1; y < 2; ++y)
                {
                    for (int x = -1; x < 2; ++x)
                    {
                        destroyedTiles.Add(new Vector2Int(_playerGridPosition.Value.x + x, _playerGridPosition.Value.y + y));
                    }
                }
                _destroyTiles.Raise(new DestroyTilesEA(destroyedTiles, 70));
            }
        }
    }
}