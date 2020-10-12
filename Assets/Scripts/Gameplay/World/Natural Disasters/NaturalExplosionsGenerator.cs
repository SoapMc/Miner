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

        public void OnDigCompleted()
        {
            if(Random.Range(0f, 1f) < _chanceForExplosion)
            {
                Instantiate(_explosionPrefab, _playerPosition.Value, Quaternion.identity);
            }
        }
    }
}