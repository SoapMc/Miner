﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.FX;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Usable Item Effects/Teleporter Effect")]
    public class TeleporterEffect : UsableItemEffect
    {
        [Header("Events")]
        [SerializeField] private GameEvent _movePlayer = null;
        [SerializeField] private GameEvent _disablePlayerController = null;
        [SerializeField] private GameEvent _enablePlayerController = null;
        [SerializeField] private GameEvent _createParticle = null;

        [Header("Resources")]
        [SerializeField] private Vector2Reference _playerSpawnPoint = null;
        [SerializeField] private ParticleSystem _teleportEffect = null;
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private CargoTable _playerCargo = null;

        [Header("Utility")]
        [SerializeField, Range(0f, 1f)] private float _delay = 0.3f;
        [SerializeField] private CoroutineHolder _holder = null;

        public override void Execute()
        {
            _soundOnUse.Play();
            _disablePlayerController.Raise();
            CoroutineHolder handler = Instantiate(_holder);
            handler.StartCoroutine(Teleport(handler.gameObject));
            _enablePlayerController.Raise();

        }

        public override string Description()
        {
            return "The Miner returns to the base in a blink from any place.";
        }

        private IEnumerator Teleport(GameObject obj)
        {
            _createParticle.Raise(new CreateParticleEA(_teleportEffect, _playerPosition.Value));
            _createParticle.Raise(new CreateParticleEA(_teleportEffect, _playerSpawnPoint.Value));
            yield return new WaitForSeconds(_delay);
            _movePlayer.Raise(new MovePlayerEA(_playerSpawnPoint.Value));
            Destroy(obj);
        }
    }
}