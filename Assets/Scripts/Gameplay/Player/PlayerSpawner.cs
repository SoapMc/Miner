using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameEvent _translatePlayer = null;
        [SerializeField] private Vector2Reference _spawnPoint = null;

        public void OnRespawnPlayer()
        {
            _translatePlayer.Raise(new TranslatePlayerEA(_spawnPoint.Value));
        }
    }
}