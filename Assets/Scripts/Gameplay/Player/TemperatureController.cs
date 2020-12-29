using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Miner.Management.Events;

namespace Miner.Gameplay
{
    public class TemperatureController : MonoBehaviour
    {
        [SerializeField] private GameEvent _damagePlayer = null;
        [SerializeField] private GameEvent _changeAlarms = null;
        [SerializeField] private FloatReference _internalTemperature = null;
        [SerializeField] private FloatReference _externalTemperature = null;
        [SerializeField] private IntReference _thermalInsulation = null;
        [SerializeField] private FloatReference _effectiveCooling = null;
        [SerializeField] private FloatReference _heatFlow = null;
        [SerializeField] private EquipmentTable _equipment = null;
        private Dictionary<EPartType, OverheatedPart> _overheatedParts = new Dictionary<EPartType, OverheatedPart>();
        private float _updateTime = 1f;
        private Coroutine _checkStatusCoroutine = null;

        #region EVENT RESPONSES
        public void OnPlayerRespawned()
        {
            StartCoroutine(WaitOneFrameAndSetInternalTemperature());
        }

        public void OnPlayerInstantiated()
        {
            StartCoroutine(WaitOneFrameAndSetInternalTemperature());
        }
        #endregion

        #region COROUTINES

        private IEnumerator WaitOneFrameAndSetInternalTemperature()
        {
            yield return null;
            _internalTemperature.Value = _externalTemperature;
        }

        private IEnumerator CheckStatus()
        {
            yield return new WaitForSeconds(_updateTime);
            while (true)
            {
                foreach(EPartType partType in Enum.GetValues(typeof(EPartType)))
                {
                    Part part = _equipment.GetEquippedPart(partType);
                    if(part != null)
                    {
                        IOverheatable oh = part.AsOverheatable();
                        if (oh != null)
                        {
                            if (_internalTemperature.Value > oh.MaximumOperatingTemperature)
                            {
                                if (!_overheatedParts.ContainsKey(partType))
                                {
                                    _overheatedParts.Add(partType, new OverheatedPart(oh, 0f));
                                    _changeAlarms.Raise(new ChangeAlarmsEA() { AddedAlarm = UI.AlarmDisplay.EAlarmType.TemperatureHigh });
                                }
                            }
                            else
                            {
                                if (_overheatedParts.ContainsKey(partType))
                                    _overheatedParts.Remove(partType);

                                if (_overheatedParts.Count == 0)
                                    _changeAlarms.Raise(new ChangeAlarmsEA() { RemovedAlarm = UI.AlarmDisplay.EAlarmType.TemperatureHigh });
                            }
                        }
                    }
                    
                }

                foreach(var elem in _overheatedParts)
                {
                    elem.Value.TimeOfOverheating += _updateTime;
                    if(elem.Value.TimeOfOverheating >= elem.Value.Overheatable.ToleranceTime)
                    {
                        DamagePlayerEA dp = new DamagePlayerEA();
                        dp.DealPermaDamage(new Tuple<EPartType, float>(elem.Key, 0.01f));
                        _damagePlayer.Raise(dp);
                        elem.Value.TimeOfOverheating = 0f;
                    }
                }

                yield return new WaitForSeconds(_updateTime);
            }
        }
        #endregion

        #region UNITY CALLBACKS
        private void Start()
        {
            _internalTemperature.Value = _externalTemperature;
        }

        private void OnEnable()
        {
            _checkStatusCoroutine = StartCoroutine(CheckStatus());
        }

        private void Update()
        {
            _heatFlow.Value = (_externalTemperature.Value - _effectiveCooling.Value - _internalTemperature.Value) * Time.deltaTime / (float)(_thermalInsulation.Value + 1);
            if (_internalTemperature.Value < 0f && _heatFlow.Value < 0f)
                return;
            _internalTemperature.Value += _heatFlow.Value;
        }

        private void OnDisable()
        {
            _heatFlow.Value = 0;
            if (_checkStatusCoroutine != null)
                StopCoroutine(_checkStatusCoroutine);
        }
        #endregion

        #region ADDITIONAL TYPES
        private class OverheatedPart
        {
            public float TimeOfOverheating;
            public IOverheatable Overheatable;

            public OverheatedPart(IOverheatable overheatable, float time)
            {
                TimeOfOverheating = time;
                Overheatable = overheatable;
            }
        }
        #endregion
    }
}