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
        [SerializeField] private AudioSource _rocket = null;
        [SerializeField] private AudioSource _drill = null;
        [SerializeField] private PlayerInputSheet _input = null;
        [SerializeField] private SoundEffect _rocketSound = null;
        [SerializeField] private AudioClip _engineWorking = null;
        [SerializeField] private AudioClip _engineIdle = null;
        [SerializeField] private SoundEffect _drillSound = null;
        private bool _engineWorkingEnabled = false;
        private Coroutine _drillCoroutine = null;

        public void OnLeadToDigPlace()
        {
            if (_drillCoroutine != null)
            {
                StopCoroutine(_drillCoroutine);
                _drillCoroutine = null;
            }
            _drill.volume = 1f;
            _drillSound.Play(_drill);
        }

        public void OnDigComplete()
        {
            _drillCoroutine = StartCoroutine(MuteDrill());
        }

        private IEnumerator MuteDrill()
        {
            while(_drill.volume > 0f)
            {
                _drill.volume -= 0.6f * Time.deltaTime;
                yield return null;
            }
            _drill.Stop();
            _drillCoroutine = null;
        }

        private void Update()
        {
            if (_input.VerticalMove > 0.1f)
            {
                if (!_rocket.isPlaying)
                    _rocketSound.Play(_rocket);
            }
            else
            {
                if (_rocket.isPlaying)
                    _rocket.Stop();
            }

            if(Mathf.Abs(_input.HorizontalMove) > 0.1f)
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
            _rocket.clip = _rocketSound.Sound;
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