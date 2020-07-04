using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using Random = UnityEngine.Random;

namespace Miner.FX
{
    public class CameraShaker : MonoBehaviour
    {
        private Coroutine _shake = null;
        private float _currentAmplitude = 0f;

        private IEnumerator ApplyCameraShake(float amplitude, float vibrationRate, float damping)
        {
            Vector3 vibrationAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            vibrationAxis = Vector3.Normalize(vibrationAxis);
            float dt = 0f;
            while (amplitude > 0.00001f)
            {
                amplitude *= (1 - damping);
                _currentAmplitude = amplitude;
                dt += Time.deltaTime;
                transform.position = vibrationAxis * amplitude * Mathf.Sin(vibrationRate * dt);
                yield return null;
            }
            _currentAmplitude = 0f;
            _shake = null;
        }

        public void OnCameraShake(EventArgs args)
        {
            if (args is CameraShakeEA cs)
            {
                if (cs.Amplitude > _currentAmplitude)
                {
                    if(_shake != null)
                        StopCoroutine(_shake);
                    _shake = StartCoroutine(ApplyCameraShake(cs.Amplitude, cs.VibrationRate, cs.Damping));
                }
            }
            else
            {
                throw new InvalidEventArgsException();
            }
        }
    }
}