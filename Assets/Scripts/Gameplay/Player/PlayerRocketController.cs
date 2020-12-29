using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Gameplay;

namespace Miner.FX
{
    public class PlayerRocketController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem = null;
        [SerializeField] private AudioSource _rocketAudioSource = null;
        [SerializeField] private SoundEffect _rocketSound = null;
        private bool _locked = false;
        private bool _enabled = false;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                if(_enabled != value)
                {
                    if (value == true)
                        TurnOn();
                    else
                        TurnOff();
                }
                _enabled = value;

            }
        }

        public bool Locked
        {
            get => _locked;
            set
            {
                _locked = value;
                if (_locked == true)
                    TurnOff();
            }
        }

        private void TurnOn()
        {
            if (!Locked)
            {
                _particleSystem.Play();
                _rocketSound.Play(_rocketAudioSource);
                _enabled = true;
            }
        }

        private void TurnOff()
        {
            _particleSystem.Stop();
            _rocketAudioSource.Stop();
            _enabled = false;
        }

        private void Awake()
        {
            _rocketAudioSource.clip = _rocketSound;
            TurnOff();
        }
    }
}
