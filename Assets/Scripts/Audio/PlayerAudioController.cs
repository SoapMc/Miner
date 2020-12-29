using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management;
using Miner.Gameplay;

namespace Miner.FX
{
    public class PlayerAudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _engine = null;
        [SerializeField] private Controls _input = null;
        [SerializeField] private AudioClip _engineWorking = null;
        [SerializeField] private AudioClip _engineIdle = null;
        private bool _engineWorkingEnabled = false;


        private void Update()
        {
            if(Mathf.Abs(_input.Player.Movement.ReadValue<Vector2>().x) > 0.1f)
            {
                if (!_engineWorkingEnabled)
                {
                    _engine.clip = _engineWorking;
                    _engineWorkingEnabled = true;
                    _engine.Play();
                }
            }
            else
            {
                if(_engineWorkingEnabled)
                {
                    _engine.clip = _engineIdle;
                    _engineWorkingEnabled = false;
                    _engine.Play();
                }
            }
        }

        private void Awake()
        {
            _input = new Controls();
            _engine.clip = _engineIdle;
            _engine.Stop();
        }

        private void OnEnable()
        {
            _engine.Play();
        }

        private void OnDisable()
        {
            _engine.Stop();
        }
    }
}