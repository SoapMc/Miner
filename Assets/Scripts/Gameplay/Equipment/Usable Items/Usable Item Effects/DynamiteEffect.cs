using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Usable Item Effects/Dynamite Effect")]
    public class DynamiteEffect : UsableItemEffect
    {
        [Header("Events")]
        [SerializeField] private GameEvent _cameraShake = null;
        [SerializeField] private GameEvent _destroyTiles = null;
        [SerializeField] private GameEvent _createParticle = null;

        [Header("Resources")]
        [SerializeField] private Vector2IntReference _playerGridPosition = null;
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private ParticleSystem _explosionEffect = null;
        [SerializeField] private ParticleSystem _groundExplosionParticles = null;

        [Header("Camera Shake")]
        [SerializeField, Range(0f, 1f)] private float _cameraShakeAmplitude = 0.5f;

        public override void Execute()
        {
            _cameraShake.Raise(new CameraShakeEA(_cameraShakeAmplitude));
            _createParticle.Raise(new CreateParticleEA(_explosionEffect, _playerPosition));

            List<Vector2Int> destroyedTiles = new List<Vector2Int>(9);
            for(int y = -1; y < 2; ++y)
            {
                for(int x = -1; x < 2; ++x)
                {
                    destroyedTiles.Add(new Vector2Int(_playerGridPosition.Value.x + x, _playerGridPosition.Value.y + y));
                    _createParticle.Raise(new CreateParticleEA(_groundExplosionParticles, new Vector2(_playerGridPosition.Value.x + x, _playerGridPosition.Value.y + y), CreateParticleEA.ECoordinateType.Grid));
                }
            }

            _destroyTiles.Raise(new DestroyTilesEA(destroyedTiles));
        }
    }
}