using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.FX
{
    public class ExplosionEffect : MonoBehaviour
    {
        [SerializeField] private GameEvent _shakeCamera = null;
        [SerializeField, Range(0f, 2f)] private float _shakeCameraAmplitude = 0.8f;
        [SerializeField] private SoundEffect _soundEffect = null;

        private void Start()
        {
            _shakeCamera.Raise(new CameraShakeEA(_shakeCameraAmplitude));
            if (_soundEffect != null)
                _soundEffect.Play();
        }
    }
}