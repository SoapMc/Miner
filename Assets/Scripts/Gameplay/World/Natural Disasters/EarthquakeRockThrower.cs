﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    public class EarthquakeRockThrower : MonoBehaviour
    {
        [SerializeField] private FallingRockParticle _fallingRockBehaviour = null;
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private GameEvent _cameraShake = null;

        private int _intensity = 1;
        private float _timeSinceThrow = 0f;
        private Camera _mainCamera = null;
        private float _cameraWidth;
        private Coroutine _cameraShakeCoroutine = null;


        public void Initialize(int intensity)
        {
            _intensity = intensity;
            if (_intensity <= 0)
                throw new ArgumentException();
        }

        private IEnumerator ShakeCamera()
        {
            while(true)
            {
                _cameraShake.Raise(new CameraShakeEA(_intensity / 100f, 90, 0.02f));
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            _cameraWidth = _mainCamera.orthographicSize * _mainCamera.aspect;
            _cameraShakeCoroutine = StartCoroutine(ShakeCamera());
        }

        private void Update()
        {
            _timeSinceThrow += Time.deltaTime;
            if (_timeSinceThrow >= 4f / _intensity)
            {
                FallingRockParticle rock = Instantiate(_fallingRockBehaviour,
                    new Vector3(Random.Range(_playerPosition.Value.x - _cameraWidth, _playerPosition.Value.x + _cameraWidth), _playerPosition.Value.y + _mainCamera.orthographicSize + 2f, 0f),
                    Quaternion.identity);
                rock.Initialize((int)(_playerPosition.Value.y / 100f));
                _timeSinceThrow = 0f;
            }
        }

        private void OnDestroy()
        {
            if (_cameraShakeCoroutine != null)
                StopCoroutine(_cameraShakeCoroutine);
        }
    }
}
