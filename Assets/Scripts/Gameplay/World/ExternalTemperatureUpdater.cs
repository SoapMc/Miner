using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public class ExternalTemperatureUpdater : MonoBehaviour
    {
        [SerializeField] private Vector2IntReference _playerGridPosition = null;
        [SerializeField] private FloatReference _surfaceTemperature = null;
        [SerializeField] private FloatReference _playerExternalTemperature = null;
        [SerializeField, Range(0.02f, 1f)] private float _temperatureGradient = 0.25f;  //per tile

        private IEnumerator UpdateExternalTemperature()
        {
            while (true)
            {
                _playerExternalTemperature.Value = (_surfaceTemperature.Value - _playerGridPosition.Value.y * _temperatureGradient) + Random.Range(-1f, 1f);
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void Start()
        {
            StartCoroutine(UpdateExternalTemperature());
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}