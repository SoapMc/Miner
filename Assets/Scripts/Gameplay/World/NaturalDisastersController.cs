using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    public class NaturalDisastersController : MonoBehaviour
    {
        [SerializeField] private IntReference _timeOfDay = null;
        [SerializeField, Range(0f, 1f)] private float _probabilityOfDisaster = 0.05f;
        [SerializeField] private IntReference _playerLayer = null;
        [SerializeField] private GroundLayerList _layers = null;

        private float _countdownToDisaster = 0f;
        private Coroutine _countdownCoroutine = null;
        private bool _disasterInProggress = false;
        public void OnHourElapsed()
        {
            if (Random.Range(0f, 1f) < _probabilityOfDisaster)
            {
                if (_countdownCoroutine == null)
                {
                    _countdownToDisaster = Random.Range(0f, 5f);
                    _countdownCoroutine = StartCoroutine(Countdown());
                }
            }
        }

        private IEnumerator Countdown()
        {
            while (_countdownToDisaster > 0f)
            {
                _countdownToDisaster -= Time.deltaTime;
                yield return null;
            }

            if (_layers[_playerLayer.Value].NaturalDisasters.Count > 0 && _disasterInProggress == false)
            {
                _disasterInProggress = true;
                int disasterIndex = Random.Range(0, _layers[_playerLayer.Value].NaturalDisasters.Count);
                _layers[_playerLayer.Value].NaturalDisasters[disasterIndex].Execute();
            }
            _countdownToDisaster = 0f;
            _countdownCoroutine = null;
        }

        public void OnNaturalDisasterEnded()
        {
            _disasterInProggress = false;
        }

        private void OnDestroy()
        {
            if (_countdownCoroutine != null)
                StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }
    }
}