using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Miner.Management.Events;
using Miner.Management.Exceptions;
using System;
using Random = UnityEngine.Random;
using Miner.Management;

namespace Miner.Gameplay
{
    public class NaturalDisastersController : MonoBehaviour
    {
        [SerializeField] protected GameEvent _disasterEnded = null;
        [SerializeField, Range(0f, 1f)] private float _probabilityOfDisaster = 0.05f;
        [SerializeField] private IntReference _playerLayer = null;
        [SerializeField] private GroundLayerList _layers = null;

        private float _countdownToOccasionalDisaster = 0f;
        private Coroutine _countdownCoroutine = null;
        private bool _occasionalDisasterInProggress = false;
        private bool _isOccasionalDisasterAvailable = false;

        private List<NaturalDisaster> _constantDisasters = new List<NaturalDisaster>();

        public void OnHourElapsed()
        {
            if (_isOccasionalDisasterAvailable == false) return;

            if (Random.Range(0f, 1f) < _probabilityOfDisaster)
            {
                if (_countdownCoroutine == null)
                {
                    _countdownToOccasionalDisaster = Random.Range(0f, 5f);
                    _countdownCoroutine = StartCoroutine(CountdownToBeginning());
                }
            }
        }

        public void OnPlayerCameToLayer(EventArgs args)
        {
            if(args is PlayerCameToLayerEA pctl)
            {
                for (int i = 0; i < _constantDisasters.Count; ++i)
                {
                    _constantDisasters[i].End();
                }

                int constantDisasterCounter = 0;
                for (int i = 0; i < pctl.GroundLayer.NaturalDisasters.Count; ++i)
                {
                    if(pctl.GroundLayer.NaturalDisasters[i].HappeningType == NaturalDisaster.EHappeningType.Constant)
                    {
                        NaturalDisaster nd = pctl.GroundLayer.NaturalDisasters[i];
                        nd.Execute();
                        _constantDisasters.Add(nd);
                        constantDisasterCounter++;
                    }
                }

                if (constantDisasterCounter == pctl.GroundLayer.NaturalDisasters.Count)
                    _isOccasionalDisasterAvailable = false;
                else
                    _isOccasionalDisasterAvailable = true;
            }
            else
            {
                Log.Instance.Write(GetType() + " : " + new InvalidEventArgsException().Message);
            }
        }

        private IEnumerator CountdownToBeginning()
        {
            while (_countdownToOccasionalDisaster > 0f)
            {
                _countdownToOccasionalDisaster -= Time.deltaTime;
                yield return null;
            }

            if (_layers[_playerLayer.Value].NaturalDisasters.Count > 0 && _occasionalDisasterInProggress == false)
            {
                _occasionalDisasterInProggress = true;
                int disasterIndex = Random.Range(0, _layers[_playerLayer.Value].NaturalDisasters.Count);
                NaturalDisaster nd = _layers[_playerLayer.Value].NaturalDisasters[disasterIndex];
                nd.Execute();
                _countdownCoroutine = StartCoroutine(CountdownToEnd(nd));
            }
            _countdownToOccasionalDisaster = 0f;
        }

        private IEnumerator CountdownToEnd(NaturalDisaster disaster)
        {
            yield return new WaitForSeconds(disaster.Time);
            disaster.End();
            _disasterEnded.Raise();
            _occasionalDisasterInProggress = false;
            _countdownCoroutine = null;
        }

        private void OnDestroy()
        {
            if (_countdownCoroutine != null)
            {
                StopCoroutine(_countdownCoroutine);
                _countdownCoroutine = null;
            }

            foreach(var disaster in _constantDisasters)
            {
                disaster.End();
            }
            _constantDisasters.Clear();
        }
    }
}