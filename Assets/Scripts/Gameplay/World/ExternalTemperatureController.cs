using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Random = UnityEngine.Random;

namespace Miner.Gameplay
{
    public class ExternalTemperatureController : MonoBehaviour
    {
        [SerializeField] private Vector2IntReference _playerGridPosition = null;
        [SerializeField] private Vector2Reference _playerPosition = null;
        [SerializeField] private FloatReference _surfaceTemperature = null;
        [SerializeField] private FloatReference _playerExternalTemperature = null;
        [SerializeField, Range(0.02f, 1f)] private float _temperatureGradient = 0.25f;  //per tile
        [SerializeField] private LayerMask _groundLayer = default;
        private float _offset = 0f;
        private Collider2D[] _temperatureSources = new Collider2D[16];
        private static float _temperatureSourcesRange = 10f;

        private float CalculateTemperatureOffset()
        {
            int temperatureSourcesAmount = Physics2D.OverlapCircleNonAlloc(_playerPosition.Value, _temperatureSourcesRange, _temperatureSources, _groundLayer);
            float maxOffset = 0f;
            for(int i = 0; i < temperatureSourcesAmount; ++i)
            {
                if(_temperatureSources[i].TryGetComponent(out ITemperatureOffsetSource temperatureOffsetSource))
                {
                    maxOffset = Mathf.Max(temperatureOffsetSource.GetTemperatureOffset(((Vector2)_temperatureSources[i].transform.position - _playerPosition.Value).sqrMagnitude), maxOffset);
                }
            }
            return maxOffset;
        }

        private void Update()
        {
            _playerExternalTemperature.Value = (_surfaceTemperature.Value - _playerGridPosition.Value.y * _temperatureGradient) + CalculateTemperatureOffset();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}